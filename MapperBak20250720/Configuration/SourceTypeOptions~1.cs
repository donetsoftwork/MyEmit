using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocoEmit.Mapper.Configuration;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TSource"></typeparam>
public class SourceTypeOptions<TSource>(IEqualityComparer<string> memberComparer) 
    : SourceOptions(memberComparer)
{

}
