using System;

namespace CK.Core;

/// <summary>
/// The <see cref="ISingleMappingAttribute"/> of the type system: the decorated interface or abstract class is an
/// abstraction with a single mapping, at most one implementation is mapped to it (in the scope of the engine that
/// handles it: the resource space, the type sets...). It is the counterpart of <see cref="IsMultipleAttribute"/>.
/// <para>
/// The cardinality is inherited by the abstractions that specialize the decorated one (see <see cref="ISingleMappingAttribute"/>).
/// </para>
/// <para>
/// This attribute has no effect on the Automatic DI engine (where the mappings of an interface are unique unless it is
/// <see cref="IsMultipleAttribute"/>).
/// </para>
/// </summary>
[AttributeUsage( AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = false, Inherited = false )]
public sealed class IsSingleAttribute : Attribute, ISingleMappingAttribute
{
}
