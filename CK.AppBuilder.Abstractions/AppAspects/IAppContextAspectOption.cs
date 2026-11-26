using CK.Core;

namespace CK.Setup;

/// <summary>
/// An option of an AppContext aspect (see <see cref="IAppContextAspect"/>): a package configured in the section of its
/// aspect, by its name (declared by its <see cref="AppContextAspectOptionAttribute"/>), that applies to
/// <typeparamref name="TAspect"/> and to the aspects derived from it.
/// <para>
/// The rules are the ones of the App aspect options (see <see cref="IAppAspectOption{TAspect}"/>): the constructor of
/// the aspect receives its options in an <c>IReadOnlyList&lt;IAppContextAspectOption&lt;TAspect&gt;&gt;</c> parameter.
/// </para>
/// </summary>
/// <typeparam name="TAspect">The aspect type (or a base of it) to which this option applies.</typeparam>
public interface IAppContextAspectOption<in TAspect> : IConfigurableOption<TAspect> where TAspect : IAppContextAspect
{
}
