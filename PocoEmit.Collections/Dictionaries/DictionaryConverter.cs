using PocoEmit.Converters;
using PocoEmit.Copies;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Dictionaries;

/// <summary>
/// 字典激活
/// </summary>
public class DictionaryConverter(Type dictionaryType, Type keyType, Type elementType, IEmitCopier copier)
    : EmitDictionaryBase(dictionaryType, keyType, elementType)
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
        var destTarget = Expression.Label(_collectionType, "returndest");

        var assign = Expression.Assign(dest, New(source));
        var list = new List<Expression>() { assign };
        list.AddRange(_copier.Copy(source, dest));
        //list.Add(Expression.Return(destTarget, dest));
        list.Add(Expression.Label(destTarget, dest));
        return Expression.Block([dest], list);
    }

    /// <summary>
    /// 激活
    /// </summary>
    /// <param name="argument"></param>
    /// <returns></returns>
    public Expression New(Expression argument)
        => Expression.New(_collectionType);
}
