namespace CK.Core;

/// <summary>
/// This interface must be implemented by attributes that make a package (a concrete <see cref="IResourceGroup"/>) a
/// configurable type: the configuration names it (its <see cref="Name"/> is the key of its section), configures it and
/// switches it (its <see cref="Activation"/>).
/// <para>
/// The <see cref="Name"/> is an alias of the type <b>in the configuration only</b>: it is never a reference to the type
/// in a relation (Requires, RequiredBy, etc.), nor a type name in an export or an import. The types drive: a name is
/// valid only when its type is a member of the configured set, the name never brings its type in.
/// </para>
/// <para>
/// Rules that the engine checks:
/// <list type="bullet">
///     <item>A decorated type must be a package: not an abstraction and an <see cref="IResourceGroup"/>.</item>
///     <item>
///     The attribute is read on the decorated type only (not on its base classes): each configurable type declares
///     its name explicitly, the name is a contract of the configuration that must not depend on a type name.
///     A type has at most one such attribute.
///     </item>
///     <item>
///     A name is an identifier (letters, digits and dots, starting with a letter), compared case insensitively.
///     It is unique in its scope: the scopes (a party, a client, the section of an owner, etc.) are defined and
///     checked by the engine of the model that defines the configurable types.
///     </item>
///     <item>The <see cref="Activation"/> is explicit: <see cref="TypeActivation.None"/> is an error.</item>
///     <item>
///     The optionality of a configurable type is its <see cref="Activation"/>: this attribute is an
///     <see cref="IOptionalResourceGroupAttribute"/> that is optional when the activation is <see cref="TypeActivation.Optional"/>
///     (an implementation must not override <see cref="IOptionalResourceGroupAttribute.IsOptional"/>).
///     </item>
/// </list>
/// </para>
/// </summary>
public interface IConfigurableTypeAttribute : IOptionalResourceGroupAttribute
{
    /// <summary>
    /// Gets the configuration name of the decorated type: the key of its section in its scope.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the activation of the decorated type: whether it is a member of a set and active by default, and whether
    /// the configuration can switch it off.
    /// </summary>
    TypeActivation Activation { get; }

    /// <summary>
    /// Gets whether the <see cref="Activation"/> is <see cref="TypeActivation.Optional"/>: the optionality of a
    /// configurable type is its activation.
    /// </summary>
    bool IOptionalResourceGroupAttribute.IsOptional => Activation == TypeActivation.Optional;
}
