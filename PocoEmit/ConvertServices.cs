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
        var compiledConverter = Compile<TSource, TDest>(emitConverter);
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
    {
        var key = new PairTypeKey(sourceType, destType);
        var converter = poco.GetEmitConverter(key);
        if (converter is null)
            return null;
        if (converter.Compiled)
            return converter;
        var compiled = Inner.Compile(sourceType, destType, converter) as IEmitConverter;
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
        if (emitConverter.Compiled && emitConverter is ICompiledConverter<TSource, TDest> compiled)
            return compiled.ConvertFunc;
        var convertFunc = emitConverter.CompileFunc<TSource, TDest>();
        var compiledConverter = new CompiledConverter<TSource, TDest>(emitConverter, convertFunc);
        poco.Set(key, compiledConverter);
        return convertFunc;
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
        if(emit is IEmitBuilder builder)
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
        => Compiler._instance.CompileFunc(converter.Build<TSource, TDest>());
    /// <summary>
    /// 编译
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="emitConverter"></param>
    /// <returns></returns>
    internal static CompiledConverter<TSource, TDest> Compile<TSource, TDest>(this IEmitConverter emitConverter)
        => new(emitConverter, emitConverter.CompileFunc<TSource, TDest>());
    #endregion
    /// <summary>
    /// 内部延迟初始化
    /// </summary>
    class Inner
    {

        /// <summary>
        /// 反射Compile方法
        /// </summary>
        private static readonly MethodInfo ConvertCompilerMethod = EmitHelper.GetActionMethodInfo<IEmitConverter>(poco => Compile<long, object>(poco))
            .GetGenericMethodDefinition();
        /// <summary>
        /// 反射调用编译方法
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="destType"></param>
        /// <param name="emit"></param>
        /// <returns></returns>
        internal static object Compile(Type sourceType, Type destType, IEmitConverter emit)
        {
            return ConvertCompilerMethod.MakeGenericMethod(sourceType, destType)
                .Invoke(null, [emit]);
        }
    }
}
