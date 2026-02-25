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

            public partial interface IUserInfo : IPoco
            {
                string Name { get; set; }
                int Age { get; set; }
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        diagnostics.ShouldBeEmpty();
        var immutableSource = generated.FirstOrDefault( s => s.Contains( "IImmutableUserInfo" ) );
        immutableSource.ShouldNotBeNull( "Should generate IImmutableUserInfo" );
        immutableSource.ShouldContain( "namespace TestApp" );
        immutableSource.ShouldContain( "public interface IImmutableUserInfo : CK.Core.IImmutablePoco" );
        immutableSource.ShouldContain( "string Name { get; }" );
        immutableSource.ShouldContain( "int Age { get; }" );
        // Must NOT contain setters
        immutableSource.ShouldNotContain( "set;" );
        immutableSource.ShouldContain( "new IUserInfo ToMutable();" );

        var partialSource = generated.FirstOrDefault( s => s.Contains( "partial interface IUserInfo" ) );
        partialSource.ShouldNotBeNull( "Should generate IUserInfo.ToImmutable partial" );
        partialSource.ShouldContain( "new IImmutableUserInfo ToImmutable();" );
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

            public partial interface IEmpty : IPoco
            {
            }
            """;

        var (diagnostics, generated) = GeneratorTestHelper.RunGenerator( source );

        var immutableSource = generated.FirstOrDefault( s => s.Contains( "IImmutableEmpty" ) );
        immutableSource.ShouldNotBeNull( "Should generate IImmutableEmpty" );
        immutableSource.ShouldContain( "public interface IImmutableEmpty : CK.Core.IImmutablePoco" );
        immutableSource.ShouldContain( "new IEmpty ToMutable();" );

        var partialSource = generated.FirstOrDefault( s => s.Contains( "partial interface IEmpty" ) );
        partialSource.ShouldNotBeNull( "Should generate IEmpty.ToImmutable partial" );
        partialSource.ShouldContain( "new IImmutableEmpty ToImmutable();" );
    }
}
