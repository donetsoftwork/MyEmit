using PocoEmit.Activators;
using PocoEmit.Builders;
using PocoEmit.Configuration;
using PocoEmit.Copies;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// 复合类型转化器
/// </summary>
/// <param name="sourceType"></param>
/// <param name="destActivator"></param>
/// <param name="copier"></param>
public class ComplexTypeConverter(Type sourceType, IEmitActivator destActivator, IEmitCopier copier)
    : IEmitComplexConverter
    , IEmitConverter
    , IEmitBuilder
{
    #region 配置
    private readonly Type _sourceType = sourceType; 
    private readonly IEmitActivator _destActivator = destActivator;
    private readonly IEmitCopier _copier = copier;
    /// <summary>
    /// 映射源类型
    /// </summary>
    public Type SourceType
        => _sourceType;
    /// <summary>
    /// 激活映射目标
    /// </summary>
    public IEmitActivator DestActivator 
        => _destActivator;
    /// <summary>
    /// 复制
    /// </summary>
    public IEmitCopier Copier 
        => _copier;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    Expression IEmitConverter.Convert(Expression source)
        => Expression.Invoke(Build(), source);
    /// <inheritdoc />
    public Expression Convert(ComplexContext cacher, Expression source)
    {
        Type destType = _destActivator.ReturnType;
        var key = new PairTypeKey(_sourceType, destType);
        if (!cacher.TryGetValue(key, out var complex))
            cacher.Set(key, complex = BuildCore(cacher, destType));
        return Expression.Invoke(complex, source);
    }
    /// <summary>
    /// 构造表达式
    /// </summary>
    /// <returns></returns>
    public Expression Build()
        => BuildCore(new(), _destActivator.ReturnType);
    /// <summary>
    /// 构造表达式
    /// </summary>
    /// <param name="cacher"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    private LambdaExpression BuildCore(ComplexContext cacher, Type destType)
    {
        var source = Expression.Variable(_sourceType, "source");
        var dest = Expression.Variable(destType, "dest");
        List<ParameterExpression> variables = [dest];
        List<Expression> list = [];
        if (PairTypeKey.CheckNullCondition(_sourceType))
        {
            list.Add(Expression.IfThen(
                    Expression.NotEqual(source, Expression.Constant(null, _sourceType)),
                    Expression.Block(ConvertCore(cacher, source, dest))
                )
            );
        }
        else
        {
            list.AddRange(ConvertCore(cacher, source, dest));
        }
        list.Add(dest);
        var funcType = Expression.GetFuncType(_sourceType, destType);
        return Expression.Lambda(funcType, Expression.Block(variables, list), source);
    }

    /// <summary>
    /// 转化核心方法
    /// </summary>
    /// <param name="cacher"></param>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    private List<Expression> ConvertCore(ComplexContext cacher, Expression source, ParameterExpression dest)
    {
        var assign = Expression.Assign(dest, _destActivator.New(cacher, source));
        var list = new List<Expression>() { assign };
        if (_copier is not null)
            list.AddRange(_copier.Copy(cacher, source, dest));
        return list;
    }
}
