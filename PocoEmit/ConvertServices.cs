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
        var key = new MapTypeKey(sourceType, typeof(TDest));
        var emitConverter = poco.GetEmitConverter(key);
        if (emitConverter is null)
            return null;
        if (emitConverter.Compiled && emitConverter is IPocoConverter<TSource, TDest> converter)
            return converter;
        var compiledConverter = CompileConverter<TSource, TDest>(emitConverter, Expression.Parameter(sourceType, "source"));
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
        var key = new MapTypeKey(sourceType, destType);
        var converter = poco.GetEmitConverter(key);
        if (converter is null)
            return null;
        if (converter.Compiled)
            return converter;
        var compileConverter = Inner.CompileConverterMethod.MakeGenericMethod(sourceType, destType);
        var compiled = compileConverter.Invoke(null, [converter, Expression.Parameter(sourceType, "source")]) as IEmitConverter;
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
        var key = new MapTypeKey(sourceType, typeof(TDest));
        var emitConverter = poco.GetEmitConverter(key);
        if (emitConverter is null)
            return null;
        if (emitConverter.Compiled && emitConverter is ICompiledConverter<TSource, TDest> compiled)
            return compiled.ConvertFunc;
        var convertFunc = Build<TSource, TDest>(emitConverter, Expression.Parameter(sourceType, "source"))
            .Compile();       
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
        => poco.GetEmitConverter(new MapTypeKey(sourceType, destType));
    #endregion
    /// <summary>
    /// 编译转化器
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="emit"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    private static CompiledConverter<TSource, TDest> CompileConverter<TSource, TDest>(IEmitConverter emit, ParameterExpression source)
    {
        var convertFunc = Build<TSource, TDest>(emit, source)
            .Compile();
        var compiledConverter = new CompiledConverter<TSource, TDest>(emit, convertFunc);
        return compiledConverter;
    }
    #region Build
    /// <summary>
    /// 编译转换委托
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="emit"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    private static Expression<Func<TSource, TDest>> Build<TSource, TDest>(IEmitConverter emit, ParameterExpression source)
    {
        var body = emit.Convert(source);
        return Expression.Lambda<Func<TSource, TDest>>(body, source);
    }
    /// <summary>
    /// 编译转换委托
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="emit"></param>
    /// <returns></returns>
    public static Expression<Func<TSource, TDest>> Build<TSource, TDest>(this IEmitConverter emit)
        => Build<TSource, TDest>(emit, Expression.Parameter(typeof(TSource), "source"));
    #endregion
    /// <summary>
    /// 内部延迟初始化
    /// </summary>
    class Inner
    {
        /// <summary>
        /// GetConverter
        /// </summary>
        public static readonly MethodInfo CompileConverterMethod = ReflectionHelper.GetMethod(typeof(PocoEmitServices), m => m.Name == "CompileConverter");
    }
}
