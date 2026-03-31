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

            public partial interface IAnimal : IPoco
            {
                string Name { get; set; }
            }

            public partial interface IDog : IAnimal
            {
                string Breed { get; set; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var animalSource = generated.FirstOrDefault( s => s.Contains( "interface IImmutableAnimal" ) );
        animalSource.ShouldNotBeNull();
        animalSource.ShouldContain( "public interface IImmutableAnimal : CK.Core.IImmutablePoco" );
        animalSource.ShouldContain( "new IAnimal ToMutable();" );

        var animalPartial = generated.FirstOrDefault( s => s.Contains( "partial interface IAnimal" ) );
        animalPartial.ShouldNotBeNull( "Should generate IAnimal.ToImmutable partial" );
        animalPartial.ShouldContain( "new IImmutableAnimal ToImmutable();" );

        var dogSource = generated.FirstOrDefault( s => s.Contains( "interface IImmutableDog" ) );
        dogSource.ShouldNotBeNull();
        dogSource.ShouldContain( "public interface IImmutableDog : TestApp.IImmutableAnimal" );
        dogSource.ShouldContain( "string Breed { get; }" );
        // Must NOT redeclare inherited properties
        dogSource.ShouldNotContain( "string Name { get; }" );
        dogSource.ShouldContain( "new IDog ToMutable();" );

        var dogPartial = generated.FirstOrDefault( s => s.Contains( "partial interface IDog" ) );
        dogPartial.ShouldNotBeNull( "Should generate IDog.ToImmutable partial" );
        dogPartial.ShouldContain( "new IImmutableDog ToImmutable();" );
    }

    [Test]
    public void Diamond_IPoco_lists_all_immutable_bases()
    {
        var source = """
            using CK.Core;

            namespace TestApp;

            public partial interface IAuthored : IPoco
            {
                string Author { get; set; }
            }

            public partial interface IDated : IPoco
            {
                DateTime Date { get; set; }
            }

            public partial interface IDocument : IAuthored, IDated
            {
                string Title { get; set; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var authoredSource = generated.FirstOrDefault( s => s.Contains( "interface IImmutableAuthored" ) );
        authoredSource.ShouldNotBeNull();
        authoredSource.ShouldContain( "new IAuthored ToMutable();" );

        var authoredPartial = generated.FirstOrDefault( s => s.Contains( "partial interface IAuthored" ) );
        authoredPartial.ShouldNotBeNull( "Should generate IAuthored.ToImmutable partial" );
        authoredPartial.ShouldContain( "new IImmutableAuthored ToImmutable();" );

        var datedSource = generated.FirstOrDefault( s => s.Contains( "interface IImmutableDated" ) );
        datedSource.ShouldNotBeNull();
        datedSource.ShouldContain( "new IDated ToMutable();" );

        var datedPartial = generated.FirstOrDefault( s => s.Contains( "partial interface IDated" ) );
        datedPartial.ShouldNotBeNull( "Should generate IDated.ToImmutable partial" );
        datedPartial.ShouldContain( "new IImmutableDated ToImmutable();" );

        var docSource = generated.FirstOrDefault( s => s.Contains( "interface IImmutableDocument" ) );
        docSource.ShouldNotBeNull();
        docSource.ShouldContain( "public interface IImmutableDocument : TestApp.IImmutableAuthored, TestApp.IImmutableDated" );
        docSource.ShouldContain( "string Title { get; }" );
        // Must NOT redeclare inherited properties
        docSource.ShouldNotContain( "string Author { get; }" );
        docSource.ShouldNotContain( "DateTime Date { get; }" );
        docSource.ShouldContain( "new IDocument ToMutable();" );

        var docPartial = generated.FirstOrDefault( s => s.Contains( "partial interface IDocument" ) );
        docPartial.ShouldNotBeNull( "Should generate IDocument.ToImmutable partial" );
        docPartial.ShouldContain( "new IImmutableDocument ToImmutable();" );
    }
}
