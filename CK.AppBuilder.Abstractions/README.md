# CK.AppBuilder.Abstractions

The declarations of the CKomposable configuration, in one package: reading it is enough to know what can be configured
in a CKomposable solution and how. Like CK.Abstractions, it holds no logic: the App builder (CK.AppBuilder), the engines
and the tools (the configurator) give these declarations their meaning.

## The four levels

A configuration applies at four levels. Each level has its configurable types (a package decorated by the attribute of
its level), their options (packages configured in the section of their owner) and an owner kind (what the options of the
level attach to):

| Level | Configurable type | Option attribute | Owner kind | Built by |
|---|---|---|---|---|
| AppContext aspects | [`[AppContextAspect]`](AppAspects/AppContextAspectAttribute.cs) | [`[AppContextAspectOption]`](AppAspects/AppContextAspectOptionAttribute.cs) | [`IAppContextAspect`](AppAspects/IAppContextAspect.cs) | the Builder, once for all the Apps of its AppContext |
| App aspects | [`[AppAspect]`](AppAspects/AppAspectAttribute.cs) | [`[AppAspectOption]`](AppAspects/AppAspectOptionAttribute.cs) | [`IAppAspect`](AppAspects/IAppAspect.cs) | the Builder, once per App |
| Party features | [`[PartyFeature]`](Party/PartyFeatureAttribute.cs) | [`[PartyFeatureOption]`](Party/PartyFeatureOptionAttribute.cs) | [`IApplicationIdentityFeatureDriver`](Party/IApplicationIdentityFeatureDriver.cs) | the AppIdentity engine, per party |
| Client features | [`[PartyClientFeature]`](PartyClient/PartyClientFeatureAttribute.cs) | [`[PartyClientFeatureOption]`](PartyClient/PartyClientFeatureOptionAttribute.cs) | [`IPartyClientFeature`](PartyClient/IPartyClientFeature.cs) | the AppIdentity engine, per client of a party |

The owner kinds of the two aspect levels are markers: what an aspect does (its `Apply`) is engine side, in
`IAppAspectApply` and `IAppContextAspectApply` (CK.Engine.Configuration), because it works on the App builder model. The
party features and the client features are both party components ([`IPartyComponent`](Party/IPartyComponent.cs)): their
options are [`IPartyComponentOption<TOwner>`](Party/IPartyComponentOption.cs). A client also states facts about itself
with [`[PartyClientFact]`](PartyClient/PartyClientFactAttribute.cs) (a key and an optional value).

## Configurable types

[`IConfigurableTypeAttribute`](Configuration/IConfigurableTypeAttribute.cs) makes a package a configurable type: the configuration
names it (its `Name` is the key of its section), configures it and switches it (its `Activation`). **Nothing in this
package implements it.**

```csharp
public interface IConfigurableTypeAttribute : IOptionalResourceGroupAttribute
{
    string Name { get; }
    TypeActivation Activation { get; }
    bool IOptionalResourceGroupAttribute.IsOptional => Activation == TypeActivation.Optional;
}

public enum TypeActivation { None = 0, Optional, ByDefault, Required }
```

- **The types drive, the names configure.** The members of a set are given by types; the name is an alias of its type
  **in the configuration only**, for readability: it is never a reference (no relation names it, no export or import
  uses it). A name is valid only when its type is a member of the configured set: it never brings its type in.
- **The name is explicit**, read on the decorated type only (not on its base classes): it is a contract of the
  configuration that must not depend on a type name. It is unique in its scope (a party, a client, the section of an
  owner...): the engine of the model that defines the configurable types defines the scopes and checks them.
- Only a package (a concrete `IResourceGroup`) is a configurable type, with at most one such attribute.

The `Activation` is explicit (`None` is an error) and answers two questions with one declaration:

| Activation | Membership of a set | Run time, no key | `"X": {}` | `"X": true` | `"X": false` |
|---|---|---|---|---|---|
| `Optional` | only when rooted (declared by the set, or required by a member) | off | configures, on | on | warning (redundant) |
| `ByDefault` | by default | on | configures | warning (redundant) | off |
| `Required` | by default | on | configures | warning (redundant) | **error** |

The membership is the optionality of the package: it lets an assembly offer more than what a party eventually uses,
and keeps the unused code out. `IConfigurableTypeAttribute` **is** an `IOptionalResourceGroupAttribute` (CK.ResourceSpace.Abstractions) whose
`IsOptional` is its activation (`Optional`): the engines read the optionality of every package, configurable or not, by
one rule (an implementation must not override `IsOptional`).

### The options

The four levels of the configuration (the AppContext aspects, the App aspects, the party features and the client
features) have options: an [`IConfigurableOption<TOwner>`](Configuration/IConfigurableOption.cs) is a package
configured in the section of its owner, by its name, that applies to `TOwner` and to the owners derived from it. Its
attribute is an [`IConfigurableOptionAttribute`](Configuration/IConfigurableOptionAttribute.cs): a configurable
type attribute (its name is its key in the section of its owner, its activation makes it opt-in or opt-out) whose
`OwnerKind` is the kind of owner of its level, and the embedded resource attribute that the ordering of the options
requires. Each level specializes both, and its engine orders, selects and creates the options of each owner.

### The configuration models

A configurable type declares the model of its section with [`[Configuration<TModel>]`](Configuration/ConfigurationAttribute.cs):
a `partial` class whose properties are the keys of the section. Its owner receives the model, read and validated, instead
of reading its configuration, and the tools (the App builder, the configurator) know the section from the model's
description: both are generated at compile time by CK.ConfigurationModel. A property is a nested model (a list, a
dictionary of named models), a foreign options type whose properties are its shape, or an opaque `IConfiguration`
described by the JSON Schema that [`[ConfigurationSchema]`](Configuration/ConfigurationSchemaAttribute.cs) names.
`[ConfigurationLevel]`, `[ConfigurationSecret]` and `[ConfigurationPerDeployment]` decorate the properties of a model
(without a path: it is derived from the model).

A model may validate what its types can't say by implementing [`IConfigurationModel`](ConfigurationModel/IConfigurationModel.cs):
its `Validate` receives an [`IConfigurationValidationContext`](ConfigurationModel/IConfigurationValidationContext.cs) (the
`Path` of its section, `Error` and `Warn`, messages in the invariant culture). [`[ConfigurationModel]`](ConfigurationModel/ConfigurationModelAttribute.cs)
marks the model class itself; CK.ConfigurationModel reads, describes and validates the models.

### The configuration attributes

They describe the configuration of a configurable type, for the builder and the tools (in `Configuration`):

| Attribute | Declares |
|---|---|
| `[ConfigurationSection]` | The section of an App singleton (a root key of the App): its `[ConfigurationLevel]` properties are its values. |
| `[ConfigurationLevel]` | The capability of a property of a section: the highest `ConfigurationLevel` (Compile, Init, Run) that its code supports. |
| `[ConfigurationLock]` | A restriction: a value of the section of a configurable type, locked at a level (with an optional value) in every party of a kind. |
| `[ConfigurationSecret]` | Values of the section of a configurable type that are secrets: their configuration is a handle (`ConfigurationSecret`). |
| `[ConfigurationPerDeployment]` | Values of the section of a configurable type (or, on an assembly, of the parties of a kind) that each deployment writes itself. |
| `[assembly: CentralConfigurationKey]` | A root key of the central configuration that an engine reads. |
