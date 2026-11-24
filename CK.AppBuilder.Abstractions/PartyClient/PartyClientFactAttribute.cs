using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace CK.AppIdentity;

/// <summary>
/// States a fact about the party clients built from the decorated type: the engine that builds a kind of client (the
/// TypeScript engine for the client applications, for instance) unions the facts of the types a client is built from,
/// once per build, into the facts of the client (<c>ClientApplicationDescription.Facts</c> of CK.AppIdentity.Abstractions).
/// The client announces them with its party's description, so that the other parties know what it is.
/// <para>
/// A fact is a key with an optional value (see <see cref="IsValidKey(string?)"/> and <see cref="IsValidValue(string?)"/>):
/// <c>[PartyClientFact( "Admin" )]</c>, <c>[PartyClientFact( "Enlistment", "app-identity/enlist" )]</c>. The attribute can
/// be repeated, and it is not inherited.
/// </para>
/// </summary>
[AttributeUsage( AttributeTargets.Class, AllowMultiple = true, Inherited = false )]
public sealed class PartyClientFactAttribute : Attribute
{
    /// <summary>
    /// The maximal length of a fact value.
    /// </summary>
    public const int MaxValueLength = 256;

    /// <summary>
    /// Initializes a new fact.
    /// </summary>
    /// <param name="key">The key: a non empty name without whitespace, '=' nor ','.</param>
    /// <param name="value">The value: empty for a fact without value.</param>
    public PartyClientFactAttribute( string key, string value = "" )
    {
        if( !IsValidKey( key ) ) throw new ArgumentException( "A fact key is a non empty name without whitespace, '=' nor ','.", nameof( key ) );
        if( !IsValidValue( value ) ) throw new ArgumentException( $"A fact value has at most {MaxValueLength} characters and no control character.", nameof( value ) );
        Key = key;
        Value = value;
    }

    /// <summary>
    /// Gets the key of the fact.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Gets the value of the fact: empty for a fact without value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Gets whether a string is a valid key: a non empty name without whitespace, '=' nor ','.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>True when valid.</returns>
    public static bool IsValidKey( [NotNullWhen( true )] string? key ) => !string.IsNullOrEmpty( key )
                                                                          && !key.Any( c => char.IsWhiteSpace( c ) || char.IsControl( c ) || c == '=' || c == ',' );

    /// <summary>
    /// Gets whether a string is a valid value: at most <see cref="MaxValueLength"/> characters without control character
    /// (the empty string is the value of a fact without value).
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>True when valid.</returns>
    public static bool IsValidValue( [NotNullWhen( true )] string? value ) => value != null
                                                                              && value.Length <= MaxValueLength
                                                                              && !value.Any( char.IsControl );
}
