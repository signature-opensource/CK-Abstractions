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

            public partial interface IWithRefTuple : IPoco
            {
                ref (int Id, string Label) Tag { get; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var immutableSource = generated.FirstOrDefault( s => s.Contains( "IImmutableWithRefTuple" ) );
        immutableSource.ShouldNotBeNull();
        immutableSource.ShouldContain( "(int Id, string Label) Tag { get; }" );
        immutableSource.ShouldNotContain( "ref " );
        immutableSource.ShouldContain( "new IWithRefTuple ToMutable();" );

        var partialSource = generated.FirstOrDefault( s => s.Contains( "partial interface IWithRefTuple" ) );
        partialSource.ShouldNotBeNull( "Should generate IWithRefTuple.ToImmutable partial" );
        partialSource.ShouldContain( "new IImmutableWithRefTuple ToImmutable();" );
    }

    [Test]
    public void Value_tuple_with_IPoco_element_maps_element_to_IImmutable()
    {
        var source = """
            using CK.Core;

            namespace TestApp;

            public partial interface IItem : IPoco
            {
                string Name { get; set; }
            }

            public partial interface IWithPocoTuple : IPoco
            {
                ref (string Label, IItem Item) Tagged { get; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var immutableSource = generated.FirstOrDefault( s => s.Contains( "IImmutableWithPocoTuple" ) );
        immutableSource.ShouldNotBeNull();
        immutableSource.ShouldContain( "(string Label, TestApp.IImmutableItem Item) Tagged { get; }" );
        immutableSource.ShouldNotContain( "ref " );
        immutableSource.ShouldContain( "new IWithPocoTuple ToMutable();" );

        var partialSource = generated.FirstOrDefault( s => s.Contains( "partial interface IWithPocoTuple" ) );
        partialSource.ShouldNotBeNull( "Should generate IWithPocoTuple.ToImmutable partial" );
        partialSource.ShouldContain( "new IImmutableWithPocoTuple ToImmutable();" );

        var itemSource = generated.FirstOrDefault( s => s.Contains( "interface IImmutableItem" ) );
        itemSource.ShouldNotBeNull();
        itemSource.ShouldContain( "new IItem ToMutable();" );

        var itemPartial = generated.FirstOrDefault( s => s.Contains( "partial interface IItem" ) );
        itemPartial.ShouldNotBeNull( "Should generate IItem.ToImmutable partial" );
        itemPartial.ShouldContain( "new IImmutableItem ToImmutable();" );
    }
}
