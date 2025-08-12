using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Copies;
using PocoEmit.Members;
using System.Collections.Generic;

namespace PocoEmit.Builders;

/// <summary>
/// 同类型复制器构建器
/// </summary>
/// <param name="options"></param>
public class CopyToSelf(IMapperOptions options)
    : CopierBuilderBase(options)
{
    /// <inheritdoc />
    public override void CheckMembers(MapTypeKey key, IEnumerable<IEmitMemberWriter> destMembers, ICollection<IMemberConverter> converters)
    {
        var readerCacher = MemberContainer.Instance.MemberReaderCacher;
        foreach (var writer in destMembers)
        {
            var reader = readerCacher.Get(writer.Info);
            if (reader is null)
                continue;
            converters.Add(new MemberConverter(_options, reader, writer));
        }
    }
}
