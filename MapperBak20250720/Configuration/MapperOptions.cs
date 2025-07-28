using System.Collections.Generic;

namespace PocoEmit.Mapper.Configuration;

/// <summary>
/// 
/// </summary>
/// <param name="memberComparer"></param>
public class MapperOptions(IEqualityComparer<string> memberComparer)
    : OptionBase(memberComparer)
{
}
