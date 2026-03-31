using NUnit.Framework;
using Shouldly;
using System.Linq;

namespace CK.Poco.Immutable.Generator.Tests;

public class PocoReferenceMappingTests
{
    [Test]
    public void IPoco_property_maps_to_IImmutable_counterpart()
    {
        var source = """
            using CK.Core;

            namespace TestApp;

            public partial interface IOrderLine : IPoco
            {
                string Product { get; set; }
            }

            public partial interface IOrder : IPoco
            {
                IOrderLine Line { get; set; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var orderSource = generated.FirstOrDefault( s => s.Contains( "interface IImmutableOrder " ) );
        orderSource.ShouldNotBeNull();
        orderSource.ShouldContain( "TestApp.IImmutableOrderLine Line { get; }" );
        orderSource.ShouldContain( "new IOrder ToMutable();" );

        var orderPartial = generated.FirstOrDefault( s => s.Contains( "new IImmutableOrder ToImmutable();" ) );
        orderPartial.ShouldNotBeNull( "Should generate IOrder.ToImmutable partial" );
        orderPartial.ShouldContain( "partial interface IOrder" );

        var lineSource = generated.FirstOrDefault( s => s.Contains( "interface IImmutableOrderLine" ) );
        lineSource.ShouldNotBeNull();
        lineSource.ShouldContain( "new IOrderLine ToMutable();" );

        var linePartial = generated.FirstOrDefault( s => s.Contains( "partial interface IOrderLine" ) );
        linePartial.ShouldNotBeNull( "Should generate IOrderLine.ToImmutable partial" );
        linePartial.ShouldContain( "new IImmutableOrderLine ToImmutable();" );
    }

    [Test]
    public void List_of_IPoco_maps_to_IReadOnlyList_of_IImmutable()
    {
        var source = """
            using CK.Core;
            using System.Collections.Generic;

            namespace TestApp;

            public partial interface IItem : IPoco
            {
                string Name { get; set; }
            }

            public partial interface IContainer : IPoco
            {
                List<IItem> Items { get; set; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var containerSource = generated.FirstOrDefault( s => s.Contains( "IImmutableContainer" ) );
        containerSource.ShouldNotBeNull();
        containerSource.ShouldContain( "System.Collections.Generic.IReadOnlyList<TestApp.IImmutableItem> Items { get; }" );
        containerSource.ShouldContain( "new IContainer ToMutable();" );

        var containerPartial = generated.FirstOrDefault( s => s.Contains( "partial interface IContainer" ) );
        containerPartial.ShouldNotBeNull( "Should generate IContainer.ToImmutable partial" );
        containerPartial.ShouldContain( "new IImmutableContainer ToImmutable();" );

        var itemSource = generated.FirstOrDefault( s => s.Contains( "interface IImmutableItem" ) );
        itemSource.ShouldNotBeNull();
        itemSource.ShouldContain( "new IItem ToMutable();" );

        var itemPartial = generated.FirstOrDefault( s => s.Contains( "partial interface IItem" ) );
        itemPartial.ShouldNotBeNull( "Should generate IItem.ToImmutable partial" );
        itemPartial.ShouldContain( "new IImmutableItem ToImmutable();" );
    }
}
