using System;

namespace CK.Setup;

/// <summary>
/// Excludes one or more types of automatic registration.
/// <para>
/// Excluding a type, just like excluding an assembly with <see cref="ExcludePFeatureAttribute"/>, is
/// "weak", it impacts the initial type set that will be considered. Other assemblies, engine configuration
/// or aspects can always register the types back.
/// </para>
/// Exclusion applies "from the leaves": most specialized types must be excluded for a "base" type
/// to also be excluded.
/// </summary>
[AttributeUsage( AttributeTargets.Assembly, AllowMultiple = false )]
public class ExcludeCKTypeAttribute : Attribute
{
    /// <summary>
    /// Initializes a new <see cref="ExcludeCKTypeAttribute"/>.
    /// </summary>
    /// <param name="type">The first type to exclude.</param>
    /// <param name="otherTypes">Other types to exclude.</param>
    public ExcludeCKTypeAttribute( Type type, params Type[] otherTypes )
    {
    }
}
