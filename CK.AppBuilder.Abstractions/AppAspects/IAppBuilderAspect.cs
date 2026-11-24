using CK.Core;

namespace CK.Setup;

/// <summary>
/// Marks a Builder aspect (see <see cref="AppBuilderAspectAttribute"/>): a package of a model that the App builder finds
/// in the type sets of its Apps, creates from its configuration and applies to the engine configuration, before the App
/// builder contributors run. It is the owner kind of the <see cref="AppBuilderAspectOptionAttribute"/> options.
/// <para>
/// A concrete aspect has one public constructor whose optional parameters are its section (a plain
/// <c>Microsoft.Extensions.Configuration.IConfiguration</c>, null when the aspect is present without section: its
/// activation, or <c>true</c>) and its options (an <c>IReadOnlyList&lt;IAppBuilderAspectOption&lt;TAspect&gt;&gt;</c>, see
/// <see cref="IAppBuilderAspectOption{TAspect}"/>: the enabled options in their order). The constructor reads its data
/// and throws on an invalid value.
/// </para>
/// <para>
/// What an aspect does is engine side: it also implements <c>IAppBuilderAspectApply</c> (CK.Engine.Configuration),
/// whose <c>Apply</c> receives the App builder context and the Apps whose type set contains the aspect.
/// </para>
/// </summary>
public interface IAppBuilderAspect : IResourceGroup
{
}
