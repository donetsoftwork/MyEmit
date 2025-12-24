using Hand.Reflection;
using PocoEmit.Builders;
using PocoEmit.Complexes;
using PocoEmit.Converters;
using PocoEmit.Members;
using PocoEmit.Resolves;
using PocoEmit.Visitors;
using System;
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
    /// <param name="builder"></param>
    /// <param name="reader"></param>
    /// <param name="instance"></param>
    /// <returns></returns>
    internal static Expression Read(this IBuildContext context, IEmitBuilder builder, IEmitReader reader, Expression instance)
    {
        if (reader is ConvertInstanceReader instanceReader)
            return Read(context, builder, instanceReader.Original, Convert(context, builder, instanceReader.Converter, instance));
        else if (reader is ConvertValueReader valueReader)
            return Convert(context, builder, valueReader.Converter, Read(context, builder, valueReader.Original, instance));
        return builder.Execute(reader, instance);
    }
    /// <summary>
    /// 转化检查复杂类型
    /// </summary>
    /// <param name="context"></param>
    /// <param name="builder"></param>
    /// <param name="converter"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    internal static Expression Convert(this IBuildContext context, IEmitBuilder builder, IEmitConverter converter, Expression source)
    {
        if (converter is IEmitComplexConverter complexConverter)
            return CallComplexConvert(context, builder, complexConverter.Key, source);
        return builder.Execute(converter, source);
    }
    /// <summary>
    /// 调用复杂类型转化
    /// </summary>
    /// <param name="context"></param>
    /// <param name="builder"></param>
    /// <param name="key"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    internal static Expression CallComplexConvert(this IBuildContext context, IEmitBuilder builder, in PairTypeKey key, Expression source)
    {
        var destType = key.RightType;
        var convertContext = context.ConvertContextParameter;        
        var bundle = context.GetBundle(key);
        var isCache = context.HasCache;
        if (convertContext is null)
            isCache = false;
        else if (bundle.HasCache)
            isCache = true;
        
        if (isCache)
        {
            var dest = builder.Declare(destType, "dest");
            var contexAchieve = context.GetContexAchieve(key);
            if (contexAchieve is not null)
            {
                var contextLambda = contexAchieve.Create();
                var noCache = Expression.Equal(ConvertContext.CallTryGetCache(convertContext, key, source, dest), Expression.Constant(false));
                
                if (contextLambda is null)
                {
                    var converter = Expression.Constant(contexAchieve, EmitMapperHelper.GetContextConvertType(key));
                    var convert = EmitMapperHelper.CallContextConvert(converter, convertContext, source);
                    builder.IfThen(noCache, CleanVisitor.Clean(Expression.Assign(dest, convert)));
                }
                else
                {
                    builder.IfThen(noCache, CleanVisitor.Clean(Expression.Assign(dest, context.Call(bundle.IsCircle, contextLambda, convertContext, source))));                    
                }
                return dest;
            }
        }

        var achieved = context.GetAchieve(key);
        var lambda = achieved.Create();
        if (lambda is null)
        {
            var converter = Expression.Constant(achieved, EmitMapperHelper.GetCompiledConvertType(key));
            return EmitMapperHelper.CallConvert(converter, source);
        }
        else
        {
            return context.Call(bundle.IsCircle, lambda, source);
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
        var contextLambda = contexAchieve?.Create();
        if (contextLambda is not null)
            return contextLambda;
        var bundle = context.GetBundle(key);
        var currentContext = context.Enter(bundle);
        var sourceType = key.LeftType;
        var destType = key.RightType;
        var source = Expression.Variable(sourceType, "source");
        var parameter = currentContext.ConvertContextParameter;
        ParameterExpression[] parameters = [parameter, source];
        var builder = new ArgumentBuilder(source);
        var result = converter.BuildFunc(currentContext, builder, source, parameter);
        var body = builder.CreateFunc(result, parameters);
        var funcType = Expression.GetFuncType(parameter.Type, sourceType, destType);
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
        var lambda = achieved?.Create();
        if (lambda is not null)
            return lambda;

        var sourceType = key.LeftType;
        var destType = key.RightType;
        var source = Expression.Variable(sourceType, "source");        

        var bundle = context.GetBundle(key);
        var builder = new ArgumentBuilder(source);

        ParameterExpression[] parameters = [source];
        Expression result;
        if (bundle is null)
        {
            result = converter.BuildFunc(context, builder, source, null);
        }
        else if (bundle.HasCache)
        {
            var dest = builder.Declare(destType, "dest");

            var contextLambda = BuildWithContext(context, converter);

            var parameter = ConvertContext.CreateParameter();
            builder.AddVariable(parameter);
            builder.Add(context.InitContext(parameter));
            builder.Add(CleanVisitor.Clean(Expression.Assign(dest, context.Call(bundle.IsCircle, contextLambda, parameter, source))));
            //builder.Assign(dest, context.Call(bundle.IsCircle, contextLambda, parameter, source));
            builder.Add(EmitDispose.Dispose(parameter));
            result = dest;
        }
        else
        {
            var currentContext = context.Enter(bundle);
            //expressions.AddRange(CleanVisitor.Clean(converter.BuildBody(currentContext, source, dest, null)));
            result = converter.BuildFunc(currentContext, builder, source, null);
        }
        
        var body = builder.CreateFunc(result, parameters);
        var funcType = Expression.GetFuncType(sourceType, destType);
        lambda = Expression.Lambda(funcType, body, [source]);
        if (lambda is null)
            throw new NullReferenceException(nameof(lambda));
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
    internal static Expression SetCache(this IBuildContext context, Expression parameter, in PairTypeKey key, Expression source, Expression dest)
    {
        if (parameter is null)
            return null;
        var bundle = context.GetBundle(key);
        if (bundle is not null && bundle.IsCache)
            return ConvertContext.CallSetCache(parameter, key, source, dest);
        return null;
    }
}
