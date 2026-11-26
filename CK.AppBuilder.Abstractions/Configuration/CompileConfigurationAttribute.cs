using System;

namespace CK.Core;

/// <summary>
/// Declares the compile time configuration model of a component (a party feature, a client feature, the party client):
/// a class whose properties are the keys of the component's section that the build reads (the engine and its
/// contributors), next to its run time model (<see cref="ConfigurationAttribute{TModel}"/>).
/// <para>
/// The keys of a compile model are the compile keys of the section: the engine reads the model and gives it to its
/// contributors (they never read the section), the run time reserves its property names, and the tools describe it as
/// "build time" (a change requires a new build).
/// </para>
/// <para>
/// A compile model follows the rules of a configuration model (see <see cref="ConfigurationAttribute{TModel}"/>): a
/// <c>partial</c> class whose description is generated at compile time (CK.ConfigurationModel). A model may derive from
/// the compile model of an upstream component (the TypeScript client's base model for a TypeScript framework).
/// </para>
/// </summary>
/// <typeparam name="TModel">The compile time configuration model.</typeparam>
[AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = false )]
public sealed class CompileConfigurationAttribute<TModel> : Attribute where TModel : class
{
}
