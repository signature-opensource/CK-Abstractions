using CK.Core;

namespace CK.Setup;

/// <summary>
/// Marks an App aspect (see <see cref="AppAspectAttribute"/>): a package of a model that the App builder finds in the
/// type set of an App, creates from the App's configuration and applies to the App, before the App builder contributors
/// run. It is the owner kind of the <see cref="AppAspectOptionAttribute"/> options.
/// <para>
/// A concrete aspect has one public constructor whose optional parameters are its section (a plain
/// <c>Microsoft.Extensions.Configuration.IConfiguration</c>, null when the aspect is present without section: its
/// activation, or <c>true</c>) and its options (an <c>IReadOnlyList&lt;IAppAspectOption&lt;TAspect&gt;&gt;</c>, see
/// <see cref="IAppAspectOption{TAspect}"/>: the enabled options in their order). The constructor reads its data and throws
/// on an invalid value. One instance is created per App.
/// </para>
/// <para>
/// What an aspect does is engine side: it also implements <c>IAppAspectApply</c> (CK.Engine.Configuration), whose
/// <c>Apply</c> receives the App builder context and the App.
/// </para>
/// </summary>
public interface IAppAspect : IResourceGroup
{
}
