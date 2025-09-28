using PocoEmit.Builders;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Members;
using PocoEmit.Resolves;
using PocoEmit.Visitors;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit;

/// <summary>
/// 上下文扩展方法
/// </summary>
public static partial class MapperServices
{
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
        if (converter is IEmitComplexConverter complexConverter)
            return CallComplexConvert(context, complexConverter.Key, source);
        return converter.Convert(source);
    }
    /// <summary>
    /// 调用复杂类型转化
    /// </summary>
    /// <param name="context"></param>
    /// <param name="key"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    internal static Expression CallComplexConvert(this IBuildContext context, in PairTypeKey key, Expression source)
    {
        var destType = key.RightType;
        var convertContext = context.ConvertContextParameter;
        var dest = Expression.Variable(destType, "dest");
        var bundle = context.GetBundle(key);
        var isCache = context.HasCache;
        if (convertContext is null)
            isCache = false;
        else if (bundle.HasCache)
            isCache = true;
        
        if (isCache)
        {
            var contexAchieve = context.GetContexAchieve(key);
            if (contexAchieve is not null)
            {
                var contextLambda = contexAchieve.Build();
                var test = ConvertContext.CallTryGetCache(convertContext, key, source, dest);
                if (contextLambda is null)
                {
                    var converter = Expression.Constant(contexAchieve, EmitMapperHelper.GetContextConvertType(key));
                    var convert = EmitMapperHelper.CallContextConvert(converter, convertContext, source);
                    return Expression.Block(
                        [dest],
                        Expression.Condition(test, dest, convert, destType));
                }
                else
                {
                    return Expression.Block([dest], Expression.Condition(test, dest, context.Call(bundle.IsCircle, contextLambda, convertContext, source), destType));
                }
            }
        }

        var achieved = context.GetAchieve(key);
        var lambda = achieved.Build();
        if (lambda is null)
        {
            var converter = Expression.Constant(achieved, EmitMapperHelper.GetCompiledConvertType(key));
            return EmitMapperHelper.CallConvert(converter, source);
        }
        else
        {
            return Expression.Block([dest], context.Call(bundle.IsCircle, lambda, source));
        }
    }
    /// <summary>
    /// 切换进入新上下文
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    internal static IBuildContext Enter(this IBuildContext parent, in PairTypeKey key)
        => parent.Enter(parent.GetBundle(key));
    /// <summary>
    /// 切换进入新上下文
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="bundle"></param>
    /// <returns></returns>
    internal static IBuildContext Enter(this IBuildContext parent, ComplexBundle bundle)
    {
        var isCache = parent.HasCache;
        if (bundle is not null)
            isCache |= bundle.HasCache;
        var parameter = isCache || parent.ConvertContextParameter is not null ? ConvertContext.CreateParameter() : null;
        return new CurrentContext(parent.Context, bundle, parameter);
    }
    /// <summary>
    /// 构建上下文转化
    /// </summary>
    /// <param name="context"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    internal static LambdaExpression BuildWithContext(this BuildContext context, IEmitComplexConverter converter)
    {
        var key = converter.Key;
        var contexAchieve = context.GetContexAchieve(key);
        var contextLambda = contexAchieve?.Build();
        if (contextLambda is not null)
            return contextLambda;
        var bundle = context.GetBundle(key);
        var currentContext = context.Enter(bundle);
        var sourceType = key.LeftType;
        var destType = key.RightType;
        var source = Expression.Variable(sourceType, "source");
        var dest = Expression.Variable(destType, "dest");
        var parameter = currentContext.ConvertContextParameter;
        List<Expression> list = [];
        list.AddRange(converter.BuildBody(currentContext, source, dest, parameter));
        var funcType = Expression.GetFuncType(parameter.Type, sourceType, destType);
        var originalBody = converter.BuildBody(currentContext, source, dest, parameter);
        Expression body;
        if (PairTypeKey.CheckNullCondition(sourceType))
        {
            body = Expression.Block(
                [dest],
                Expression.IfThen(
                    Expression.NotEqual(source, Expression.Constant(null, sourceType)),
                    CleanVisitor.Clean(Expression.Block(originalBody))
                ),
                dest);
        }
        else
        {
            body = Expression.Block(
                [dest],
                [
                    ..CleanVisitor.Clean(originalBody),
                    dest
                ]
            );
        }
        contextLambda = Expression.Lambda(funcType, body, [parameter, source]);
        contexAchieve.CompileDelegate(contextLambda);
        return contextLambda;
    }
    /// <summary>
    /// 构建转化
    /// </summary>
    /// <param name="context"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    internal static LambdaExpression Build(this BuildContext context, IEmitComplexConverter converter)
    {
        var key = converter.Key;
        var achieved = context.GetAchieve(key);
        var lambda = achieved?.Build();
        if (lambda is not null)
            return lambda;

        var sourceType = key.LeftType;
        var destType = key.RightType;
        var source = Expression.Variable(sourceType, "source");
        var dest = Expression.Variable(destType, "dest");

        var bundle = context.GetBundle(key);
        var parameters = new List<ParameterExpression>() { dest };
        var expressions = new List<Expression>();
        if (bundle is null)
        {
            expressions.AddRange(CleanVisitor.Clean(converter.BuildBody(context, source, dest, null)));
        }
        else if (bundle.HasCache)
        {
            var contextLambda = BuildWithContext(context, converter);

            var parameter = ConvertContext.CreateParameter();
            parameters.Add(parameter);
            expressions.Add(context.InitContext(parameter));
            expressions.Add(CleanVisitor.Clean(Expression.Assign(dest, context.Call(bundle.IsCircle, contextLambda, parameter, source))));
            expressions.Add(EmitDispose.Dispose(parameter));
        }
        else
        {

            var currentContext = context.Enter(bundle);
            expressions.AddRange(CleanVisitor.Clean(converter.BuildBody(currentContext, source, dest, null)));
        }
        var funcType = Expression.GetFuncType(sourceType, destType);
        Expression body;
        if (PairTypeKey.CheckNullCondition(sourceType))
        {
            body = Expression.Block(
                parameters,
                Expression.IfThen(
                    Expression.NotEqual(source, Expression.Constant(null, sourceType)),
                    CleanVisitor.Clean(Expression.Block(expressions))
                ),
                dest);
        }
        else
        {
            body = Expression.Block(
                parameters,
                [
                    ..CleanVisitor.Clean(expressions),
                    dest
                ]
            );
        }
        lambda = Expression.Lambda(funcType, body, [source]);
        achieved?.CompileDelegate(lambda);
        return lambda;
    }
    /// <summary>
    /// 设置缓存
    /// </summary>
    /// <param name="context"></param>
    /// <param name="parameter"></param>
    /// <param name="key"></param>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    internal static Expression SetCache(this IBuildContext context, ParameterExpression parameter, in PairTypeKey key, Expression source, Expression dest)
    {
        if (parameter is null)
            return null;
        var bundle = context.GetBundle(key);
        if (bundle is not null && bundle.IsCache)
            return ConvertContext.CallSetCache(parameter, key, source, dest);
        return null;
    }
}
