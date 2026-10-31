namespace CK.Core;

/// <summary>
/// This interface can be implemented by attributes that make the decorated interface or abstract class an abstraction
/// with a multiple mapping: all its implementations are mapped to it.
/// <para>
/// The cardinality is inherited: the abstractions that specialize this one also have a multiple mapping, unless they are
/// decorated with an <see cref="ISingleMappingAttribute"/> (a single abstraction can specialize a multiple one).
/// </para>
/// <para>
/// The mapping is automatic, the membership is not: an engine maps the implementations it selects, it doesn't select
/// a type because it implements an abstraction. See <see cref="INotMappedAttribute"/> to revoke the mapping of a type.
/// </para>
/// <para>
/// <see cref="IsMultipleAttribute"/> implements this interface.
/// </para>
/// </summary>
public interface IMultipleMappingAttribute
{
}
