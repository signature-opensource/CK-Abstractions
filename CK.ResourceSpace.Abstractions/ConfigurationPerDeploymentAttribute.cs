using System;

namespace CK.Core;

/// <summary>
/// Declares that values of the section of a configurable type (see <see cref="IConfigurableTypeAttribute"/>) are "per
/// deployment": an address, a public URL... Each deployment of a solution (a folder of its "Deployment/" folder, an overlay
/// of its central configuration) must write them itself: inheriting them from the development configuration, or from
/// another deployment, is a build error.
/// <para>
/// The <see cref="Path"/> is built like the one of <see cref="ConfigurationLockAttribute"/>: it starts with the kind of
/// the parties ("Party", "Remote"...), the rest is relative to the section of the type, and may contain the patterns of
/// the configuration paths (<c>*</c> for any member, <c>[*]</c> for any item):
/// <c>[ConfigurationPerDeployment( "Party:Urls" )]</c>.
/// </para>
/// </summary>
[AttributeUsage( AttributeTargets.Class, AllowMultiple = true, Inherited = false )]
public sealed class ConfigurationPerDeploymentAttribute : Attribute
{
    /// <summary>
    /// Initializes a new "per deployment" declaration.
    /// </summary>
    /// <param name="path">The kind of the parties, then the path in the section of the type (':' separated).</param>
    public ConfigurationPerDeploymentAttribute( string path )
    {
        Path = path;
    }

    /// <summary>
    /// Gets the kind of the parties, then the path in the section of the type (':' separated).
    /// </summary>
    public string Path { get; }
}
