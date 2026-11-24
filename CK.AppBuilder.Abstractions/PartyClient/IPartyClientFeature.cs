namespace CK.AppIdentity;

/// <summary>
/// Marks a feature of a client of a party (see <see cref="PartyClientFeatureAttribute"/>): the <c>ClientFeature</c> base
/// class of CK.AppIdentity.PartyClient implements it. It is the owner kind of the
/// <see cref="PartyClientFeatureOptionAttribute"/> options.
/// </summary>
public interface IPartyClientFeature : IPartyComponent
{
}
