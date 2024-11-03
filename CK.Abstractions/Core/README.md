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

## ExcludeCKTypeAttribute
This attribute excludes the decorated type from Automatic DI discovery process. It doesn't cancel the
nature of the type (a public interface that is a ISingletonAutoService is what it is and any class that
implements it will be an auto service): this attribute should be used on classes and to have an effect
it must decorate a Type and all its specializations: exclusion doesn't propagate to specializations.

If this attribute can decorate an interface, it is to open a door to a future "strong", "propagatable",
"inheritable" exclusion and to handle an edge case: an excluded generic interface
definition propagates its exclusion to its closed generic types (that is also an interface).

After many years of experimentation we never met a really useful case for this "strong" exclusion.
Exclusion is rather exceptional and used in advanced scenario: explictly excluding types bottom up
in the hierarchy has never been an issue.

## PreserveAssemblyReferenceAttribute

This is a simple helper that avoids a reference to an assembly to be trimmed out by the compiler/linker.
