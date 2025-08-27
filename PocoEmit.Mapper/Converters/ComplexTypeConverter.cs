using PocoEmit.Activators;
using PocoEmit.Builders;
using PocoEmit.Configuration;
using PocoEmit.Copies;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// 复合类型转化器
/// </summary>
/// <param name="destActivator"></param>
/// <param name="copier"></param>
public class ComplexTypeConverter(IEmitActivator destActivator, IEmitCopier copier)
    : IEmitConverter
{
    #region 配置
    private readonly IEmitActivator _destActivator = destActivator;
    private readonly IEmitCopier _copier = copier;
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
    public Expression Convert(Expression source)
    {
        List<ParameterExpression> variables = [];
        var sourceType = source.Type;
        var destType = _destActivator.ReturnType;
        var dest = Expression.Variable(destType, "dest");
        variables.Add(dest);
        if(PairTypeKey.CheckNullCondition(sourceType))
        {
            var list = new List<Expression>();
            if (EmitHelper.CheckComplexSource(source, false))
            {
                var source2 = Expression.Variable(sourceType, "source");
                variables.Add(source2);
                list.Add(Expression.Assign(source2, source));
                source = source2;
            }
            list.Add(Expression.Condition(
                    Expression.Equal(source, Expression.Constant(null, sourceType)),
                    Expression.Default(destType),
                    Expression.Block(ConvertCore(source, dest))
                )
            );

            return Expression.Block(variables, list);
        }
        return Expression.Block(variables, ConvertCore(source, dest));
    }
    /// <summary>
    /// 转化核心方法
    /// </summary>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    private List<Expression> ConvertCore(Expression source, ParameterExpression dest)
    {
        var assign = Expression.Assign(dest, _destActivator.New(source));
        var list = new List<Expression>() { assign };
        if (_copier is not null)
            list.AddRange(_copier.Copy(source, dest));
        list.Add(dest);
        return list;
    }
}
