using CK.Core;
using System;
using System.Runtime.CompilerServices;

namespace CK.Setup;

/// <summary>
/// Decorates an option of an AppContext aspect: an <see cref="IAppContextAspectOption{TAspect}"/>. Its name is its key in the
/// section of its aspect, in the <c>"Aspects"</c> of the AppContext.
/// </summary>
[AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = false )]
public sealed class AppContextAspectOptionAttribute : Attribute, IConfigurableOptionAttribute
{
    /// <summary>
    /// Initializes a new <see cref="AppContextAspectOptionAttribute"/>.
    /// </summary>
    /// <param name="name">The configuration name of the option: its key in the section of its aspect.</param>
    /// <param name="activation">The activation of the option.</param>
    /// <param name="callerFilePath">Automatically set by the compiler.</param>
    public AppContextAspectOptionAttribute( string name, TypeActivation activation, [CallerFilePath] string? callerFilePath = null )
    {
        Name = name;
        Activation = activation;
        CallerFilePath = callerFilePath;
    }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public TypeActivation Activation { get; }

    /// <inheritdoc />
    public string? CallerFilePath { get; }

    /// <summary>
    /// Gets <see cref="IAppContextAspect"/>.
    /// </summary>
    public Type OwnerKind => typeof( IAppContextAspect );
}
