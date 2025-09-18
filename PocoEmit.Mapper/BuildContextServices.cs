using PocoEmit.Builders;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Members;
using PocoEmit.Resolves;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PocoEmit;

/// <summary>
/// 上下文扩展方法
/// </summary>
public static partial class MapperServices
{
    /// <summary>
    /// 获取执行上下文
    /// </summary>
    /// <param name="context"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static IEnumerable<ParameterExpression> GetConvertContexts(this IBuildContext context, PairTypeKey key)
    {
        var bundle = context.GetBundle(key);
        if (bundle is null || !bundle.IsCircle)
            return [];
        return context.ConvertContexts;
    }
    /// <summary>
    /// 读取检查复杂类型
    /// </summary>
    /// <param name="context"></param>
    /// <param name="reader"></param>
    /// <param name="instance"></param>
    /// <returns></returns>
    internal static Expression Read(this IBuildContext context, IEmitReader reader, Expression instance)
    {
        if (reader is ConvertInstanceReader instanceReader)
            return Read(context, instanceReader.Original, Convert(context, instanceReader.Converter, instance));
        else if (reader is ConvertValueReader valueReader)
            return Convert(context, valueReader.Converter, Read(context, valueReader.Original, instance));

        return reader.Read(instance);
    }
    /// <summary>
    /// 转化检查复杂类型
    /// </summary>
    /// <param name="context"></param>
    /// <param name="converter"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    internal static Expression Convert(this IBuildContext context, IEmitConverter converter, Expression source)
    {
        var key = converter.Key;
        if (context.TryGetLambda(key, out LambdaExpression lambda))
            return ConvertByLambda(context, key, lambda, source);
        var convertContexts = context.ConvertContexts;
        if (convertContexts.Count == 1)
        {
            var bundle = context.GetBundle(key);
            if(bundle is not null && bundle.IsCircle)
            {
                var sourceType = key.LeftType;
                var destType = key.RightType;
                if (PairTypeKey.CheckNullCondition(sourceType))
                {
                    return Expression.Condition(
                        Expression.Equal(source, Expression.Constant(null, sourceType)),
                        Expression.Default(destType),
                        CallContextConvert(context, key, convertContexts[0], source)
                    );
                }
                else
                {
                    return CallContextConvert(context, key, convertContexts[0], source);
                }
            }
        }
        
        if (converter is IComplexIncludeConverter complexConverter)
        {
            return complexConverter.Convert(context, source);
        }
        else
        {
            return converter.Convert(source);
        }
    }
    /// <summary>
    /// 调用上下文转化
    /// </summary>
    /// <param name="context"></param>
    /// <param name="key"></param>
    /// <param name="convertContext"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    internal static Expression CallContextConvert(this IBuildContext context, PairTypeKey key, ParameterExpression convertContext, Expression source)
    {
        if(context.TryGetContextLambda(key, out LambdaExpression contextLambda))
            return Expression.Invoke(contextLambda, convertContext, source);
        return ConvertContext.CallConvert(convertContext, key, source);
    }
    /// <summary>
    /// 使用表达式转化
    /// </summary>
    /// <param name="context"></param>
    /// <param name="key"></param>
    /// <param name="lambda"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    internal static Expression ConvertByLambda(this IBuildContext context, PairTypeKey key, LambdaExpression lambda, Expression source)
    {
        var sourceType = key.LeftType;
        var destType = key.RightType;
        if (PairTypeKey.CheckNullCondition(sourceType, destType))
        {
            return Expression.Condition(
                Expression.Equal(source, Expression.Constant(null, sourceType)),
                Expression.Default(destType),
                context.Call(lambda, source)
            );
        }
        else
        {
            return context.Call(lambda, source);
        }
    }
    /// <summary>
    /// 切换进入新上下文
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    internal static IBuildContext Enter(this IBuildContext parent, PairTypeKey key)
    {
        var convertContexts = parent.ConvertContexts;
        if (convertContexts.Count == 0)
            return parent;
        var bundle = parent.GetBundle(key);
        if (bundle is null)
            return parent;
        if (bundle.IsCircle || bundle.HasCircle)
            return new CurrentContext(parent.Context, [ConvertContext.CreateParameter()]);
        return parent;
    }
}
