using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocoEmit.Mapper.Configuration;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TDest"></typeparam>
/// <param name="memberComparer"></param>
public class DestTypeOptions<TDest>(IEqualityComparer<string> memberComparer)
    : DestOptions(memberComparer)
{

}
