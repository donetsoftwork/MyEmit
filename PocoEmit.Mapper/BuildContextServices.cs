using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Members;
using PocoEmit.Resolves;
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
    public static ParameterExpression GetConvertContextParameter(this IBuildContext context, PairTypeKey key)
    {
        var bundle = context.GetBundle(key);
        if (bundle is null || !bundle.IsCircle)
            return null;
        return context.ConvertContextParameter;
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
        var convertContextParameter = context.ConvertContextParameter;
        if (convertContextParameter is not null)
        {
            var bundle = context.GetBundle(key);
            if(bundle is not null && bundle.HasCircle)
            {
                var sourceType = key.LeftType;
                var destType = key.RightType;
                if (PairTypeKey.CheckNullCondition(sourceType))
                {
                    return Expression.Condition(
                        Expression.Equal(source, Expression.Constant(null, sourceType)),
                        Expression.Default(destType),
                        CallContextConvert(context, key, convertContextParameter, source)
                    );
                }
                else
                {
                    return CallContextConvert(context, key, convertContextParameter, source);
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
        var achieved = context.GetAchieve(key);
        var contextLambda = achieved.Lambda;
        if (contextLambda is null)
            return ConvertContext.CallConvert(convertContext, key, Expression.Constant(achieved, typeof(IContextConverter)), source);
        else
            return Expression.Invoke(contextLambda, convertContext, source);
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
        var convertContextParameter = parent.ConvertContextParameter;
        if (convertContextParameter is null)
            return parent;
        var bundle = parent.GetBundle(key);
        if (bundle is null)
            return new CurrentContext(parent.Context, null);
        if (bundle.HasCircle)
            return new CurrentContext(parent.Context, ConvertContext.CreateParameter());
        return new CurrentContext(parent.Context, null);
    }
    /// <summary>
    /// 设置缓存
    /// </summary>
    /// <param name="context"></param>
    /// <param name="key"></param>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    internal static Expression SetCache(this IBuildContext context, PairTypeKey key, Expression source, Expression dest)
    {
        var parameter = context.ConvertContextParameter;
        if (parameter is not null)
        {
            var bundle = context.GetBundle(key);
            if (bundle is not null && bundle.IsCircle)
                return ConvertContext.CallSetCache(parameter, key, source, dest);
        }
        return null;
    }
}
