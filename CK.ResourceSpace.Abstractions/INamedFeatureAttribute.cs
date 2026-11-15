namespace CK.Core;

/// <summary>
/// This interface must be implemented by attributes that give a feature name to a package (a concrete
/// <see cref="IResourceGroup"/>): the name that activates or deactivates it in a type set (the roots and the
/// exclusions of the set, the "AllowFeatures" and "DisallowFeatures" of a party).
/// <para>
/// This name is <b>not</b> an alias of the type: it is never a reference to it (in a relation or anywhere else). It is
/// only resolved by the engine while it computes a type set.
/// </para>
/// <para>
/// Rules that the engine checks:
/// <list type="bullet">
///     <item>A decorated type must be a package: not an abstraction and an <see cref="IResourceGroup"/>.</item>
///     <item>A package has at most one feature name: the closest one in its base classes (an abstract class can name its specializations).</item>
///     <item>
///     Exactly one of <see cref="Name"/> and <see cref="TypeNameSuffix"/> is not null, and a type name must end with
///     the <see cref="TypeNameSuffix"/> (that is not the whole type name).
///     </item>
///     <item>
///     A name is an identifier (letters, digits and dots, starting with a letter), compared case insensitively.
///     </item>
///     <item>
///     A name is borne by a single package unless all the packages that bear it are <see cref="IsMultiple"/>: the name
///     is then a set of 1 to n packages that must agree on their optionality (see <see cref="IOptionalResourceGroupAttribute"/>).
///     </item>
/// </list>
/// </para>
/// </summary>
public interface INamedFeatureAttribute
{
    /// <summary>
    /// Gets the explicit feature name. When null, <see cref="TypeNameSuffix"/> is not null.
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// Gets the suffix to remove from the decorated type name to obtain the feature name ("FeatureDriver" for instance:
    /// the "HttpEndpointFeatureDriver" is the "HttpEndpoint" feature). This enables an attribute that is inherited from
    /// a base class to name its specializations. When null, <see cref="Name"/> is not null.
    /// </summary>
    string? TypeNameSuffix => null;

    /// <summary>
    /// Gets whether other packages can bear the same feature name (they must also be multiple).
    /// </summary>
    bool IsMultiple => false;
}
