using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocoEmit.Mapper.Configuration;

/// <summary>
/// 映射目标配置
/// </summary>
/// <param name="memberComparer"></param>
public class DestOptions(IEqualityComparer<string> memberComparer)
    : OptionBase(memberComparer)
{
}
