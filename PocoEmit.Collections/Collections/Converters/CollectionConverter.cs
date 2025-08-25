using PocoEmit.Collections.Activators;
using PocoEmit.Collections.Counters;
using PocoEmit.Converters;
using PocoEmit.Copies;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Converters;

/// <summary>
/// 集合类型转化
/// </summary>
/// <param name="elementType"></param>
/// <param name="collectionType"></param>
/// <param name="sourceCount"></param>
/// <param name="copier"></param>
public class CollectionConverter(Type collectionType, Type elementType, IEmitCounter sourceCount, IEmitCopier copier)
    : CollectionActivator(collectionType, elementType, sourceCount)
    , IEmitConverter
{
    #region 配置
    private readonly IEmitCopier _copier = copier;
    /// <summary>
    /// 复制
    /// </summary>
    public IEmitCopier Copier
        => _copier;
    #endregion
    /// <inheritdoc />
    public Expression Convert(Expression source)
    {
        var dest = Expression.Variable(_collectionType, "dest");

        var assign = Expression.Assign(dest, New(source));
        var list = new List<Expression>() { assign };
        list.AddRange(_copier.Copy(source, dest));
        list.Add(dest);
        return Expression.Block([dest], list);
    }
}
