using System;

namespace CK.Core;

/// <summary>
/// Abstraction of <see cref="EngineAttribute"/>.
/// </summary>
public interface IEngineAttribute
{
    /// <summary>
    /// Gets the Assembly Qualified Name of the object that will replace this attribute during setup.
    /// </summary>
    string ActualAttributeTypeAssemblyQualifiedName { get; }

    /// <summary>
    /// Gets the parent <see cref="IEngineAttribute"/> type if this attribute is a child <see cref="IEngineAttribute{T}"/>.
    /// </summary>
    Type? ParentEngineAttributeType { get; }

    internal void LocalImplementationOnly();
}
