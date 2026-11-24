using System;

namespace CK.Core;

/// <summary>
/// Names the JSON Schema that describes an opaque property of a configuration model (see
/// <see cref="ConfigurationAttribute{TModel}"/>): a property typed <c>IConfiguration</c> whose shape is not the properties
/// of a type, read by its consumer (Kestrel's section, for instance:
/// <c>[ConfigurationSchema( "https://json.schemastore.org/appsettings.json#/definitions/kestrel" )]</c>). The description
/// of the model refers to it: the tools use it to complete and check the section, the run time doesn't.
/// </summary>
[AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
public sealed class ConfigurationSchemaAttribute : Attribute
{
    /// <summary>
    /// Initializes a new schema reference.
    /// </summary>
    /// <param name="schemaUri">The URI of the JSON Schema (with its fragment when it is a part of a document).</param>
    public ConfigurationSchemaAttribute( string schemaUri )
    {
        SchemaUri = schemaUri;
    }

    /// <summary>
    /// Gets the URI of the JSON Schema.
    /// </summary>
    public string SchemaUri { get; }
}
