using Hand.Reflection;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Resolves;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// Emit工具
/// </summary>
public static class EmitMapperHelper
{
    #region CompiledConvert
    /// <summary>
    /// 构造Emit上下文转化
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="key"></param>
    /// <param name="original"></param>
    /// <returns></returns>
    public static ICompiledConverter CreateCompiledConverter(IMapperOptions mapper, in PairTypeKey key, IEmitConverter original)
        => Activator.CreateInstance(GetCompiledConvertType(key), mapper, key, original) as ICompiledConverter;
    /// <summary>
    /// 调用转化方法
    /// </summary>
    /// <param name="converter"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static Expression CallConvert(Expression converter, Expression source)
    {
        var contextConvertType = converter.Type;
        var method = ReflectionMember.GetMethod(contextConvertType, "Convert", [source.Type]);
        return Expression.Call(converter, method, source);
    }
    /// <summary>
    /// 获取上下文转化类型
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Type GetCompiledConvertType(in PairTypeKey key)
        => _compiledConvertGenericType.MakeGenericType(key.LeftType, key.RightType);
    #endregion
    #region ContextConvert
    /// <summary>
    /// 构造Emit上下文转化
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static IEmitContextConverter CreateContextConverter(IMapperOptions mapper, in PairTypeKey key)
        => Activator.CreateInstance(GetContextConvertType(key), mapper, key) as IEmitContextConverter;
    /// <summary>
    /// 获取上下文转化类型
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Type GetContextConvertType(in PairTypeKey key)
        => _contextConvertGenericType.MakeGenericType(key.LeftType, key.RightType);
    /// <summary>
    /// 调用上下文转化方法
    /// </summary>
    /// <param name="converter"></param>
    /// <param name="context"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static Expression CallContextConvert(Expression converter, ParameterExpression context, Expression source)
    {
        var contextConvertType = converter.Type;
        var method = ReflectionMember.GetMethod(contextConvertType, "Convert", [typeof(IConvertContext), source.Type]);
        return Expression.Call(converter, method, context, source);
    }
    #endregion
    #region 反射
    private static Type _compiledConvertGenericType = typeof(CompiledConverter<,>);
    private static Type _contextConvertGenericType = typeof(ContextConverter<,>);
    #endregion
}
