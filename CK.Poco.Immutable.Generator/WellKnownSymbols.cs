#nullable enable
using Microsoft.CodeAnalysis;

namespace CK.Poco.Immutable.Generator;

readonly struct WellKnownSymbols
{
    public readonly INamedTypeSymbol IPoco;
    public readonly INamedTypeSymbol? ListOfT;
    public readonly INamedTypeSymbol? DictionaryOfKV;

    WellKnownSymbols( INamedTypeSymbol iPoco, INamedTypeSymbol? listOfT, INamedTypeSymbol? dictionaryOfKV )
    {
        IPoco = iPoco;
        ListOfT = listOfT;
        DictionaryOfKV = dictionaryOfKV;
    }

    public static WellKnownSymbols? TryCreate( Compilation compilation )
    {
        var iPoco = compilation.GetTypeByMetadataName( "CK.Core.IPoco" );
        if( iPoco is null ) return null;
        var listOfT = compilation.GetTypeByMetadataName( "System.Collections.Generic.List`1" );
        var dictOfKV = compilation.GetTypeByMetadataName( "System.Collections.Generic.Dictionary`2" );
        return new WellKnownSymbols( iPoco, listOfT, dictOfKV );
    }
}
