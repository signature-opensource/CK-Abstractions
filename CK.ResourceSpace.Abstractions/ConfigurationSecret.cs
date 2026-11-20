using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;

namespace CK.Core;

/// <summary>
/// The secrets of the configuration. A secret is never written in a configuration file: its place holds a handle, the
/// name of the secret, that the host resolves when it starts:
/// <code>
/// "ClientSecret": { "$SECRET": "Duende:ClientSecret" }
/// </code>
/// In the flattened configuration, a handle is the key <c>"&lt;path&gt;:$SECRET"</c> whose value is the name: the
/// projections and the values burnt by the build carry it like any other value. Its value is the configuration key
/// <c>"Secrets:&lt;Name&gt;"</c>, read from any source of the host (environment variables, a vault...); in development,
/// the <see cref="DevFileName"/> file of the solution's "#Dev" folder holds them.
/// <para>
/// A name is a configuration path: segments separated by ':' made of letters, digits, '_' and '-' (an environment variable
/// can name it: "Secrets__Duende__ClientSecret").
/// </para>
/// <para>
/// An optional handle (<see cref="OptionalHandleKey"/>, <c>{ "$SECRET?": "Enlist:Worker2" }</c>) names a secret that may be
/// missing: its path is then absent (as if nothing were configured there), the feature behaves as when it is not
/// configured (a one-time enlistment ticket, an optional activation).
/// </para>
/// </summary>
public static class ConfigurationSecret
{
    /// <summary>
    /// The key of a handle: <c>{ "$SECRET": "&lt;Name&gt;" }</c>.
    /// </summary>
    public const string HandleKey = "$SECRET";

    /// <summary>
    /// The key of an optional handle: <c>{ "$SECRET?": "&lt;Name&gt;" }</c>. A missing secret leaves its path absent.
    /// </summary>
    public const string OptionalHandleKey = "$SECRET?";

    /// <summary>
    /// The configuration section that holds the values of the secrets, by name.
    /// </summary>
    public const string SectionName = "Secrets";

    /// <summary>
    /// The file of the solution's "#Dev" folder that holds the secrets of the development: its root is the content of
    /// the <see cref="SectionName"/> section (<c>{ "Duende": { "ClientSecret": "..." } }</c>).
    /// </summary>
    public const string DevFileName = "Secrets.json";

    /// <summary>
    /// Gets whether a name of secret is valid: segments separated by ':' made of letters, digits, '_' and '-'.
    /// </summary>
    /// <param name="name">The name to check.</param>
    /// <returns>True when the name is valid.</returns>
    public static bool IsValidName( [NotNullWhen( true )] string? name )
    {
        if( string.IsNullOrEmpty( name ) ) return false;
        foreach( var segment in name.Split( ':' ) )
        {
            if( segment.Length == 0 ) return false;
            foreach( var c in segment )
            {
                if( !char.IsAsciiLetterOrDigit( c ) && c != '_' && c != '-' ) return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Gets whether a configuration key is the key of a handle (<see cref="HandleKey"/> or <see cref="OptionalHandleKey"/>).
    /// </summary>
    /// <param name="key">The key (the last segment of a path).</param>
    /// <param name="isOptional">True for an optional handle.</param>
    /// <returns>True when the key is the key of a handle.</returns>
    public static bool IsHandleKey( string key, out bool isOptional )
    {
        isOptional = key.Equals( OptionalHandleKey, StringComparison.OrdinalIgnoreCase );
        return isOptional || key.Equals( HandleKey, StringComparison.OrdinalIgnoreCase );
    }

    /// <summary>
    /// Gets whether a path of the flattened configuration is a handle (<c>"&lt;path&gt;:$SECRET"</c> or
    /// <c>"&lt;path&gt;:$SECRET?"</c>).
    /// </summary>
    /// <param name="path">The configuration path.</param>
    /// <param name="valuePath">The path of the value that the handle replaces.</param>
    /// <returns>True when the path is a handle.</returns>
    public static bool IsHandlePath( string path, [NotNullWhen( true )] out string? valuePath ) => IsHandlePath( path, out valuePath, out _ );

    /// <summary>
    /// Gets whether a path of the flattened configuration is a handle (<c>"&lt;path&gt;:$SECRET"</c> or
    /// <c>"&lt;path&gt;:$SECRET?"</c>).
    /// </summary>
    /// <param name="path">The configuration path.</param>
    /// <param name="valuePath">The path of the value that the handle replaces.</param>
    /// <param name="isOptional">True for an optional handle.</param>
    /// <returns>True when the path is a handle.</returns>
    public static bool IsHandlePath( string path, [NotNullWhen( true )] out string? valuePath, out bool isOptional )
    {
        var i = path.LastIndexOf( ':' );
        if( i > 0 && IsHandleKey( path[(i + 1)..], out isOptional ) )
        {
            valuePath = path[..i];
            return true;
        }
        valuePath = null;
        isOptional = false;
        return false;
    }

    /// <summary>
    /// Reads a file of secrets (see <see cref="DevFileName"/>): a JSON object (comments and trailing commas are allowed)
    /// whose members are flattened into names (<c>{ "Duende": { "ClientSecret": "..." } }</c> is "Duende:ClientSecret").
    /// The values are strings, numbers or booleans. A missing file is an empty set.
    /// </summary>
    /// <param name="filePath">The path of the file.</param>
    /// <param name="secrets">The secrets by name (case insensitive).</param>
    /// <param name="error">The error when the file is invalid.</param>
    /// <returns>True on success, false when the file is invalid.</returns>
    public static bool TryReadFile( string filePath, out IReadOnlyDictionary<string, string> secrets, [NotNullWhen( false )] out string? error )
    {
        var result = new Dictionary<string, string>( StringComparer.OrdinalIgnoreCase );
        secrets = result;
        error = null;
        if( !File.Exists( filePath ) ) return true;
        try
        {
            using var doc = JsonDocument.Parse( File.ReadAllText( filePath ),
                                                new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true } );
            if( doc.RootElement.ValueKind != JsonValueKind.Object )
            {
                error = $"'{filePath}' must be a JSON object.";
                return false;
            }
            return Flatten( doc.RootElement, null, result, filePath, ref error );
        }
        catch( Exception ex ) when( ex is JsonException or IOException or UnauthorizedAccessException )
        {
            error = $"Unable to read '{filePath}': {ex.Message}";
            return false;
        }

        static bool Flatten( JsonElement o, string? prefix, Dictionary<string, string> result, string filePath, ref string? error )
        {
            foreach( var p in o.EnumerateObject() )
            {
                var name = prefix == null ? p.Name : $"{prefix}:{p.Name}";
                switch( p.Value.ValueKind )
                {
                    case JsonValueKind.Object:
                        if( !Flatten( p.Value, name, result, filePath, ref error ) ) return false;
                        break;
                    case JsonValueKind.String:
                    case JsonValueKind.Number:
                    case JsonValueKind.True:
                    case JsonValueKind.False:
                        if( !IsValidName( name ) )
                        {
                            error = $"Invalid secret name '{name}' in '{filePath}': segments separated by ':' made of letters, digits, '_' and '-' are expected.";
                            return false;
                        }
                        result[name] = p.Value.ValueKind == JsonValueKind.String ? p.Value.GetString()! : p.Value.GetRawText();
                        break;
                    default:
                        error = $"Invalid value of '{name}' in '{filePath}': a secret is a string, a number or a boolean.";
                        return false;
                }
            }
            return true;
        }
    }
}
