using System;

namespace CK.Core;

/// <summary>
/// Declares a root key of the central configuration of a solution (its root "ckonf.json") that the engine of this
/// assembly reads: the App builder claims it for the App builder contributors of the assembly (a root key that no
/// contributor claims is an error). Declared on an engine assembly, it is also what the tools know of the central
/// configuration (which package owns which key) without running the engine.
/// </summary>
[AttributeUsage( AttributeTargets.Assembly, AllowMultiple = true, Inherited = false )]
public sealed class CentralConfigurationKeyAttribute : Attribute
{
    /// <summary>
    /// Initializes a new central configuration key declaration.
    /// </summary>
    /// <param name="key">The root key.</param>
    public CentralConfigurationKeyAttribute( string key )
    {
        Key = key;
    }

    /// <summary>
    /// Gets the root key.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Gets or sets the patterns of the values per deployment under the key, relative to it (':' separated, <c>*</c>
    /// for any member, <c>[*]</c> for any item): each deployment of the solution must write them itself
    /// (<c>"[*]:Link:Address"</c> under "Links").
    /// </summary>
    public string[]? PerDeployment { get; set; }
}
