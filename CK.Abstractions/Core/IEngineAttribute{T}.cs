namespace CK.Core;

/// <summary>
/// Abstraction for <see cref="EngineAttribute{T}"/>.
/// </summary>
/// <typeparam name="T">The <see cref="IEngineAttribute"/> that this attribute extends.</typeparam>
interface IEngineAttribute<T> : IEngineAttribute where T : IEngineAttribute
{
}
