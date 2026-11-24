namespace CK.Core;

/// <summary>
/// What <see cref="IConfigurationModel.Validate(IConfigurationValidationContext)"/> receives: the path of the model's
/// section and the collectors of its messages. The messages are written in the invariant culture (a configuration error
/// is read by the developer or the operator, not by a user).
/// </summary>
public interface IConfigurationValidationContext
{
    /// <summary>
    /// Gets the configuration path of the model's section (for the messages: <c>$"'{context.Path}:RpId' must be..."</c>).
    /// </summary>
    string Path { get; }

    /// <summary>
    /// Collects an error: the reading of the model fails.
    /// </summary>
    /// <param name="message">The message.</param>
    void Error( string message );

    /// <summary>
    /// Collects a warning: the model is read.
    /// </summary>
    /// <param name="message">The message.</param>
    void Warn( string message );
}
