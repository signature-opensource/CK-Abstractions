namespace CK.Core;

/// <summary>
/// Pseudo-marker interface for immutable Poco.
/// </summary>
public interface IImmutablePoco
{
    IPoco ToMutable();
}
