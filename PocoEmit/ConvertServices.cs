using PocoEmit.Builders;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit;

/// <summary>
/// 类型转换扩展方法
/// </summary>
public static partial class PocoEmitServices
{    
    #region GetConverter
    /// <summary>
    /// 获取类型转化
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="poco"></param>
    /// <returns></returns>
    public static IPocoConverter<TSource, TDest> GetConverter<TSource, TDest>(this IPoco poco)
    {
        var sourceType = typeof(TSource);
        var key = new PairTypeKey(sourceType, typeof(TDest));
        var emitConverter = poco.GetEmitConverter(key);
        if (emitConverter is null)
            return null;
        if (emitConverter.Compiled && emitConverter is IPocoConverter<TSource, TDest> converter)
            return converter;
        var compiledConverter = CompileConverter<TSource, TDest>((IPocoOptions)poco, key, emitConverter);
        poco.Set(key, compiledConverter);
        return compiledConverter;
    }
    #endregion
    #region GetObjectConverter
    /// <summary>
    /// 获取弱类型转化(IObjectConverter)
    /// </summary>
    /// <param name="poco"></param>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public static object GetObjectConverter(this IPoco poco, Type sourceType, Type destType)
        => GetObjectConverter(poco, new PairTypeKey(sourceType, destType));
    /// <summary>
    /// 获取弱类型转化(IObjectConverter)
    /// </summary>
    /// <param name="poco"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static object GetObjectConverter(this IPoco poco, in PairTypeKey key)
    {
        var converter = poco.GetEmitConverter(key);
        if (converter is null)
            return null;
        if (converter.Compiled)
            return converter;
        var compiled = Inner.Compile((IPocoOptions)poco, key, converter) as IEmitConverter;
        if (compiled != null)
            poco.Set(key, compiled);
        return compiled;
    }
    #endregion
    #region GetConvertFunc
    /// <summary>
    /// 获取转换委托
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="poco"></param>
    /// <returns></returns>
    public static Func<TSource, TDest> GetConvertFunc<TSource, TDest>(this IPoco poco)
    {
        var sourceType = typeof(TSource);
        var key = new PairTypeKey(sourceType, typeof(TDest));
        var emitConverter = poco.GetEmitConverter(key);
        if (emitConverter is null)
            return null;
        if (emitConverter.Compiled && emitConverter is CompiledConverter<TSource, TDest> compiled)
            return compiled.ConvertFunc;
        compiled = CompileConverter<TSource, TDest>((IPocoOptions)poco, key, emitConverter);
        poco.Set(key, compiled);
        return compiled.ConvertFunc;
    }
    #endregion
    #region Convert
    /// <summary>
    /// 转换类型
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="poco"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static TDest Convert<TSource, TDest>(this IPoco poco, TSource source)
    {
        var convertFunc = poco.GetConvertFunc<TSource, TDest>()
            ?? throw new InvalidOperationException($"不支持转换的类型：{typeof(TSource).FullName} -> {typeof(TDest).FullName}");
        return convertFunc(source);
    }
    #endregion
    #region GetEmitConverter
    /// <summary>
    /// 获取Emit类型转化
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="poco"></param>
    /// <returns></returns>
    public static IEmitConverter GetEmitConverter<TSource, TDest>(this IPoco poco)
        => GetEmitConverter(poco, typeof(TSource), typeof(TDest));
    /// <summary>
    /// 获取Emit类型转化
    /// </summary>
    /// <param name="poco"></param>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public static IEmitConverter GetEmitConverter(this IPoco poco, Type sourceType, Type destType)
        => poco.GetEmitConverter(new PairTypeKey(sourceType, destType));
    #endregion
    #region Build
    /// <summary>
    /// 编译转换委托
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="poco"></param>
    /// <returns></returns>
    public static Expression<Func<TSource, TDest>> BuildConverter<TSource, TDest>(this IPoco poco)
    {
        return poco.GetEmitConverter<TSource, TDest>()
            .Build<TSource, TDest>();
    }
    /// <summary>
    /// 编译转换委托
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="emit"></param>
    /// <returns></returns>
    public static Expression<Func<TSource, TDest>> Build<TSource, TDest>(this IEmitConverter emit)
    {
        if(emit is IBuilder<LambdaExpression> builder)
            return builder.Build() as Expression<Func<TSource, TDest>>;
        var source = Expression.Parameter(typeof(TSource), "source");
        var body = emit.Convert(source);
        return Expression.Lambda<Func<TSource, TDest>>(body, source);
    }
    /// <summary>
    /// 编译类型转化
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="converter"></param>
    /// <returns></returns>
    public static Func<TSource, TDest> CompileFunc<TSource, TDest>(this IEmitConverter converter)
        => Compiler._instance.CompileDelegate(converter.Build<TSource, TDest>());
    /// <summary>
    /// 编译
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="poco"></param>
    /// <param name="key"></param>
    /// <param name="emitConverter"></param>
    /// <returns></returns>
    internal static CompiledConverter<TSource, TDest> CompileConverter<TSource, TDest>(this IPocoOptions poco,in PairTypeKey key, IEmitConverter emitConverter)
    {
        var lambda = emitConverter.Build<TSource, TDest>();
        var func = Compiler._instance.CompileDelegate(lambda);
        return new(poco, key, emitConverter, lambda, func);
    }
    #endregion
    /// <summary>
    /// 内部延迟初始化
    /// </summary>
    class Inner
    {
        /// <summary>
        /// 反射Compile方法
        /// </summary>
        private static readonly MethodInfo ConvertCompilerMethod = EmitHelper.GetActionMethodInfo<IPocoOptions>(poco => CompileConverter<long, object>(poco, new PairTypeKey(null, null), null))
            .GetGenericMethodDefinition();
        /// <summary>
        /// 反射调用编译方法
        /// </summary>
        /// <param name="poco"></param>
        /// <param name="key"></param>
        /// <param name="emitConverter"></param>
        /// <returns></returns>
        internal static object Compile(IPocoOptions poco, in PairTypeKey key, IEmitConverter emitConverter)
        {
            return ConvertCompilerMethod.MakeGenericMethod(key.LeftType, key.RightType)
                .Invoke(null, [poco, key, emitConverter]);
        }
    }
}
