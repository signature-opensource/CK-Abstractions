using NUnit.Framework;
using Shouldly;
using System.Linq;

namespace CK.Poco.Immutable.Generator.Tests;

public class CollectionMappingTests
{
    [Test]
    public void List_maps_to_IReadOnlyList()
    {
        var source = """
            using CK.Core;
            using System.Collections.Generic;

            namespace TestApp;

            public partial interface IWithList : IPoco
            {
                List<string> Names { get; set; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var immutableSource = generated.FirstOrDefault( s => s.Contains( "IImmutableWithList" ) );
        immutableSource.ShouldNotBeNull();
        immutableSource.ShouldContain( "System.Collections.Generic.IReadOnlyList<string> Names { get; }" );
        immutableSource.ShouldContain( "new IWithList ToMutable();" );

        var partialSource = generated.FirstOrDefault( s => s.Contains( "partial interface IWithList" ) );
        partialSource.ShouldNotBeNull( "Should generate IWithList.ToImmutable partial" );
        partialSource.ShouldContain( "new IImmutableWithList ToImmutable();" );
    }

    [Test]
    public void Array_maps_to_IReadOnlyList()
    {
        var source = """
            using CK.Core;

            namespace TestApp;

            public partial interface IWithArray : IPoco
            {
                int[] Values { get; set; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var immutableSource = generated.FirstOrDefault( s => s.Contains( "IImmutableWithArray" ) );
        immutableSource.ShouldNotBeNull();
        immutableSource.ShouldContain( "System.Collections.Generic.IReadOnlyList<int> Values { get; }" );
        immutableSource.ShouldContain( "new IWithArray ToMutable();" );

        var partialSource = generated.FirstOrDefault( s => s.Contains( "partial interface IWithArray" ) );
        partialSource.ShouldNotBeNull( "Should generate IWithArray.ToImmutable partial" );
        partialSource.ShouldContain( "new IImmutableWithArray ToImmutable();" );
    }

    [Test]
    public void Dictionary_maps_to_IReadOnlyDictionary()
    {
        var source = """
            using CK.Core;
            using System.Collections.Generic;

            namespace TestApp;

            public partial interface IWithDict : IPoco
            {
                Dictionary<string, int> Scores { get; set; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var immutableSource = generated.FirstOrDefault( s => s.Contains( "IImmutableWithDict" ) );
        immutableSource.ShouldNotBeNull();
        immutableSource.ShouldContain( "System.Collections.Generic.IReadOnlyDictionary<string, int> Scores { get; }" );
        immutableSource.ShouldContain( "new IWithDict ToMutable();" );

        var partialSource = generated.FirstOrDefault( s => s.Contains( "partial interface IWithDict" ) );
        partialSource.ShouldNotBeNull( "Should generate IWithDict.ToImmutable partial" );
        partialSource.ShouldContain( "new IImmutableWithDict ToImmutable();" );
    }
}
