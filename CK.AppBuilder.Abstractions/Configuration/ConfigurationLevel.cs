namespace CK.Core;

/// <summary>
/// The level of a configuration value: when it can be set. A level is first a capability of the code that consumes the
/// value; the configuration can only restrict it (a <c>"$LOCK"</c> lowers it, never raises it).
/// <list type="bullet">
///     <item><see cref="Compile"/>: fixed by the build (an engine input, or a value burnt in the generated code).</item>
///     <item><see cref="Init"/>: read once, when the application starts. It can be restricted to <see cref="Compile"/>.</item>
///     <item><see cref="Run"/>: the code supports its change while it runs. It can be restricted to <see cref="Init"/> or
///     <see cref="Compile"/>.</item>
/// </list>
/// </summary>
public enum ConfigurationLevel
{
    /// <summary>
    /// Fixed by the build: nothing at run time can change it.
    /// </summary>
    Compile = 0,

    /// <summary>
    /// Read once, when the application starts.
    /// </summary>
    Init = 1,

    /// <summary>
    /// Can change while the application runs.
    /// </summary>
    Run = 2
}
