The contracts that let a type declare which resources it owns and where it sits among the others:
`IResourceGroup` and `IResourcePackage` (a child may be in many groups but in at most one package), and the
`[Package]`, `[Children]`, `[Groups]`, `[Requires]` and `[RequiredBy]` attributes (with types or package full names),
plus their `[Optional...]` variants that apply only if the named package is part of the selection.
An attribute defined elsewhere can declare the same relations by implementing `IResourceGroupTopologyAttribute`.
