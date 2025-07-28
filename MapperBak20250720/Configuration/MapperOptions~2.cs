using System.Collections.Generic;

namespace PocoEmit.Mapper.Configuration;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
/// <param name="memberComparer"></param>
public class MapperOptions<TSource, TDest>(IEqualityComparer<string> memberComparer) 
    : MapperOptions(memberComparer)
{
}
