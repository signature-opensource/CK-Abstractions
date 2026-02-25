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

            public partial interface ITag : IPoco
            {
                string Value { get; set; }
            }

            public partial interface IComplex : IPoco
            {
                Dictionary<string, List<ITag>> TagGroups { get; set; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var complexSource = generated.FirstOrDefault( s => s.Contains( "interface IImmutableComplex" ) );
        complexSource.ShouldNotBeNull();
        complexSource.ShouldContain(
            "System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IReadOnlyList<TestApp.IImmutableTag>> TagGroups { get; }" );
        complexSource.ShouldContain( "new IComplex ToMutable();" );

        var complexPartial = generated.FirstOrDefault( s => s.Contains( "partial interface IComplex" ) );
        complexPartial.ShouldNotBeNull( "Should generate IComplex.ToImmutable partial" );
        complexPartial.ShouldContain( "new IImmutableComplex ToImmutable();" );

        var tagSource = generated.FirstOrDefault( s => s.Contains( "interface IImmutableTag" ) );
        tagSource.ShouldNotBeNull();
        tagSource.ShouldContain( "new ITag ToMutable();" );

        var tagPartial = generated.FirstOrDefault( s => s.Contains( "partial interface ITag" ) );
        tagPartial.ShouldNotBeNull( "Should generate ITag.ToImmutable partial" );
        tagPartial.ShouldContain( "new IImmutableTag ToImmutable();" );
    }

    [Test]
    public void Namespace_is_preserved_exactly()
    {
        var source = """
            using CK.Core;

            namespace My.Deep.Namespace;

            public partial interface IDeep : IPoco
            {
                int Value { get; set; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var deepSource = generated.FirstOrDefault( s => s.Contains( "IImmutableDeep" ) );
        deepSource.ShouldNotBeNull();
        deepSource.ShouldContain( "namespace My.Deep.Namespace" );
        deepSource.ShouldContain( "new IDeep ToMutable();" );

        var deepPartial = generated.FirstOrDefault( s => s.Contains( "partial interface IDeep" ) );
        deepPartial.ShouldNotBeNull( "Should generate IDeep.ToImmutable partial" );
        deepPartial.ShouldContain( "new IImmutableDeep ToImmutable();" );
    }
}
