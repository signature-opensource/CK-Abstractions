using System;

namespace CK.Core;

/// <summary>
/// Abstract base class for Engine attributes.
/// <para>
/// This attribute and <see cref="EngineAttribute{T}"/> bind the user code to the Engine side through
/// a simple assembly qualified name that identifies the actual attribute implementation.
/// The user code side attribute can declare any properties that configures the behavior of its Engine peer.
/// </para>
/// <para>
/// A <see cref="EngineAttribute"/> can be "augmented" by any number of child <see cref="EngineAttribute{T}"/>.
/// Usually, a type or a member is decorated by only one root EngineAttribute but nothing prevents
/// more than one root EngineAttribute to decorate the same type or member if the <see cref="AttributeUsageAttribute"/>
/// specifies a true <see cref="AttributeUsageAttribute.AllowMultiple"/>: it is up to the Engine
/// side to define the behavior.
/// </para>
/// <para>
/// When specializing, the <see cref="AttributeUsageAttribute"/> should be specified with <c>Inherited = false</c>
/// (the attribute inheritance is not used by the engine). <see cref="AttributeUsageAttribute.AllowMultiple"/>
/// should be false but may be true: it's up to the associated implementation to handle the subtleties of
/// multiplicity.
/// </para>
/// </summary>
public abstract class EngineAttribute : Attribute, IEngineAttribute
{
    /// <summary>
    /// Defines the default type targets.
    /// </summary>
    public const AttributeTargets DefaultTypeTargets = AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Enum;

    /// <summary>
    /// Defines the default member targets.
    /// </summary>
    public const AttributeTargets DefaultMemberTargets = AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Constructor | AttributeTargets.Method;

    /// <summary>
    /// Initializes a new <see cref="EngineAttribute"/> that delegates its behaviors to
    /// an Engine implementation.
    /// </summary>
    /// <param name="actualAttributeTypeAssemblyQualifiedName">
    /// Assembly Qualified Name of the associated engine implementation that must be a specialized
    /// CK.Engine.TypeCollector.EngineAttributeImpl class (in CK.Engine.TypeCollector assembly).
    /// <para>
    /// Example: "CK.TypeScript.Engine.TypeScriptPackageImpl, CK.TypeScript.Engine".
    /// </para>
    /// </param>
    protected EngineAttribute( string actualAttributeTypeAssemblyQualifiedName )
    {
        ActualAttributeTypeAssemblyQualifiedName = actualAttributeTypeAssemblyQualifiedName;
    }

    /// <inheritdoc />
    public string ActualAttributeTypeAssemblyQualifiedName { get; }

    /// <inheritdoc />
    public Type? ParentEngineAttributeType { get; private protected init; }

    void IEngineAttribute.LocalImplementationOnly() {}
}
