using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;

namespace CK.Poco.Immutable.Generator.Tests;

public static class GeneratorTestHelper
{
    static readonly MetadataReference[] _baseReferences;

    static GeneratorTestHelper()
    {
        var assemblies = new[]
        {
            typeof( object ).Assembly,                                    // mscorlib / System.Runtime
            typeof( CK.Core.IPoco ).Assembly,                             // CK.Abstractions
        };
        var refs = new List<MetadataReference>();
        foreach( var asm in assemblies )
        {
            refs.Add( MetadataReference.CreateFromFile( asm.Location ) );
        }
        // Add System.Runtime for netcoreapp
        var runtimeDir = System.IO.Path.GetDirectoryName( typeof( object ).Assembly.Location )!;
        refs.Add( MetadataReference.CreateFromFile( System.IO.Path.Combine( runtimeDir, "System.Runtime.dll" ) ) );
        refs.Add( MetadataReference.CreateFromFile( System.IO.Path.Combine( runtimeDir, "System.Collections.dll" ) ) );
        _baseReferences = refs.ToArray();
    }

    public static (ImmutableArray<Diagnostic> Diagnostics, string[] GeneratedSources) RunGenerator( string source )
    {
        var syntaxTree = CSharpSyntaxTree.ParseText( source );
        var compilation = CSharpCompilation.Create(
            "TestAssembly",
            new[] { syntaxTree },
            _baseReferences,
            new CSharpCompilationOptions( OutputKind.DynamicallyLinkedLibrary ) );

        var generator = new ImmutablePocoGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create( generator );
        driver = driver.RunGeneratorsAndUpdateCompilation( compilation, out var outputCompilation, out var diagnostics );

        var runResult = driver.GetRunResult();
        var generatedSources = runResult.GeneratedTrees
            .Select( t => t.GetText().ToString() )
            .ToArray();

        return (diagnostics, generatedSources);
    }
}
