using System;

namespace CK.Core;

/// <summary>
/// This interface can be implemented by attributes that decorate a <see cref="IResourceGroup"/> (or <see cref="IResourcePackage"/>)
/// to declare topology relations of the decorated type. These relations are handled exactly like the
/// <see cref="RequiresAttribute{T}"/>, <see cref="RequiredByAttribute{T}"/>, <see cref="ChildrenAttribute{T}"/>,
/// <see cref="GroupsAttribute{T}"/>, <see cref="PackageAttribute{T}"/> attributes and their [Optional...] variants.
/// <para>
/// This enables an attribute that carries a type (like a component that must appear under a target component) to
/// state the relation once: it is known from the type's attributes, before any package is created.
/// </para>
/// <para>
/// All members have a default (empty) implementation: an attribute only declares the relations it carries.
/// Every type must be a <see cref="IResourceGroup"/>.
/// </para>
/// </summary>
public interface IResourceGroupTopologyAttribute
{
    /// <summary>
    /// Gets the types required by the decorated type.
    /// See <see cref="RequiresAttribute{T}"/>.
    /// </summary>
    ReadOnlySpan<Type> Requires => [];

    /// <summary>
    /// Gets the types required by the decorated type IF they belong to the package selection.
    /// See <see cref="OptionalRequiresAttribute{T}"/>.
    /// </summary>
    ReadOnlySpan<Type> OptionalRequires => [];

    /// <summary>
    /// Gets the types that require the decorated type.
    /// See <see cref="RequiredByAttribute{T}"/>.
    /// </summary>
    ReadOnlySpan<Type> RequiredBy => [];

    /// <summary>
    /// Gets the types that require the decorated type IF they belong to the package selection.
    /// See <see cref="OptionalRequiredByAttribute{T}"/>.
    /// </summary>
    ReadOnlySpan<Type> OptionalRequiredBy => [];

    /// <summary>
    /// Gets the children of the decorated type.
    /// See <see cref="ChildrenAttribute{T}"/>.
    /// </summary>
    ReadOnlySpan<Type> Children => [];

    /// <summary>
    /// Gets the children of the decorated type IF they belong to the package selection.
    /// See <see cref="OptionalChildrenAttribute{T}"/>.
    /// </summary>
    ReadOnlySpan<Type> OptionalChildren => [];

    /// <summary>
    /// Gets the groups to which the decorated type belongs.
    /// See <see cref="GroupsAttribute{T}"/>.
    /// </summary>
    ReadOnlySpan<Type> Groups => [];

    /// <summary>
    /// Gets the groups to which the decorated type belongs IF they belong to the package selection.
    /// See <see cref="OptionalGroupsAttribute{T}"/>.
    /// </summary>
    ReadOnlySpan<Type> OptionalGroups => [];

    /// <summary>
    /// Gets the package of the decorated type. At most one package can be declared for a type,
    /// by a <see cref="PackageAttribute"/>, a <see cref="PackageAttribute{T}"/> or any of these attributes.
    /// See <see cref="PackageAttribute{T}"/>.
    /// </summary>
    Type? Package => null;
}
