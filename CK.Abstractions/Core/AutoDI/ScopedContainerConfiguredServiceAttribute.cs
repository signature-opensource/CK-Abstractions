using System;

namespace CK.Core;

/// <summary>
/// States that the decorated class or interface is a scoped service that is registered specifically
/// by Dependency Injection container.
/// <para>
/// This attribute cannot declare an ambient service: use the <see cref="IAmbientAutoService"/> interface marker
/// (or <see cref="ConfigurableAutoServiceKind.IsAmbientService"/> for an external type that cannot be decorated).
/// </para>
/// </summary>
[AttributeUsage( AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false )]
public sealed class ScopedContainerConfiguredServiceAttribute : Attribute
{
    /// <summary>
    /// Initializes a new <see cref="ScopedContainerConfiguredServiceAttribute"/>.
    /// </summary>
    public ScopedContainerConfiguredServiceAttribute()
    {
    }

    /// <summary>
    /// Obsolete and unsupported: this attribute cannot declare an ambient service.
    /// </summary>
    /// <param name="isAmbientService">Unused.</param>
    [Obsolete( "Use ExternalTypes registration instead.", error: true )]
    public ScopedContainerConfiguredServiceAttribute( bool isAmbientService = false )
    {
    }

}
