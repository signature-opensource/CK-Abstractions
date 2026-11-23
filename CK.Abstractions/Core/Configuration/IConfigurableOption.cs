namespace CK.Core;

/// <summary>
/// An option of a configurable type (its owner): a package that is configured in the section of its owner, by its name
/// (declared by its <see cref="IConfigurableOptionAttribute"/>), and that applies to <typeparamref name="TOwner"/> and to
/// the owners derived from it.
/// <para>
/// The options exist at every level of the configuration: the Builder aspects, the App aspects, the party features and
/// the client features have options. Each level defines its option interface (a specialization of this one) and its
/// attribute (a specialization of <see cref="IConfigurableOptionAttribute"/>), and the engine of that level orders the
/// options of each owner (the topology attributes between options order them; a non optional relation also means
/// "only with it"), selects the enabled ones from the section of the owner and creates them.
/// </para>
/// </summary>
/// <typeparam name="TOwner">The owner type (or a base of it) to which this option applies.</typeparam>
public interface IConfigurableOption<TOwner> : IResourceGroup
{
}
