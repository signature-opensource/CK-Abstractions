# CK.ResourceSpace.Abstractions

The contracts that let a type declare **which resources it owns and where it sits among the others**.
Two marker interfaces and a set of attributes; the engine that reads them ships elsewhere.

## A group, and the package that owns its children

The difference between the two markers is a single ownership rule:

```csharp
/// <summary>
/// Marks a type as being a group of resources. Such types must also be decorated
/// with at least one attribute that is a <see cref="IEmbeddedResourceTypeAttribute"/>.
/// <para>
/// A group, as opposed to a package, can contain any number of other <see cref="IResourceGroup"/>
/// or <see cref="IResourcePackage"/> (its Children) that are free to also belong to other <see cref="IResourceGroup"/>.
/// </para>
/// </summary>
public interface IResourceGroup { }
```

```csharp
/// <summary>
/// Marks a type as being a <see cref="IResourceGroup"/> but restricts its Children to
/// not also belong to another <see cref="IResourcePackage"/>: a package is the single owner
/// of its children.
/// </summary>
public interface IResourcePackage : IResourceGroup { }
```

So a child may appear in several groups, but in **at most one package**. That single rule is what makes
"who owns this file" answerable, while still letting groups overlap freely for anything else.

Note the second requirement in the comment above: implementing [`IResourceGroup`](IResourceGroup.cs) is
not enough, the type must also carry an attribute that is an `IEmbeddedResourceTypeAttribute` - that is
what says where the resources actually come from.

## Declaring the topology

Five families in [TopologyAttributes](TopologyAttributes), all `AttributeTargets.Class`:

| Attribute | Declares | Multiple? |
|-----------|----------|-----------|
| [`[Package]`](TopologyAttributes/PackageAttribute.cs) | *"its single `IResourcePackage` owner"* | no |
| [`[Children]`](TopologyAttributes/ChildrenAttribute.cs) | *"other package full names that the decorated type contains"* | yes |
| [`[Groups]`](TopologyAttributes/GroupsAttribute.cs) | *"`IResourceGroup` package full names to which the decorated type belongs"* | yes |
| [`[Requires]`](TopologyAttributes/RequiresAttribute.cs) | *"any number of package full names"* | yes |
| [`[RequiredBy]`](TopologyAttributes/RequiredByAttribute.cs) | *"any number of reverse dependencies"* | yes |

Each comes in two forms, and `[Package]` is the exception in both.

The **string** form of the four multiple families takes `params string[]`, where *"each string can be a
single package full name or comma separated multiple package full names"* - so
`[Children( "A.B, A.C", "A.D" )]` names three. `[Package]` takes a single `string packageFullName`:
one owner, one name.

The **generic** form names the types directly and comes in six arities, `<T>` through
`<T1,T2,T3,T4,T5,T6>` - except `[Package<T>]`, single-arity and constrained
`where T : IResourcePackage`. Every family recommends it over the string form, in its own words:
`Requires` and `RequiredBy` say it *"should be preferred when the types are accessible"*, `Children` and
`Groups` say the same of the children types and of the types of the groups, and `Package` goes further -
*"The typed attribute `PackageAttribute{T}` should almost always be preferred."*

The string form remains necessary: a package full name can be written down even when its type is not
accessible from this assembly.

## The optional variants

[OptionalTopologyAttributes](OptionalTopologyAttributes) mirrors four of the five: `OptionalChildren`,
`OptionalGroups`, `OptionalRequires`, `OptionalRequiredBy`. There is no `OptionalPackage`.

What "optional" qualifies here is **the edge, not the node**. Every one of the four ends its summary on
the same condition, in four slightly different words: *"IF the full name belongs to the package
selection"* for `OptionalRequires` and `OptionalRequiredBy`, *"IF the child belongs to the package
selection"* for `OptionalChildren`, *"IF the child type belongs to the package selection"* for
`OptionalGroups`. Name a package that is not in the selection and the declaration is dropped rather than
left unresolved.

Same two forms as above - `params string[]` and six generic arities.

A separate contract, [`IOptionalResourceGroupAttribute`](IOptionalResourceGroupAttribute.cs), covers the
other meaning of optional - a whole *type* that is opt-in. **Nothing in this package implements it**: it
is here for attributes defined elsewhere, and its comment states the rules an implementer must honour:

- *"Such opt-in groups or packages must be explicitly registered as non-optional or required by another
  registered component, otherwise they are ignored."*
- *"There should usually be at most one such attribute that decorate a Type but if there are mutiple
  ones, all of them must have a true `IsOptional` for the package to be considered optional by default."*
- *"There is no such thing as an 'optional abstraction', an abstraction is always available unless it has
  no registered 'implementation'."*

## Requires.

- `CK.EmbeddedResources.Abstractions`, which defines the `IEmbeddedResourceTypeAttribute` that
  `IResourceGroup` requires.
