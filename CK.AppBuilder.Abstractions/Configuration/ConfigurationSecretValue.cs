using System;

namespace CK.Core;

/// <summary>
/// The value of a secret of a configuration model that is read before the handles are resolved (an AppContext aspect: the
/// build reads the configuration files): either the value itself, or the name of the secret of its handle
/// (<c>{ "$SECRET": "&lt;Name&gt;" }</c>, see <see cref="ConfigurationSecret"/>). The consumer resolves a handle when it
/// needs the value, or projects it as a handle. At run time, the handles are resolved: it is a <see cref="Value"/>.
/// <para>
/// A property of this type is a <see cref="ConfigurationSecretAttribute"/>: <c>[ConfigurationSecret] public
/// ConfigurationSecretValue? ConnectionString { get; init; }</c>.
/// </para>
/// </summary>
public readonly struct ConfigurationSecretValue : IEquatable<ConfigurationSecretValue>
{
    readonly string? _value;
    readonly string? _secretName;

    ConfigurationSecretValue( string? value, string? secretName )
    {
        _value = value;
        _secretName = secretName;
    }

    /// <summary>
    /// Creates a value written in the configuration.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The secret value.</returns>
    public static ConfigurationSecretValue FromValue( string value )
    {
        ArgumentNullException.ThrowIfNull( value );
        return new ConfigurationSecretValue( value, null );
    }

    /// <summary>
    /// Creates a handle: the name of the secret.
    /// </summary>
    /// <param name="secretName">The name of the secret (see <see cref="ConfigurationSecret.IsValidName(string?)"/>).</param>
    /// <returns>The secret value.</returns>
    public static ConfigurationSecretValue FromHandle( string secretName )
    {
        if( !ConfigurationSecret.IsValidName( secretName ) ) throw new ArgumentException( $"Invalid secret name '{secretName}'.", nameof( secretName ) );
        return new ConfigurationSecretValue( null, secretName );
    }

    /// <summary>
    /// Gets the value. Null when this is a handle.
    /// </summary>
    public string? Value => _value;

    /// <summary>
    /// Gets the name of the secret. Null when this is a value.
    /// </summary>
    public string? SecretName => _secretName;

    /// <summary>
    /// Gets whether this is a handle: the value is the one of the secret <see cref="SecretName"/>.
    /// </summary>
    public bool IsHandle => _secretName != null;

    /// <inheritdoc />
    public bool Equals( ConfigurationSecretValue other ) => _value == other._value && _secretName == other._secretName;

    /// <inheritdoc />
    public override bool Equals( object? obj ) => obj is ConfigurationSecretValue v && Equals( v );

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine( _value, _secretName );

    /// <summary>
    /// Gets the handle, or "***" for a value: a secret is never displayed.
    /// </summary>
    /// <returns>A display string.</returns>
    public override string ToString() => _secretName != null ? $"{{ \"{ConfigurationSecret.HandleKey}\": \"{_secretName}\" }}" : "***";
}
