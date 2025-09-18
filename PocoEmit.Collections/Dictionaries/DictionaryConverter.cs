using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Copies;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Dictionaries;

/// <summary>
/// 字典激活
/// </summary>
public class DictionaryConverter(Type instanceType, Type dictionaryType, Type keyType, Type elementType, IEmitCopier copier)
    : EmitDictionaryBase(dictionaryType, keyType, elementType)
    , IComplexIncludeConverter
{
    #region 配置
    private readonly PairTypeKey _key = new(instanceType, dictionaryType);
    private readonly IEmitCopier _copier = copier;
    /// <inheritdoc />
    public PairTypeKey Key
        => _key;
    /// <summary>
    /// 复制
    /// </summary>
    public IEmitCopier Copier
        => _copier;
    #endregion
    /// <inheritdoc />
    IEnumerable<ComplexBundle> IComplexPreview.Preview(IComplexBundle parent)
         => _copier.Preview(parent);
    /// <summary>
    /// 转化核心方法
    /// </summary>
    /// <param name="context"></param>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    public List<Expression> ConvertCore(IBuildContext context, Expression source, Expression dest)
    {
        var assign = Expression.Assign(dest, New(source));
        var list = new List<Expression>() { assign };
        list.AddRange(_copier.Copy(context, source, dest));
        list.Add(dest);
        return list;
    }
    /// <summary>
    /// 激活
    /// </summary>
    /// <param name="argument"></param>
    /// <returns></returns>
    public Expression New(Expression argument)
        => Expression.New(_collectionType);

    #region IEmitComplexConverter
    /// <inheritdoc />
    public Expression Convert(IBuildContext context, Expression source)
    {
        var lambda = Build(context);
        return context.Call(lambda, source);
    }
    /// <inheritdoc />
    public LambdaExpression Build(IBuildContext context)
    {
        if (context.TryGetLambda(_key, out LambdaExpression lambda))
            return lambda;
        var sourcetype = _key.LeftType;
        var destType = _key.RightType;
        var source = Expression.Variable(sourcetype, "source");
        var dest = Expression.Variable(destType, "dest");
        List<ParameterExpression> variables = [dest];
        List<Expression> list = [];
        if (PairTypeKey.CheckNullCondition(sourcetype))
        {
            list.Add(Expression.IfThen(
                    Expression.NotEqual(source, Expression.Constant(null, sourcetype)),
                    Expression.Block(ConvertCore(context, source, dest))
                )
            );
        }
        else
        {
            list.AddRange(ConvertCore(context, source, dest));
        }
        list.Add(dest);
        var funcType = Expression.GetFuncType(sourcetype, destType);
        return Expression.Lambda(funcType, Expression.Block(variables, list), source);
    }
    #endregion
}
