namespace CK.Core;

/// <summary>
/// Abstraction for <see cref="ChildEngineAttribute{T}"/>.
/// </summary>
/// <typeparam name="T">The <see cref="IEngineAttribute"/> that this attribute extends.</typeparam>
interface IChildEngineAttribute<T> : IEngineAttribute where T : IEngineAttribute
{
}
