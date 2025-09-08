using System;

namespace CK.Core;

/// <summary>
/// Decorates the <see cref="IPoco"/> interface to specify that it is a secondary interface of a
/// IPoco family defined by a <see cref="PrimaryPocoAttribute"/>.
/// </summary>
[AttributeUsage( AttributeTargets.Interface, AllowMultiple = false, Inherited = false )]
public sealed class SecondaryPocoAttribute : Attribute
{
}
