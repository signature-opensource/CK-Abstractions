namespace CK.Core;

/// <summary>
/// This interface can be implemented by attributes that make the decorated interface or abstract class an abstraction
/// with a single mapping: at most one implementation is mapped to it.
/// <para>
/// The cardinality is inherited: the abstractions that specialize this one also have a single mapping (unless they are
/// themselves decorated with an <see cref="IMultipleMappingAttribute"/>, which is an error: there cannot be multiple
/// implementations of an abstraction that has at most one).
/// </para>
/// <para>
/// The mapping is automatic, the membership is not: an engine maps the implementations it selects, it doesn't select
/// a type because it implements an abstraction. See <see cref="INotMappedAttribute"/> to revoke the mapping of a type.
/// </para>
/// </summary>
public interface ISingleMappingAttribute
{
}
