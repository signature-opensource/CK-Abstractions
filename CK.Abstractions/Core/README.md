# Core types

This set of interfaces and attributes are in CK.Core namespace and appears in code base.
The Setup set of attributes applies to assemblies (in the conventional `AssemblyInfo.cs` file).

## CKTypeDefinerAttribute
Attribute that marks a type as being a "Definer" of a `IRealObject`, `IPoco` or `IAutoService`.
This defines the decorated object as a "base type" of the CK type: it is not itself a `IAutoService`, `IPoco` or `IRealObject` type but
its specializations are (unless they also are decorated with this attribute).
This is a little bit like an 'abstract' type regarding Auto Services, Poco or Real Objects.

## CKTypeSuperDefinerAttribute
Attribute that marks a type as being a "Definer of Definer". When `CKTypeDefinerAttribute` is the "father"
of the actual CK type, this one acts as a "grand father".

## ExcludedCKTypeAttribute
This attribute excludes the decorated type from Automatic DI discovery process. It doesn't cancel the
nature of the type (a public interface that is a ISingletonAutoService is what it is and any class that
implements it will be an auto service): this attribute should be used on classes and to have an effect
it must decorate a Type and all its specializations: exclusion doesn't propagate to specializations.

If this attribute can decorate an interface, it is to open a door to a future "strong", "propagatable",
"inheritable" exclusion and to handle an edge case: an excluded generic interface
definition propagates its exclusion to its closed generic types (that is also an interface).

Exclusion is rather exceptional and used in advanced scenario: types are excluded explicitly, bottom up
in the hierarchy.

## Mappings: ISingleMappingAttribute, IMultipleMappingAttribute and INotMappedAttribute
These [Mapping](Mapping) contracts are implemented by attributes defined elsewhere. They describe how abstractions
(interfaces and abstract classes) are mapped to their implementations, for any engine that handles mappings:

- An [`ISingleMappingAttribute`](Mapping/ISingleMappingAttribute.cs) makes the decorated abstraction a single one:
  at most one implementation is mapped to it.
- An [`IMultipleMappingAttribute`](Mapping/IMultipleMappingAttribute.cs) makes it a multiple one: all its
  implementations are mapped to it. [`[IsMultiple]`](AutoDI/IsMultipleAttribute.cs) implements it.
- An [`INotMappedAttribute`](Mapping/INotMappedAttribute.cs) revokes the mappings of the decorated type only:
  it is not an abstraction to map to, and not an implementation mapped to its abstractions.

The cardinality is inherited by the specializations: a single abstraction can specialize a multiple one, the reverse
is an error. The revocation is never inherited: the specializations of a not mapped type are mapped.

The mapping is automatic, the membership is not: an engine maps the implementations it selects, it never selects a
type because it implements an abstraction.

## AlsoRegisterTypeAttribute&lt;T&gt;
[[AlsoRegisterType&lt;T&gt;](AlsoRegisterTypeAttributes.cs)] enables any selected type to indicate that one or
more other types must also be considered. It exists with 1 to 8 generic parameters
(`[AlsoRegisterType<T1, T2, ..., T8>]`), can decorate a class, an interface, a struct or an enum and can
be applied multiple times.
