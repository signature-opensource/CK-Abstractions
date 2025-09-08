using System;

namespace CK.Setup;

/// <summary>
/// Removes one or more types of automatic registration.
/// <para>
/// Removing a type, just like removing an assembly with <see cref="RemovePFeatureAttribute"/>, is
/// "weak", it impacts the initial type set that will be considered. Other assemblies, engine configuration
/// or aspects can always register the types back.
/// </para>
/// To be effective, removing applies "from the leaves": most specialized types must be removed for a "base" type
/// to also be removed.
/// </summary>
[AttributeUsage( AttributeTargets.Assembly, AllowMultiple = false )]
public class RemoveTypeAttribute : Attribute
{
    /// <summary>
    /// Initializes a new <see cref="RemoveTypeAttribute"/>.
    /// </summary>
    /// <param name="type">The first type to remove.</param>
    /// <param name="otherTypes">Other types to remove.</param>
    public RemoveTypeAttribute( Type type, params Type[] otherTypes )
    {
    }
}
