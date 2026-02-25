using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace CK.Poco.Immutable.Generator.Tests;

public class InheritanceMappingTests
{
    [Test]
    public void Derived_IPoco_mirrors_inheritance_chain()
    {
        var source = """
            using CK.Core;

            namespace TestApp;

            public interface IAnimal : IPoco
            {
                string Name { get; set; }
            }

            public interface IDog : IAnimal
            {
                string Breed { get; set; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var animalSource = generated.FirstOrDefault( s => s.Contains( "interface IImmutableAnimal" ) );
        animalSource.ShouldNotBeNull();
        animalSource.ShouldContain( "public interface IImmutableAnimal : CK.Core.IImmutablePoco" );

        var dogSource = generated.FirstOrDefault( s => s.Contains( "interface IImmutableDog" ) );
        dogSource.ShouldNotBeNull();
        dogSource.ShouldContain( "public interface IImmutableDog : TestApp.IImmutableAnimal" );
        dogSource.ShouldContain( "string Breed { get; }" );
        // Must NOT redeclare inherited properties
        dogSource.ShouldNotContain( "string Name { get; }" );
    }

    [Test]
    public void Diamond_IPoco_lists_all_immutable_bases()
    {
        var source = """
            using CK.Core;

            namespace TestApp;

            public interface IAuthored : IPoco
            {
                string Author { get; set; }
            }

            public interface IDated : IPoco
            {
                DateTime Date { get; set; }
            }

            public interface IDocument : IAuthored, IDated
            {
                string Title { get; set; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var docSource = generated.FirstOrDefault( s => s.Contains( "interface IImmutableDocument" ) );
        docSource.ShouldNotBeNull();
        docSource.ShouldContain( "public interface IImmutableDocument : TestApp.IImmutableAuthored, TestApp.IImmutableDated" );
        docSource.ShouldContain( "string Title { get; }" );
        // Must NOT redeclare inherited properties
        docSource.ShouldNotContain( "string Author { get; }" );
        docSource.ShouldNotContain( "DateTime Date { get; }" );
    }
}
