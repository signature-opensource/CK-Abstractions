using System;

namespace CK.Core;

/// <summary>
/// Declares that values of the section of a configurable type (see <see cref="IConfigurableTypeAttribute"/>) are secrets:
/// their configuration should be a handle (<c>{ "$SECRET": "&lt;Name&gt;" }</c>, see <see cref="ConfigurationSecret"/>),
/// never the value itself. The builder checks it at build time: a plain value is reported.
/// <para>
/// The <see cref="Path"/> is built like the one of <see cref="ConfigurationLockAttribute"/>: it starts with the kind of
/// the parties ("Party", "Remote"...), the rest is relative to the section of the type, and may contain the patterns of
/// the configuration paths (<c>*</c> for any member, <c>[*]</c> for any item):
/// <c>[ConfigurationSecret( "Party:Oidc:*:ClientSecret" )]</c>.
/// </para>
/// <para>
/// On a property of a configuration model (see <see cref="ConfigurationAttribute{TModel}"/>), it has no path: the property
/// is a secret, its path is derived from the model.
/// </para>
/// </summary>
[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true, Inherited = false )]
public sealed class ConfigurationSecretAttribute : Attribute
{
    /// <summary>
    /// Initializes a new secret declaration.
    /// </summary>
    /// <param name="path">The kind of the parties, then the path in the section of the type (':' separated).</param>
    public ConfigurationSecretAttribute( string path )
    {
        Path = path;
    }

    /// <summary>
    /// Initializes a new secret declaration on a property of a configuration model.
    /// </summary>
    public ConfigurationSecretAttribute()
    {
        Path = string.Empty;
    }

    /// <summary>
    /// Gets the kind of the parties, then the path in the section of the type (':' separated). Empty on a property of a
    /// configuration model.
    /// </summary>
    public string Path { get; }
}
