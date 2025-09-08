using System;

namespace CK.Core;

/// <summary>
/// Decorates the <see cref="IPoco"/> interface that defines a family.
/// </summary>
[AttributeUsage( AttributeTargets.Interface, AllowMultiple = false, Inherited = false )]
public sealed class PrimaryPocoAttribute : Attribute
{
}
