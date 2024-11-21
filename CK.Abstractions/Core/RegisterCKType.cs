using System;

namespace CK.Core;

/// <summary>
/// Declares one or more types that will necessarily be registered if the decorated type is registered.
/// These registrations are intrinsic, non revocable.
/// <para>
/// The registered types must not be decorated with <see cref="ExcludeCKTypeAttribute"/>. 
/// </para>
/// <para>
/// Note that this is not the same attribute as the <see cref="CK.Setup.RegisterCKTypeAttribute"/> that applies
/// to assemblies.
/// </para>
/// </summary>
[AttributeUsage( AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false )]
public sealed class RegisterCKTypeAttribute : Attribute
{
    /// <summary>
    /// Initializes a new <see cref="RegisterCKTypeAttribute"/>.
    /// </summary>
    /// <param name="type">The first type to register.</param>
    /// <param name="otherTypes">Other types to register.</param>
    public RegisterCKTypeAttribute( Type type, params Type[] otherTypes )
    {
    }
}
