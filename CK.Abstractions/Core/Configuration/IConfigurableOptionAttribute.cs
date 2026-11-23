using System;

namespace CK.Core;

/// <summary>
/// The attribute of an <see cref="IConfigurableOption{TOwner}"/>. Each level of the configuration specializes it for its
/// kind of owner. It is also the embedded resource attribute that the ordering of the options requires.
/// <para>
/// An option is a configurable type: its <see cref="IConfigurableTypeAttribute.Name"/> is its key in the section of its
/// owner (unique among the options of an owner), its <see cref="IConfigurableTypeAttribute.Activation"/> says whether it
/// is enabled when the owner's configuration doesn't say (<see cref="TypeActivation.ByDefault"/> or
/// <see cref="TypeActivation.Required"/>: an opt-out option) or only when it says (<see cref="TypeActivation.Optional"/>:
/// an opt-in option).
/// </para>
/// </summary>
public interface IConfigurableOptionAttribute : IEmbeddedResourceTypeAttribute, IConfigurableTypeAttribute
{
    /// <summary>
    /// Gets the kind of owner of the options decorated by this attribute: the owner type of the option must be
    /// assignable to it.
    /// </summary>
    Type OwnerKind { get; }
}
