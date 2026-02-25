using NUnit.Framework;
using Shouldly;
using System.Linq;

namespace CK.Poco.Immutable.Generator.Tests;

public class NestedGenericTests
{
    [Test]
    public void Nested_generic_with_IPoco_maps_recursively()
    {
        var source = """
            using CK.Core;
            using System.Collections.Generic;

            namespace TestApp;

            public interface ITag : IPoco
            {
                string Value { get; set; }
            }

            public interface IComplex : IPoco
            {
                Dictionary<string, List<ITag>> TagGroups { get; set; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var complexSource = generated.FirstOrDefault( s => s.Contains( "interface IImmutableComplex" ) );
        complexSource.ShouldNotBeNull();
        complexSource.ShouldContain(
            "System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IReadOnlyList<TestApp.IImmutableTag>> TagGroups { get; }" );
    }

    [Test]
    public void Namespace_is_preserved_exactly()
    {
        var source = """
            using CK.Core;

            namespace My.Deep.Namespace;

            public interface IDeep : IPoco
            {
                int Value { get; set; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var deepSource = generated.FirstOrDefault( s => s.Contains( "IImmutableDeep" ) );
        deepSource.ShouldNotBeNull();
        deepSource.ShouldContain( "namespace My.Deep.Namespace" );
    }
}
