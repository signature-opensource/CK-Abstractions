using System;

namespace CK.Setup;

/// <summary>
/// Declares one or more types from a referenced assembly that could have been hidden by <see cref="ExcludePFeatureAttribute"/>
/// or by a <see cref="ExcludeCKTypeAttribute"/> in referenced assemblies or are in a regular (non PFeature) assembly.
/// <list type="bullet">
///     <item>The types to register must be public (<see cref="Type.IsVisible"/>) otherwise it is a setup error.</item>
///     <item>They can belong to any assembly (excluded or not).</item>
/// </list>
/// This is a "weak" exclusion that excludes the types from the initial set to analyze. Other assemblies, engine configuration
/// or aspects can always register the types back.
/// <para>
/// This attribute can only decorate assemblies. The other attribute in CK.Core namespace <see cref="CK.Core.RegisterCKTypeAttribute"/>
/// can decorate classes or interfaces and defines an intrinsic regisrattion: the registered types are necessarily registered.
/// </para>
/// </summary>
[AttributeUsage( AttributeTargets.Assembly, AllowMultiple = true )]
public sealed class RegisterCKTypeAttribute : Attribute
{
    /// <summary>
    /// Initializes a new <see cref="RegisterCKTypeAttribute"/>.
    /// </summary>
    /// <param name="type">The first type to expose.</param>
    /// <param name="otherTypes">Other types to expose.</param>
    public RegisterCKTypeAttribute( Type type, params Type[] otherTypes )
    {
    }

}
