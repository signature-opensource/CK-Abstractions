namespace CK.Core;

/// <summary>
/// This interface can be implemented by attributes that revoke the mappings of the decorated type: it is not a
/// mapping key (when it is an abstraction) and it is not mapped to its abstractions (when it is a concrete type).
/// <para>
/// Only the decorated type is concerned: its specializations inherit the mapping cardinality (<see cref="ISingleMappingAttribute"/>,
/// <see cref="IMultipleMappingAttribute"/>) and are mapped. This is never inherited.
/// </para>
/// <para>
/// This doesn't remove the type from the types that an engine handles: the type can still be used by its own type
/// or name. It is typically used for a type that has mapped bases but is used manually, or for the root of a family
/// of abstractions that must not be an abstraction itself.
/// </para>
/// </summary>
public interface INotMappedAttribute
{
}
