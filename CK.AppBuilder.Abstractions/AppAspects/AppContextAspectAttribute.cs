using CK.Core;
using System;

namespace CK.Setup;

/// <summary>
/// Declares an AppContext aspect: an <see cref="IAppContextAspect"/> package that the <c>"Aspects"</c> of an AppContext
/// (the Apps and the aspects that one engine run builds, managed by its Builder) names (<see cref="Name"/>), configures
/// (its section) and switches (<see cref="Activation"/>). An AppContext aspect is engine wide (one per engine run): it is
/// present when it is a member of the type set of one of the Apps of the AppContext (<see cref="TypeActivation.ByDefault"/> and <see cref="TypeActivation.Required"/>) or
/// when its key is declared (<see cref="TypeActivation.Optional"/>), unless its key is <c>false</c>.
/// </summary>
[AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = false )]
public sealed class AppContextAspectAttribute : Attribute, IConfigurableTypeAttribute
{
    /// <summary>
    /// Initializes a new <see cref="AppContextAspectAttribute"/>.
    /// </summary>
    /// <param name="name">The configuration name: the key of the aspect in the root <c>"Aspects"</c>.</param>
    /// <param name="activation">The activation of the aspect.</param>
    public AppContextAspectAttribute( string name, TypeActivation activation )
    {
        Name = name;
        Activation = activation;
    }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public TypeActivation Activation { get; }
}
