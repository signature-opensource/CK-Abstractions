using System;

namespace CK.Core;

/// <summary>
/// Declares the capability of a property of a configuration section (see <see cref="ConfigurationSectionAttribute"/>):
/// <see cref="ConfigurationLevel.Run"/> means that its code supports its change while the application runs. It is a
/// contract: the setter is thread safe and validates (it throws on an invalid value). The value is a basic one: a number,
/// a boolean, a string, an enum (by its name), a <see cref="TimeSpan"/> or a <see cref="Guid"/>.
/// <para>
/// A property whose type is a class can be decorated too: its own decorated properties are reached through it (its
/// getter must return a stable instance).
/// </para>
/// </summary>
[AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
public sealed class ConfigurationLevelAttribute : Attribute
{
    /// <summary>
    /// Initializes a new capability declaration.
    /// </summary>
    /// <param name="level">The capability of the property.</param>
    public ConfigurationLevelAttribute( ConfigurationLevel level )
    {
        Level = level;
    }

    /// <summary>
    /// Gets the capability of the property.
    /// </summary>
    public ConfigurationLevel Level { get; }
}
