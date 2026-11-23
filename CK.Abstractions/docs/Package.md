The contracts of the Real Objects, Poco and Auto Services features of the CKomposable framework:
`IRealObject` (true singletons modeling the outside world), `IPoco` (data transfer objects any package can
enrich), `IAutoService` with its scoped, singleton and ambient variants (automatic DI configuration), the
assembly-level attributes (`[IsModel]`, `[SkippedAssembly]`, `[AddType]`...), the `EngineAttribute` base class the
mapping contracts of abstractions (`ISingleMappingAttribute`, `IMultipleMappingAttribute`, `INotMappedAttribute`), and
the configuration contracts: the configurable types (`IConfigurableTypeAttribute`, `TypeActivation`), their options
(`IConfigurableOption<TOwner>`, `IConfigurableOptionAttribute`) and the `[Configuration...]` attributes.
