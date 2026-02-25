using System.Text;
using Microsoft.CodeAnalysis;

namespace CK.Poco.Immutable.Generator;

static class TypeMapper
{
    internal static readonly SymbolDisplayFormat FullyQualifiedNoGlobal = SymbolDisplayFormat.FullyQualifiedFormat
        .WithGlobalNamespaceStyle( SymbolDisplayGlobalNamespaceStyle.Omitted );

    /// <summary>
    /// Returns true if the given type implements CK.Core.IPoco (directly or transitively).
    /// Uses symbol comparison instead of string concatenation.
    /// </summary>
    public static bool IsIPoco( INamedTypeSymbol type, WellKnownSymbols wellKnown )
    {
        return type.AllInterfaces.Contains( wellKnown.IPoco, SymbolEqualityComparer.Default );
    }

    /// <summary>
    /// Single source of truth for the IFoo -> IImmutableFoo naming convention.
    /// </summary>
    public static string GetImmutableName( string interfaceName )
    {
        return interfaceName.StartsWith( "I" ) && interfaceName.Length > 1
            ? "IImmutable" + interfaceName.Substring( 1 )
            : "IImmutable" + interfaceName;
    }

    /// <summary>
    /// Maps a type symbol to its immutable equivalent string representation.
    /// Rules:
    /// - List&lt;T&gt; -> IReadOnlyList&lt;T&gt;
    /// - T[] -> IReadOnlyList&lt;T&gt;
    /// - Dictionary&lt;K,V&gt; -> IReadOnlyDictionary&lt;K,V&gt;
    /// - Everything else: pass through with fully qualified (no global::) format.
    /// Type arguments are recursively mapped.
    /// </summary>
    public static string MapType( ITypeSymbol type, WellKnownSymbols wellKnown )
    {
        // Handle arrays: T[] -> IReadOnlyList<T>
        if( type is IArrayTypeSymbol arrayType )
        {
            var elementMapped = MapType( arrayType.ElementType, wellKnown );
            return "System.Collections.Generic.IReadOnlyList<" + elementMapped + ">";
        }

        // Handle value tuples: recursively map each element's type
        if( type is INamedTypeSymbol tupleType && tupleType.IsTupleType )
        {
            var elements = tupleType.TupleElements;
            var sb = new StringBuilder();
            sb.Append( '(' );
            for( int i = 0; i < elements.Length; i++ )
            {
                if( i > 0 ) sb.Append( ", " );
                var element = elements[i];
                sb.Append( MapType( element.Type, wellKnown ) );
                // Include explicit element names (skip default names like Item1, Item2, etc.)
                if( !element.IsImplicitlyDeclared )
                {
                    sb.Append( ' ' );
                    sb.Append( element.Name );
                }
            }
            sb.Append( ')' );
            return sb.ToString();
        }

        // Handle named generic types
        if( type is INamedTypeSymbol namedType && namedType.IsGenericType )
        {
            var sb = new StringBuilder();

            // List<T> -> IReadOnlyList<T>
            if( wellKnown.ListOfT is not null
                && SymbolEqualityComparer.Default.Equals( namedType.ConstructedFrom, wellKnown.ListOfT ) )
            {
                sb.Append( "System.Collections.Generic.IReadOnlyList<" );
                AppendMappedTypeArguments( sb, namedType, wellKnown );
                sb.Append( '>' );
                return sb.ToString();
            }

            // Dictionary<TKey, TValue> -> IReadOnlyDictionary<TKey, TValue>
            if( wellKnown.DictionaryOfKV is not null
                && SymbolEqualityComparer.Default.Equals( namedType.ConstructedFrom, wellKnown.DictionaryOfKV ) )
            {
                sb.Append( "System.Collections.Generic.IReadOnlyDictionary<" );
                AppendMappedTypeArguments( sb, namedType, wellKnown );
                sb.Append( '>' );
                return sb.ToString();
            }

            // For any other generic type, always recurse into type arguments
            // in case they contain collections or IPoco types that need mapping.
            var containerName = namedType.ConstructedFrom.ToDisplayString( FullyQualifiedNoGlobal );
            var angleBracketIndex = containerName.IndexOf( '<' );
            if( angleBracketIndex > 0 )
            {
                sb.Append( containerName, 0, angleBracketIndex );
                sb.Append( '<' );
                AppendMappedTypeArguments( sb, namedType, wellKnown );
                sb.Append( '>' );
                return sb.ToString();
            }
        }

        // IPoco reference: IFoo -> IImmutableFoo (fully qualified)
        if( type is INamedTypeSymbol pocoType && IsIPoco( pocoType, wellKnown ) )
        {
            return MapPocoToImmutable( pocoType );
        }

        // Default: pass through
        return type.ToDisplayString( FullyQualifiedNoGlobal );
    }

    static void AppendMappedTypeArguments( StringBuilder sb, INamedTypeSymbol namedType, WellKnownSymbols wellKnown )
    {
        var typeArgs = namedType.TypeArguments;
        for( int i = 0; i < typeArgs.Length; i++ )
        {
            if( i > 0 ) sb.Append( ", " );
            sb.Append( MapType( typeArgs[i], wellKnown ) );
        }
    }

    static string MapPocoToImmutable( INamedTypeSymbol pocoType )
    {
        var ns = pocoType.ContainingNamespace.IsGlobalNamespace
            ? null
            : pocoType.ContainingNamespace.ToDisplayString();

        var immutableName = GetImmutableName( pocoType.Name );

        return ns != null ? ns + "." + immutableName : immutableName;
    }
}
