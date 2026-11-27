using System;

namespace CK.Core;

/// <summary>
/// Declares that a property of a configuration model (see <see cref="ConfigurationAttribute{TModel}"/>) is read on the
/// owner's own section only, never from the levels above it: a party's setting that must be configured on each party
/// (a security setting: its trust anchors, its keys...). A value in a <c>"Defaults"</c> above the party is an error, not
/// silently ignored.
/// </summary>
[AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = false )]
public sealed class ConfigurationNoDefaultsAttribute : Attribute
{
}
