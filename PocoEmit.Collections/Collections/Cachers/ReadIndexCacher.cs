using PocoEmit.Collections.Indexs;
using PocoEmit.Indexs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocoEmit.Collections.Cachers;

/// <summary>
/// 索引器缓存
/// </summary>
internal class ReadIndexCacher(ICacher<Type, IEmitIndexMemberReader> cacher)
    : CacheBase<Type, IEmitIndexMemberReader>(cacher)
{
    /// <inheritdoc />
    protected override IEmitIndexMemberReader CreateNew(Type key)
    {
        if (key.IsArray)
            return ArrayMemberIndex.Instance;
        if (ReflectionHelper.HasGenericType(key, typeof(IList<>)))
            return PropertyIndexMemberReader.Create(key);
        return null;
    }
}
