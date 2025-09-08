using CK.Core;
using System;

namespace CK.Setup;

/// <summary>
/// This attribute can:
/// <list type="number">
///     <item>
///         Explicitly consider one or more types from a referenced assembly that could have been hidden by <see cref="RemovePFeatureAttribute"/>
///         or by a <see cref="RemoveTypeAttribute"/> in referenced assemblies.
///     </item>
///     <item>
///         Specify a <see cref="ConfigurableAutoServiceKind"/> for a type: this allows well-known Dependency Injection types (provided by
///         the .NET framework) to be configured.
///     </item>
/// </list>
/// The types to register must be public (<see cref="Type.IsVisible"/>) otherwise it is a setup error.
/// They can belong to any assembly (removed or not), including regular (non PFeature) assembly.
/// </summary>
[AttributeUsage( AttributeTargets.Assembly, AllowMultiple = true )]
public sealed class AddTypeAttribute : Attribute
{
    /// <summary>
    /// Initializes a new <see cref="AddTypeAttribute"/> to add one or more types to consider.
    /// </summary>
    /// <param name="type">The first type to consider.</param>
    /// <param name="otherTypes">Other types to consider.</param>
    public AddTypeAttribute( Type type, params Type[] otherTypes )
    {
    }

    /// <summary>
    /// Initializes a new <see cref="AddTypeAttribute"/> that configures a DI related type.
    /// </summary>
    /// <param name="type">The type to configure.</param>
    /// <param name="kind">The service kind.</param>
    public AddTypeAttribute( Type type, ConfigurableAutoServiceKind kind )
    {
    }
}
