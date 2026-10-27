namespace CK.Core;

/// <summary>
/// Companion service that must be available for any <see cref="IAmbientAutoService"/>.
/// <para>
/// This is a singleton service because the default value of an ambient service must not
/// depend on the resolution context.
/// </para>
/// </summary>
/// <typeparam name="T">
/// The ambient service type. This is not constrained to <see cref="IAmbientAutoService"/>
/// because an external type (that cannot be decorated) can also be declared as an ambient service
/// with <see cref="ConfigurableAutoServiceKind.IsAmbientService"/>.
/// </typeparam>
public interface IAmbientServiceDefaultProvider<out T> : ISingletonAutoService where T : class
{
    /// <summary>
    /// Gets a default value for the ambient service <typeparamref name="T"/>.
    /// </summary>
    T Default { get; }
}
