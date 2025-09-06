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
    , IEmitComplexConverter
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
    Expression IEmitConverter.Convert(Expression source)
        => Convert(new(), source);
    /// <inheritdoc />
    public Expression Convert(ComplexContext cacher, Expression source)
    {
        var dest = Expression.Variable(_collectionType, "dest");
        var assign = Expression.Assign(dest, New(source));
        var list = new List<Expression>() { assign };
        list.AddRange(_copier.Copy(cacher, source, dest));
        list.Add(dest);
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
