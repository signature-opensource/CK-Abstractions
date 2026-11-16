namespace CK.Core;

/// <summary>
/// The activation of a configurable type (see <see cref="IConfigurableTypeAttribute"/>). It answers two questions with
/// one declaration:
/// <list type="bullet">
///     <item>
///     The membership of a set: an <see cref="Optional"/> type is a member only when it is rooted (declared by the set
///     or required by a member); the other ones are members by default. This optionality lets an assembly offer more
///     than what a party eventually uses while keeping the unused code out.
///     </item>
///     <item>
///     The activation of a member at run time, by its key in the configuration: <c>"X": {}</c> configures it (and turns
///     it on), <c>"X": true</c> turns it on, <c>"X": false</c> turns it off.
///     </item>
/// </list>
/// </summary>
public enum TypeActivation
{
    /// <summary>
    /// Not declared: an invalid value (a configurable type declares its activation explicitly).
    /// </summary>
    None = 0,

    /// <summary>
    /// A member of a set only when rooted, off unless the configuration says <c>"X": {}</c> or <c>"X": true</c>
    /// (<c>"X": false</c> is redundant: a warning).
    /// </summary>
    Optional = 1,

    /// <summary>
    /// A member of a set by default, on unless the configuration says <c>"X": false</c>
    /// (<c>"X": true</c> is redundant: a warning).
    /// </summary>
    ByDefault = 2,

    /// <summary>
    /// A member of a set by default, always on: <c>"X": false</c> is an error
    /// (<c>"X": true</c> is redundant: a warning).
    /// </summary>
    Required = 3
}
