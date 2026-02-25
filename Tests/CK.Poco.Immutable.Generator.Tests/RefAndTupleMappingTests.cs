using NUnit.Framework;
using Shouldly;
using System.Linq;

namespace CK.Poco.Immutable.Generator.Tests;

public class RefAndTupleMappingTests
{
    [Test]
    public void Ref_value_tuple_drops_ref_and_preserves_tuple()
    {
        var source = """
            using CK.Core;

            namespace TestApp;

            public interface IWithRefTuple : IPoco
            {
                ref (int Id, string Label) Tag { get; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var immutableSource = generated.FirstOrDefault( s => s.Contains( "IImmutableWithRefTuple" ) );
        immutableSource.ShouldNotBeNull();
        immutableSource.ShouldContain( "(int Id, string Label) Tag { get; }" );
        immutableSource.ShouldNotContain( "ref " );
    }

    [Test]
    public void Value_tuple_with_IPoco_element_maps_element_to_IImmutable()
    {
        var source = """
            using CK.Core;

            namespace TestApp;

            public interface IItem : IPoco
            {
                string Name { get; set; }
            }

            public interface IWithPocoTuple : IPoco
            {
                ref (string Label, IItem Item) Tagged { get; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var immutableSource = generated.FirstOrDefault( s => s.Contains( "IImmutableWithPocoTuple" ) );
        immutableSource.ShouldNotBeNull();
        immutableSource.ShouldContain( "(string Label, TestApp.IImmutableItem Item) Tagged { get; }" );
        immutableSource.ShouldNotContain( "ref " );
    }
}
