namespace CK.Core;

/// <summary>
/// Abstract base class for child engine attributes. An existing <see cref="IEngineAttribute"/> of a type
/// assignable to <typeparamref name="T"/> is required and must appear before this one.
/// <para>
/// When specializing, the <see cref="AttributeUsageAttribute"/> should be specified with <c>Inherited = false</c>
/// (the attribute inheritance is not used by the engine) and <see cref="AttributeUsageAttribute.AllowMultiple"/>
/// should usually be true as such "attribute extensions" can exist for the same <typeparamref name="T"/> or
/// apply to mutiple <typeparamref name="T"/>:: it's up to the associated implementation to handle the subtleties
/// of multiplicity. 
/// </para>
/// </summary>
/// <typeparam name="T">The <see cref="IEngineAttribute"/> that this attribute extends.</typeparam>
public abstract class EngineAttribute<T> : EngineAttribute, IEngineAttribute<T> where T : IEngineAttribute
{
    /// <summary>
    /// Initializes a new <see cref="EngineAttribute{T}"/> that delegates its behaviors to
    /// an Engine implementation.
    /// </summary>
    /// <param name="actualAttributeTypeAssemblyQualifiedName">
    /// Assembly Qualified Name of the associated engine implementation that must be a specialized
    /// CK.Engine.TypeCollector.EngineAttributeImpl class.
    /// <para>
    /// Example: "Namespace.TypeNameAttributeImpl, SomeEngineAssembly".
    /// </para>
    /// </param>
    protected EngineAttribute( string actualAttributeTypeAssemblyQualifiedName )
        : base( actualAttributeTypeAssemblyQualifiedName )
    {
        ParentEngineAttributeType = typeof( T );
    }

}

