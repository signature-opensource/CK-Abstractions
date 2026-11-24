using System;

namespace CK.Core;

/// <summary>
/// Declares the configuration model of a configurable type (a Builder or App aspect, a party feature, a client feature,
/// an option: see <see cref="IConfigurableTypeAttribute"/>): a class whose properties are the keys of the section of the
/// type. The owner receives its model, read and validated, instead of reading its configuration: the model's
/// description is what the tools (the App builder, the configurator) know of the section.
/// <para>
/// A model is a <c>partial</c> class: its description is generated at compile time (CK.ConfigurationModel). A property is
/// one of three kinds: a nested model (or a list, or a dictionary of named models), a foreign options type whose
/// properties are its shape, or an opaque <c>IConfiguration</c> described by its <see cref="ConfigurationSchemaAttribute"/>.
/// </para>
/// </summary>
/// <typeparam name="TModel">The configuration model.</typeparam>
[AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = false )]
public sealed class ConfigurationAttribute<TModel> : Attribute where TModel : class
{
}
