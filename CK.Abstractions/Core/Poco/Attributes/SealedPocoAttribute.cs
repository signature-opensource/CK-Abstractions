using System;

namespace CK.Core;

/// <summary>
/// Decorates a <see cref="IPoco"/> interface that is a IPoco family (like a <see cref="PrimaryPocoAttribute"/>
/// but  cannot have any <see cref="SecondaryPocoAttribute"/>. It is not extensible.
/// </summary>
/// <remarks>
/// As of September 2025, this is not yet supported.
/// </remarks>
[AttributeUsage( AttributeTargets.Interface, AllowMultiple = false, Inherited = false )]
public sealed class SealedPocoAttribute : Attribute
{
}
