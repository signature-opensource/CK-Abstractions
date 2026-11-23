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
/// <para>
/// On an assembly, it is declared by the engine of the structure of the parties (the one that declares their anchors,
/// see the App builder contributors): the path is the kind of the parties, then the path in the party itself, not in
/// the section of a type: <c>[assembly: ConfigurationPerDeployment( "Remote:Address" )]</c>. The App builder reads it on
/// the assemblies of its contributors.
/// </para>
/// <para>
/// On a property of a configuration model (see <see cref="ConfigurationAttribute{TModel}"/>), it has no path: the property
/// is written by each deployment, its path is derived from the model.
/// </para>
/// </summary>
[AttributeUsage( AttributeTargets.Class | AttributeTargets.Assembly | AttributeTargets.Property, AllowMultiple = true, Inherited = false )]
public sealed class ConfigurationPerDeploymentAttribute : Attribute
{
    /// <summary>
    /// Initializes a new "per deployment" declaration.
    /// </summary>
    /// <param name="path">The kind of the parties, then the path in the section of the type, or in the party for an assembly (':' separated).</param>
    public ConfigurationPerDeploymentAttribute( string path )
    {
        Path = path;
    }

    /// <summary>
    /// Initializes a new "per deployment" declaration on a property of a configuration model.
    /// </summary>
    public ConfigurationPerDeploymentAttribute()
    {
        Path = string.Empty;
    }

    /// <summary>
    /// Gets the kind of the parties, then the path in the section of the type, or in the party for an assembly (':' separated).
    /// Empty on a property of a configuration model.
    /// </summary>
    public string Path { get; }
}
