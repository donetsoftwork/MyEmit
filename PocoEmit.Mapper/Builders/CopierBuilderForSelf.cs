using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Copies;
using PocoEmit.Copies;
using System.Collections.Generic;
using System.Reflection;

namespace PocoEmit.Builders;

/// <summary>
/// 同类型复制器构建器
/// </summary>
/// <param name="factory"></param>
public class CopierBuilderForSelf(CopierFactory factory)
    : CopierBuilderBase(factory)
{
    /// <inheritdoc />
    protected override void CheckMembers(MapTypeKey key, IEnumerable<MemberInfo> destMembers, ICollection<IMemberConverter> converters)
    {
        var container = MemberContainer.Instance;
        foreach (var member in destMembers)
        {
            var reader = container.MemberReaderCacher.Get(member);
            if (reader is null)
                continue;
            var writer = container.MemberWriterCacher.Get(member);
            if (writer is null)
                continue;
            converters.Add(new MemberConverter(reader, writer));
        }
    }
}
