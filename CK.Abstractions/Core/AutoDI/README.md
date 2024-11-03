# Automatic Dependency Injection contracts

This set of interfaces and attributes are pure markers.
These types are the heart of the Automatic DI that is handled by CK.Engine.

## IRealObject

This interface marker states that a class or an interface instance
is a gateway to an external resource or a foundation of the architecture.
Such foundations are necessarily unique instance in a "context" that is defined
by the "CKomposableMap".

If the lifetimes of this [IRealObject](IRealObject.cs) and [ISingletonAutoService](ISingletonAutoService.cs)
instances are the same, their roles are different as well as the way they are handled.

Real objects must be used to model actual singletons "in real life", typically external resources
with which the system interacts such as a data repository (a database is a real object as well as the tables
it contains) or an external service.

Singleton services are more classical services that happens to be able to be shared by different
activities because of their thread safety and the fact that they depend only on other singleton
services or real objects.

"Normal" singleton services rely on "normal" constructor injection but real objects cannot have constructors:
it is somehow replaced by the optional `static public Requires( ... )` method that isolates dependencies
between a generalization and its specializations.

This `Requires` method is not the only `static public` method that **can** be defined on a Real Object.
These methods cannot be formalized ([static abstract members](https://blog.ndepend.com/c-11-static-abstract-members/)
cannot be used) as most of them can have arbitrary parameters.

All these methods are optional but all of them:
- Must define their first parameter as a Real Object instance of their type.
- Can always define a `IActivityMonitor` parameter.

> **Note:** This documents the next fully refactored/revisited "Twenty5" version of the CK stack. Not the current
(crappy) implementation that relies on private instance methods.

### Public static methods.

Real objects support the optional, public static following methods:

 - `public static void Requires( RealObject o, ...)`
is the method that defines the dependencies: its parameters are other IRealObjects on which this object depends,
the parameters drive the topological sort and there is no real point to put code in it, except that
references to dependencies may be captured here if needed.

 - `public static void Initialize( RealObject o, ICKomposableMap )`
is called at runtime once all the real objects have been instantiated and are available.

Defining this method requires a dependency to CK.ActivityMonitor and CK.StObj.Model packages.

 - `public static void RegisterStartupServices( RealObject o, SimpleServiceContainer )`
is called at runtime once all the real objects have been initialized and can register any services in the [SimpleServiceContainer](../ServiceContainer/SimpleServiceContainer.cs)
that may be used by the following `ConfigureServices` methods.

Defining this method requires a dependency to CK.ActivityMonitor package.

- `public static void ConfigureServices( RealObject o, IServiceCollection, ... )`
enables real objects to configure the DI (registering new services, configuring things, etc.) based on any number of parameters that
can be any other real objects and/or startup services previously registered.

Defining this method requires a dependency to CK.StObj.Model package.

- `public static void OnHostStart/Stop( RealObject o, ... )` or `Task/ValueTask OnHostStart/StopAsync( RealObject o, ... )`
enables real objects to act like hosted services: `OnHostStart[Async]` and `OnHostStop[Async]` are called
by an automatically generated Microsoft.Extensions.Hosting.IHostedService on startup (resp. on host stop).

The parameters can be any scope or singleton service that may be available from the global DI container:
a temporary scope is created to call all the Start (resp. Stop) methods and these methods' execution
share this temporary scope.

Defining this private method doesn't require any direct dependency but these methods requires the https://www.nuget.org/packages/Microsoft.Extensions.Hosting.Abstractions/
package to be available in the final application (otherwise a compilation error of the generated code will be raised).

> Note that a class that is a IRealObject can perfectly implement a IAutoService: this will be
resolved as a singleton (a ISingletonAutoService) and will be a potential implementation of the service
(that may be replaced). This applies only to class, not to interface: an interface cannot be both
a IAutoService and a IRealObject.

## IAutoService
This interface marker states that a class or an interface instance is a service that will participate in automatic dependency injection.

This marker doesn't indicate the scoped vs. singleton lifetime. The actual
lifetime depends on the final implementation that may be marked with the more
specific [ISingletonAutoService](ISingletonAutoService.cs) or [IScopedAutoService](IScopedAutoService.cs)
(but not both) or by analyzing its single constructor parameters: if and only if all parameters
are known to be singletons, the service will be singleton otherwise it will be considered as a Scoped one.

## ISingletonAutoService : IAutoService
This interface marker states that a class or an interface instance
must be a globally unique Service in a context, just like IRealObject.

## IScopedAutoService : IAutoService

This interface marker states that a class or an interface instance
must be a unique Service in a scope.

Note that even if an implementation only relies on other singletons,
this interface forces the service to be scoped.

If there is no specific constraint, the IAutoService marker
should be used for abstractions so that its scoped vs. singleton lifetime is
either determined by the final, actual, implementation that can be automatically
detected based on its constructor dependencies and/or by the way this Service is
used referenced by the other participants.

## ContainerConfiguredScopedServiceAttribute & ContainerConfiguredSingletonServiceAttribute

Marks a class or an interface to be configured by the DI container: it is not necessarily
available from all DI containers that may exist in an application. Note that defining a service
as being "container configured" requires to declare its lifetime (hence the 2 attributes).

When a `IAutoService` is also a container configured service, it is available in the global DI container
but not in other contaiers: it is up to the other containers to explicitly support the service by
registering the abstractions and/or implementations in its own service collection.

## IsMultipleAttribute
Marks an interface or an abstract class so that all its mappings to concrete classes must be automatically
registered and made available as a `IEnumerable<T>`.

When the decorated type is a IAutoService or a IRealObject (regardless of whether the type an interface or an
abstract class), this implies that the type is a `[CKTypeDefiner]`: this is handled automatically.

This attribute cancels the implicit unicity of the mapping but doesn't impact the lifetime:
the `IEnumerable<T>` will be a singleton if all the implementations are singletons otherwise
it will be a scoped service.

> IPoco interfaces cannot use the `[IsMultiple]` attribute, instead `[CKTypeDefiner]` must be used.

## ReplaceAutoServiceAttribute

Optional attribute for `IAutoService` **implementation** that
declares that this implementation replaces another one (the replaced implementation type
is the single constructor parameter).

Note that this attribute is useless if this implementation specializes the replaced service since
discovering the most precise implementation is one of the key goal of Auto services handling.

It is also useless if the replaced service is used by this implementation: as long as a parameter with the
same type appears in its constructor, this service "covers" (and possibly reuses) the replaced one.


