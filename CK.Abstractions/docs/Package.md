The contracts of the Real Objects, Poco and Auto Services features of the CKomposable framework.

Markers that the engine interprets: `IRealObject` for the true singletons that model the outside world
and form the backbone of the system, `IPoco` for the data transfer objects any package can enrich
independently, and `IAutoService` with its scoped, singleton and ambient variants, which let the DI
container be configured automatically rather than by hand.

Also the assembly-level attributes that scope what the engine collects, and the base classes that bind
an attribute to the engine implementation that gives it behaviour.
