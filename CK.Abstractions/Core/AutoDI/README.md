# Automatic Dependency Injection contracts

This set of interfaces and attributes are markers (only `IAmbientServiceDefaultProvider<T>` declares a
member). These types are the heart of the Automatic DI that is handled by the CKomposable engine.

## IRealObject

This interface marker states that a class or an interface instance
is a gateway to an external resource or a foundation of the architecture.
Such foundations are necessarily unique instance in the "context" defined
by the map of Real Objects that the engine builds.

If the lifetimes of this [IRealObject](IRealObject.cs) and [ISingletonAutoService](ISingletonAutoService.cs)
instances are the same, their roles are different as well as the way they are handled.

Real objects must be used to model actual singletons "in real life", typically external resources
with which the system interacts such as a data repository (a database is a real object as well as the tables
it contains) or an external service.

Singleton services are more classical services that happens to be able to be shared by different
activities because of their thread safety and the fact that they depend only on other singleton
services or real objects.

"Normal" singleton services rely on "normal" constructor injection but real objects cannot have constructors:
it is replaced by the optional `StObjConstruct` method that isolates dependencies between a base class and
its specializations.

### Optional methods

`StObjConstruct` is not the only method the engine looks for on a Real Object. These methods cannot be
formalized as interface members as most of them can have arbitrary parameters: they are discovered by
name, on each class of the hierarchy that declares them. They are optional **instance** methods that must
**not be virtual** and are typically private:

- `void StObjConstruct( ... )`
is the method that defines the dependencies: its parameters are other IRealObjects on which this object depends,
the parameters drive the topological sort and there is no real point to put code in it, except that
references to dependencies may be captured here if needed. A `StObjConstruct` without parameters is ignored.

- `void StObjInitialize( IActivityMonitor, IStObjObjectMap )`
is called at runtime once all the real objects have been instantiated and are available.

- `void RegisterStartupServices( IActivityMonitor, SimpleServiceContainer )`
is called at runtime once all the real objects have been initialized and can register any services in the
[SimpleServiceContainer](https://github.com/signature-opensource/CK-Core/blob/stable/CK.Core/ServiceContainer/SimpleServiceContainer.cs)
that may be used by the following `ConfigureServices` methods.

- `void ConfigureServices( StObjContextRoot.ServiceRegister, ... )`
enables real objects to configure the DI (registering new services, configuring things, etc.). The first
parameter is required (it can also be an `in` parameter); the other ones are resolved from the startup
services previously registered: a parameter with a default value is optional, any other one must be available.

- `void OnHostStart/Stop( ... )` or `Task/ValueTask OnHostStart/StopAsync( ... )`
enable real objects to act like hosted services: `OnHostStart[Async]` and `OnHostStop[Async]` are called
by an automatically generated Microsoft.Extensions.Hosting.IHostedService on startup (resp. on host stop).
These ones **must** be private instance methods.

  The parameters can be any scope or singleton service that may be available from the global DI container:
  a temporary scope is created to call all the Start (resp. Stop) methods and these methods' execution
  share this temporary scope.

  Defining these methods doesn't require any direct dependency but they require the
  [Microsoft.Extensions.Hosting.Abstractions](https://www.nuget.org/packages/Microsoft.Extensions.Hosting.Abstractions/)
  package to be available in the final application (otherwise a compilation error of the generated code will be raised).

`IActivityMonitor` comes from CK.ActivityMonitor, `SimpleServiceContainer` from CK.Core, `IStObjObjectMap` and
`StObjContextRoot.ServiceRegister` from the engine's CK.StObj.Model package: a Real Object that defines one of
these methods depends on the package of the types it uses.

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

## IAmbientAutoService : IScopedAutoService

[IAmbientAutoService](IAmbientAutoService.cs) marks an ambient service: a scoped service that is available
in all Dependency Injection containers and whose values can be injected in new "Unit of Work" (scoped
service provider). It is itself a `[CKTypeDefiner]`.

Very few services are (or can be) ambient services:
- It must carry information of general interest.
- It should be immutable (at least thread safe).
- It must not depend on any other service.
- A `IAmbientServiceDefaultProvider<T>` must exist for it.

Examples of such ambient services are concepts like the authentication information, the tenant or the current culture.

## IAmbientServiceDefaultProvider&lt;T&gt; : ISingletonAutoService

[IAmbientServiceDefaultProvider&lt;T&gt;](IAmbientServiceDefaultProvider%7BT%7D.cs) is the companion service
that must be available for any ambient service. Its single member, `T Default { get; }`, provides the
default value of the ambient service `T`. It is a singleton because the default value of an ambient
service must not depend on the resolution context.

## ScopedServiceAttribute & SingletonServiceAttribute

[[ScopedService](ScopedServiceAttribute.cs)] and [[SingletonService](SingletonServiceAttribute.cs)] mark
a class or an interface with a scoped (resp. singleton) lifetime without handling automatic DI for it:
the service must be manually registered in the global DI container. Unlike the "container configured"
attributes below, the service is automatically registered in the other DI containers.

## ScopedContainerConfiguredServiceAttribute & SingletonContainerConfiguredServiceAttribute

Marks a class or an interface to be configured by the DI container: it is not necessarily
available from all DI containers that may exist in an application. Note that defining a service
as being "container configured" requires to declare its lifetime (hence the 2 attributes).

When a `IAutoService` is also a container configured service, it is available in the global DI container
but not in other containers: it is up to the other containers to explicitly support the service by
registering the abstractions and/or implementations in its own service collection.

## ConfigurableAutoServiceKind

[ConfigurableAutoServiceKind](ConfigurableAutoServiceKind.cs) is the `[Flags]` subset of the Automatic DI
categorization that can be applied to external classes or interfaces (types that cannot be decorated),
thanks to the [`[AddType( Type, ConfigurableAutoServiceKind )]`](../../Setup/README.md#removepfeature-removetype-and-addtype)
assembly attribute. It applies only to Automatic Services: Real Objects and Poco can only be defined by code.

| Flag | Meaning |
|------|---------|
| `None` | No specific information is known about the type. |
| `IsScoped` | Each Unit of Work is provided a unique instance. Rejects `IsSingleton`. |
| `IsSingleton` | All Unit of Works (including concurrent ones) use the exact same instance. Rejects `IsScoped`. |
| `IsContainerConfiguredService` | Available in some DI containers but not necessarily in all of them. |
| `IsAmbientService` | A scoped service available in all contexts that automatically flows from endpoints to the background context. Implies `IsScoped` and `IsContainerConfiguredService`. |
| `IsMultipleService` | Multiple registration (see `[IsMultiple]` below). Applies only to interfaces. |

## IsMultipleAttribute
Marks an interface so that all its mappings to concrete classes must be automatically
registered and made available as a `IEnumerable<T>`, regardless of any existing registrations.
The engine honors it only on interfaces.

A "Multiple" interface is not compatible with IRealObject but can support any other auto service
marker like IScopedAutoService.

This attribute cancels the implicit unicity of the mapping but doesn't impact the lifetime:
the `IEnumerable<T>` will be a singleton if all the implementations are singletons otherwise
it will be a scoped service.

> IPoco interfaces cannot use the `[IsMultiple]` attribute, instead `[CKTypeDefiner]` must be used.

## ReplaceAutoServiceAttribute

Optional attribute for `IAutoService` **implementation** that
declares that this implementation replaces another one (the replaced implementation is
given to the constructor, either as a type or as its assembly qualified name).

Note that this attribute is useless if this implementation specializes the replaced service since
discovering the most precise implementation is one of the key goal of Auto services handling.

It is also useless if the replaced service is used by this implementation: as long as a parameter with the
same type appears in its constructor, this service "covers" (and possibly reuses) the replaced one.

