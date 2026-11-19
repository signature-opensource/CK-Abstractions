using System;

namespace CK.Core;

/// <summary>
/// A restriction declared by a package: on a configurable type (see <see cref="IConfigurableTypeAttribute"/>) whose
/// section is a key of a party (a feature), it locks a value of this section in every party of a kind. The builder
/// checks it at build time.
/// <para>
/// The <see cref="Path"/> starts with the kind of the parties (declared by the contributors: "Party", "Remote",
/// "RemoteTemplate", "OpenedRemoteTemplate"...), the rest is relative to the section of the type in the party:
/// <c>[ConfigurationLock( "OpenedRemoteTemplate:AutoTrustKey", ConfigurationLevel.Compile, Value = "Never" )]</c> on
/// the KeyManagement feature locks "KeyManagement:AutoTrustKey" of every Opened remote template.
/// </para>
/// <para>
/// With a <see cref="Value"/>, a configured value that differs is a build error, and an absent one is burnt with it.
/// </para>
/// </summary>
[AttributeUsage( AttributeTargets.Class, AllowMultiple = true, Inherited = false )]
public sealed class ConfigurationLockAttribute : Attribute
{
    /// <summary>
    /// Initializes a new restriction.
    /// </summary>
    /// <param name="path">The kind of the parties, then the path in the section of the type (':' separated).</param>
    /// <param name="level">The level to which the value is restricted.</param>
    public ConfigurationLockAttribute( string path, ConfigurationLevel level )
    {
        Path = path;
        Level = level;
    }

    /// <summary>
    /// Gets the kind of the parties, then the path in the section of the type (':' separated).
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// Gets the level to which the value is restricted.
    /// </summary>
    public ConfigurationLevel Level { get; }

    /// <summary>
    /// Gets or sets the required value. Null when only the level is restricted.
    /// </summary>
    public string? Value { get; set; }
}
