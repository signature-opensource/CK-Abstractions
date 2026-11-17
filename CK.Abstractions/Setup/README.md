# Assembly level attributes

This set of attributes are in CK.Setup namespace and apply to assemblies: they should be in
the conventional `AssemblyInfo.cs` file.

## SkippedAssemblyAttribute

The [[SkippedAssembly](SkippedAssemblyAttribute.cs)] allows an assembly to be totally ignored.
By default "Microsoft.*" and "System.*" assemblies are skipped. This is primarily an optimization:
only assemblies that are not skipped are analyzed, first for their assembly level attributes, and
when they are a Model, their types are collected.

## Model, ModelDefiner, Engine and RequiredEngine
A CKomposable assembly is either a Model or an Engine.
- A Model contains the regular code and will be deployed.
- An Engine contains the code that can generate code, documentation, etc. It is not deployed.

An assembly decorated with a [[IsModel](IsModelAttribute.cs)] adds the Types it contains
to the CKomposable engine and can also declare one or more required engines thanks
to [[RequiredEngine](RequiredEngineAttribute.cs)].

Engine assemblies must be decorated with [[IsEngine](IsEngineAttribute.cs)]. An engine can also
requires other engines - this can be used to _hide_ the dependency to an engine.

The [[IsModelDefiner](IsModelDefinerAttribute.cs)] is used by the most abstract assemblies:
they define attributes and abstract types (typically interfaces) but no directly usable, final, types.

The [[IsModel](IsModelAttribute.cs)] is often not required: an assembly that recursively depends
on a Model or ModelDefiner is a Model assembly. Similarly, an assembly that recursively
depends on an Engine is automatically an Engine assembly.

## RemoveModel, RemoveType and AddType
The [[RemoveModel](RemoveModelAttribute.cs)] attribute can "hide" a dependent Model: its types
are no more included by default. Instead of removing all the types of a Model, [[RemoveType](RemoveTypeAttribute.cs)]
can be used to remove one or more specific types.

Types that have been removed by a referencing assembly (with `[RemoveType]` or `[RemoveModel]`) can be added back
by a higher level referencing assembly thanks to [[AddType](AddTypeAttribute.cs)]. The `[AddType]` attribute
can also be used to configure a type for the Automatic Dependency Injection by specifying a
service [`kind`](../Core/AutoDI/ConfigurableAutoServiceKind.cs) for a type.

## PreserveAssemblyReferenceAttribute

This is a simple helper that prevents a reference to an assembly to be trimmed out by the compiler/linker
by referencing one of its type.
