using PocoEmit.Collections.Activators;
using PocoEmit.Collections.Counters;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Copies;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Collections.Converters;

/// <summary>
/// 集合类型转化
/// </summary>
/// <param name="sourceType"></param>
/// <param name="collectionType"></param>
/// <param name="elementType"></param>
/// <param name="capacityConstructor"></param>
/// <param name="sourceCount"></param>
/// <param name="copier"></param>
public class CollectionConverter(Type sourceType, Type collectionType, Type elementType, ConstructorInfo capacityConstructor, IEmitCounter sourceCount, IEmitCopier copier)
    : CollectionActivator(collectionType, elementType, capacityConstructor, sourceCount)
    , IComplexIncludeConverter
{
    #region 配置
    private readonly PairTypeKey _key = new(sourceType, collectionType);
    private readonly IEmitCopier _copier = copier;
    /// <summary>
    /// 复制
    /// </summary>
    public IEmitCopier Copier
        => _copier;
    #endregion
    /// <inheritdoc />
    IEnumerable<ComplexBundle> IComplexPreview.Preview(IComplexBundle parent)
        => _copier.Preview(parent);
    /// <inheritdoc />
    public Expression Convert(IBuildContext context, Expression source)
    {
        var dest = Expression.Variable(_collectionType, "dest");

        var assign = Expression.Assign(dest, New(context, source));
        var list = new List<Expression>() { assign };
        var cache = context.SetCache(_key, source, dest);
        if (cache is not null)
            list.Add(cache);
        list.AddRange(_copier.Copy(context, source, dest));
        list.Add(dest);
        return Expression.Block([dest], list);
    }
}
