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

            public interface IOrderLine : IPoco
            {
                string Product { get; set; }
            }

            public interface IOrder : IPoco
            {
                IOrderLine Line { get; set; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var orderSource = generated.FirstOrDefault( s => s.Contains( "interface IImmutableOrder " ) );
        orderSource.ShouldNotBeNull();
        orderSource.ShouldContain( "TestApp.IImmutableOrderLine Line { get; }" );
    }

    [Test]
    public void List_of_IPoco_maps_to_IReadOnlyList_of_IImmutable()
    {
        var source = """
            using CK.Core;
            using System.Collections.Generic;

            namespace TestApp;

            public interface IItem : IPoco
            {
                string Name { get; set; }
            }

            public interface IContainer : IPoco
            {
                List<IItem> Items { get; set; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var containerSource = generated.FirstOrDefault( s => s.Contains( "IImmutableContainer" ) );
        containerSource.ShouldNotBeNull();
        containerSource.ShouldContain( "System.Collections.Generic.IReadOnlyList<TestApp.IImmutableItem> Items { get; }" );
    }
}
