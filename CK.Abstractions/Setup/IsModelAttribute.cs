using System;

namespace CK.Setup;

/// <summary>
/// Marks an assembly as being a Model. The types it contains must be processed by CKomposable.
/// <para>
/// This attribute doesn't need to be defined if the assembly eventually depends on an assembly that is
/// marked with <see cref="IsModelDefinerAttribute"/> because being a Model is transitive: assemblies
/// that evenually depends on a Model are also Models.
/// </para>
/// </summary>
[AttributeUsage( AttributeTargets.Assembly, AllowMultiple = false )]
public sealed class IsModelAttribute : Attribute
{
}
