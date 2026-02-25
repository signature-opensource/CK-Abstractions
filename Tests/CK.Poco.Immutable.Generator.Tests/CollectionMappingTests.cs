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

            public interface IWithList : IPoco
            {
                List<string> Names { get; set; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var immutableSource = generated.FirstOrDefault( s => s.Contains( "IImmutableWithList" ) );
        immutableSource.ShouldNotBeNull();
        immutableSource.ShouldContain( "System.Collections.Generic.IReadOnlyList<string> Names { get; }" );
    }

    [Test]
    public void Array_maps_to_IReadOnlyList()
    {
        var source = """
            using CK.Core;

            namespace TestApp;

            public interface IWithArray : IPoco
            {
                int[] Values { get; set; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var immutableSource = generated.FirstOrDefault( s => s.Contains( "IImmutableWithArray" ) );
        immutableSource.ShouldNotBeNull();
        immutableSource.ShouldContain( "System.Collections.Generic.IReadOnlyList<int> Values { get; }" );
    }

    [Test]
    public void Dictionary_maps_to_IReadOnlyDictionary()
    {
        var source = """
            using CK.Core;
            using System.Collections.Generic;

            namespace TestApp;

            public interface IWithDict : IPoco
            {
                Dictionary<string, int> Scores { get; set; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var immutableSource = generated.FirstOrDefault( s => s.Contains( "IImmutableWithDict" ) );
        immutableSource.ShouldNotBeNull();
        immutableSource.ShouldContain( "System.Collections.Generic.IReadOnlyDictionary<string, int> Scores { get; }" );
    }
}
