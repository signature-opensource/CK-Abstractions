using System;

namespace CK.Core;

/// <summary>
/// Declares the section of the configuration of an App singleton (a root key of the App in "ckonf.json", of its host's
/// configuration): its properties decorated with <see cref="ConfigurationLevelAttribute"/> are configuration values of
/// this section (<c>"CrisExecutionHost": { "ParallelRunnerCount": 4 }</c>).
/// </summary>
[AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = false )]
public sealed class ConfigurationSectionAttribute : Attribute
{
    /// <summary>
    /// Initializes a new section declaration.
    /// </summary>
    /// <param name="name">The name of the section (a root key of the App).</param>
    public ConfigurationSectionAttribute( string name )
    {
        Name = name;
    }

    /// <summary>
    /// Gets the name of the section.
    /// </summary>
    public string Name { get; }
}
