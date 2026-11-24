namespace CK.Core;

/// <summary>
/// Optional interface of a configuration model (see <see cref="ConfigurationModelAttribute"/>) that validates what its
/// types can't say (a range, an absolute URL, a rule between keys). It has no service: it runs wherever the model is read,
/// at run time, at build time and in the tools. The reader of the models (CK.ConfigurationModel) calls it once the keys are
/// read without error; an error collected here fails the reading.
/// </summary>
public interface IConfigurationModel
{
    /// <summary>
    /// Validates this model. The errors (and the warnings) are collected, never thrown.
    /// </summary>
    /// <param name="context">The path of the model's section and the collectors of the messages.</param>
    void Validate( IConfigurationValidationContext context );
}
