using NUnit.Framework;
using Shouldly;
using System.Linq;

namespace CK.Poco.Immutable.Generator.Tests;

public class BasicGenerationTests
{
    [Test]
    public void Basic_IPoco_generates_IImmutable_with_readonly_properties()
    {
        var source = """
            using CK.Core;

            namespace TestApp;

            public interface IUserInfo : IPoco
            {
                string Name { get; set; }
                int Age { get; set; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        diagnostics.ShouldBeEmpty();
        // Find the generated source (not the IImmutablePoco stub)
        var immutableSource = generated.FirstOrDefault( s => s.Contains( "IImmutableUserInfo" ) );
        immutableSource.ShouldNotBeNull( "Should generate IImmutableUserInfo" );
        immutableSource.ShouldContain( "namespace TestApp" );
        immutableSource.ShouldContain( "public interface IImmutableUserInfo : CK.Core.IImmutablePoco" );
        immutableSource.ShouldContain( "string Name { get; }" );
        immutableSource.ShouldContain( "int Age { get; }" );
        // Must NOT contain setters
        immutableSource.ShouldNotContain( "set;" );
    }

    [Test]
    public void Non_IPoco_interface_is_ignored()
    {
        var source = """
            namespace TestApp;

            public interface INotAPoco
            {
                string Name { get; set; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var immutableSource = generated.FirstOrDefault( s => s.Contains( "IImmutableNotAPoco" ) );
        immutableSource.ShouldBeNull( "Should NOT generate for non-IPoco interfaces" );
    }

    [Test]
    public void Empty_IPoco_generates_empty_immutable_interface()
    {
        var source = """
            using CK.Core;

            namespace TestApp;

            public interface IEmpty : IPoco
            {
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var immutableSource = generated.FirstOrDefault( s => s.Contains( "IImmutableEmpty" ) );
        immutableSource.ShouldNotBeNull( "Should generate IImmutableEmpty" );
        immutableSource.ShouldContain( "public interface IImmutableEmpty : CK.Core.IImmutablePoco" );
    }
}
