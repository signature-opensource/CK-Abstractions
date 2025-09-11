using System;

namespace CK.Core;

/// <summary>
/// Decorates a <see cref="IPoco"/> interface that is both abstract and concrete.
/// Such interface can appear in any number of IPoco family but can be instantiated on their own
/// (they cannot have any <see cref="SecondaryPocoAttribute"/>).
/// </summary>
/// <remarks>
/// As of September 2025, this is not yet supported.
/// </remarks>
[AttributeUsage( AttributeTargets.Interface, AllowMultiple = false, Inherited = false )]
public sealed class VirtualPocoAttribute : Attribute
{
}
