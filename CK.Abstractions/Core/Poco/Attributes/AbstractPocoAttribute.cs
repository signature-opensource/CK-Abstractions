using System;

namespace CK.Core;

/// <summary>
/// Decorates a <see cref="IPoco"/> to specify that it is an absract interface that will appear in
/// any number of IPoco family.
/// </summary>
[AttributeUsage( AttributeTargets.Interface, AllowMultiple = false, Inherited = false )]
public sealed class AbstractPocoAttribute : Attribute
{
}
