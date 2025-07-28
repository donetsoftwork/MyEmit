using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Reflection;
using System.Linq.Expressions;

namespace PocoEmit;

/// <summary>
/// 类型转换扩展方法
/// </summary>
public static partial class PocoEmitServices
{
    #region 配置
    /// <summary>
    /// 设置委托来转化
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="settings"></param>
    /// <param name="convertFunc"></param>
    /// <returns></returns>
    public static DelegateConverter<TSource, TDest> SetConvertFunc<TSource, TDest>(this ISettings<MapTypeKey, IEmitConverter> settings, Func<TSource, TDest> convertFunc)
    {
        var key = new MapTypeKey(typeof(TSource), typeof(TDest));
        var converter = new DelegateConverter<TSource, TDest>(convertFunc);
        settings.Set(key, converter);
        return converter;
    }
    /// <summary>
    /// 尝试设置委托来转化不覆盖
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="settings"></param>
    /// <param name="convertFunc"></param>
    /// <returns></returns>
    public static IEmitConverter TrySetConvertFunc<TSource, TDest>(this ISettings<MapTypeKey, IEmitConverter> settings, Func<TSource, TDest> convertFunc)
    {
        var key = new MapTypeKey(typeof(TSource), typeof(TDest));
        if (settings.TryGetValue(key, out var value0) && value0 is not null)
            return value0;
        var converter = new DelegateConverter<TSource, TDest>(convertFunc);
        settings.Set(key, converter);
        return converter;
    }
    /// <summary>
    /// 加载System.Convert
    /// </summary>
    /// <typeparam name="TSettings"></typeparam>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static TSettings SetSystemConvert<TSettings>(this TSettings settings)
        where TSettings : ISettings<MapTypeKey, IEmitConverter>
        => AddStaticConverter(settings, typeof(Convert));
    /// <summary>
    /// 加载字符串转化基础类型
    /// </summary>
    /// <param name="settings"></param>
    public static TSettings SetStringConvert<TSettings>(this TSettings settings)
        where TSettings : ISettings<MapTypeKey, IEmitConverter>
    {
        settings.TrySetConvertFunc<string, bool>(System.Convert.ToBoolean);
        settings.TrySetConvertFunc<string, byte>(System.Convert.ToByte);
        settings.TrySetConvertFunc<string, char>(System.Convert.ToChar);
        settings.TrySetConvertFunc<string, DateTime>(System.Convert.ToDateTime);
        settings.TrySetConvertFunc<string, decimal>(System.Convert.ToDecimal);
        settings.TrySetConvertFunc<string, float>(System.Convert.ToSingle);
        settings.TrySetConvertFunc<string, double>(System.Convert.ToDouble);
        settings.TrySetConvertFunc<string, short>(System.Convert.ToInt16);
        settings.TrySetConvertFunc<string, int>(System.Convert.ToInt32);
        settings.TrySetConvertFunc<string, long>(System.Convert.ToInt64);
        settings.TrySetConvertFunc<string, sbyte>(System.Convert.ToSByte);
        settings.TrySetConvertFunc<string, ushort>(System.Convert.ToUInt16);
        settings.TrySetConvertFunc<string, uint>(System.Convert.ToUInt32);
        settings.TrySetConvertFunc<string, ulong>(System.Convert.ToUInt64);
        return settings;
    }
    /// <summary>
    /// 添加静态转化
    /// </summary>
    /// <typeparam name="TConverter"></typeparam>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static void AddStaticConverter<TConverter>(this ISettings<MapTypeKey, IEmitConverter> settings)
        => AddStaticConverter(settings, typeof(TConverter));
    /// <summary>
    /// 添加静态转化
    /// </summary>
    /// <typeparam name="TSettings"></typeparam>
    /// <param name="settings"></param>
    /// <param name="converterType"></param>
    /// <returns></returns>
    public static TSettings AddStaticConverter<TSettings>(this TSettings settings, Type converterType)
        where TSettings : ISettings<MapTypeKey, IEmitConverter>
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var methods = converterType.GetTypeInfo().DeclaredMethods;
#else
        var methods = converterType.GetMethods();
#endif
        foreach (var method in methods)
        {
            var returnType = method.ReturnType;
            var parameters = method.GetParameters();
            if (method.DeclaringType == converterType && method.IsStatic && returnType != typeof(void) && parameters.Length == 1)
            {
                StaticMethodConverter converter = new(method);
                MapTypeKey key = new(parameters[0].ParameterType, returnType);
                settings.Set(key, converter);
            }
        }
        return settings;
    }
    /// <summary>
    /// 添加实例转化
    /// </summary>
    /// <typeparam name="TSettings"></typeparam>
    /// <param name="settings"></param>
    /// <param name="instance"></param>
    /// <returns></returns>
    public static TSettings AddConverter<TSettings>(this TSettings settings, object instance)
        where TSettings : ISettings<MapTypeKey, IEmitConverter>
    {
        Type converterType = instance.GetType();
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var methods = converterType.GetTypeInfo().DeclaredMethods;
#else
        var methods = converterType.GetMethods();
#endif
        foreach (var method in methods)
        {
            var returnType = method.ReturnType;
            var parameters = method.GetParameters();
            if (method.DeclaringType == converterType && !method.IsStatic && returnType != typeof(void) && parameters.Length == 1)
            {
                InstanceMethodConverter converter = new(instance, method);
                MapTypeKey key = new(parameters[0].ParameterType, returnType);
                settings.Set(key, converter);
            }
        }
        return settings;
    }
    #endregion
    #region GetConverter
    /// <summary>
    /// 获取类型转化
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IPocoConverter<TSource, TDest> GetConverter<TSource, TDest>(this IPocoOptions options)
    {
        var sourceType = typeof(TSource);
        var key = new MapTypeKey(sourceType, typeof(TDest));
        var emitConverter = options.ConverterFactory.Get(key);
        if (emitConverter is null)
            return null;
        if (emitConverter.Compiled && emitConverter is IPocoConverter<TSource, TDest> converter)
            return converter;
        var compiledConverter = CompileConverter<TSource, TDest>(emitConverter, Expression.Parameter(sourceType, "source"));
        options.ConverterFactory.Set(key, compiledConverter);
        return compiledConverter;
    }
    #endregion
    #region GetGenericConverter
    /// <summary>
    /// 获取类型转化(用于IOC注入)
    /// </summary>
    /// <param name="options"></param>
    /// <param name="converterType"></param>
    /// <returns></returns>
    public static object GetGenericConverter(this IPocoOptions options, Type converterType)
    {
        if (!ReflectionHelper.IsGenericTypeDefinition(converterType, typeof(IPocoConverter<,>)))
            return null;
#if (NETSTANDARD1_1 || NETSTANDARD1_3)
        var argumentsType = converterType.GetTypeInfo().GenericTypeParameters;
#else
        var argumentsType = converterType.GetGenericArguments();
#endif
        var sourceType = argumentsType[0];
        var key = new MapTypeKey(sourceType, argumentsType[1]);
        var converter = options.ConverterFactory.Get(key);
        if (converter is null)
            return null;
        if (converter.Compiled)
            return converter;
        var compileConverter = Inner.CompileConverterMethod.MakeGenericMethod(argumentsType);
        var compiled = compileConverter.Invoke(null, [converter, Expression.Parameter(sourceType, "source")]) as IEmitConverter;
        if (compiled != null && compiled.Compiled)
            options.ConverterFactory.Set(key, compiled);
        return compiled;
    }
    #endregion
    #region GetConvertFunc
    /// <summary>
    /// 获取转换委托
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="options"></param>
    /// <returns></returns>
    public static Func<TSource, TDest> GetConvertFunc<TSource, TDest>(this IPocoOptions options)
    {
        var sourceType = typeof(TSource);
        var key = new MapTypeKey(sourceType, typeof(TDest));
        var emitConverter = options.ConverterFactory.Get(key);
        if (emitConverter is null)
            return null;
        if (emitConverter.Compiled && emitConverter is ICompiledConverter<TSource, TDest> compiled)
            return compiled.ConvertFunc;
        var convertFunc = Compile<TSource, TDest>(emitConverter, Expression.Parameter(sourceType, "source"));       
        var compiledConverter = new CompiledConverter<TSource, TDest>(emitConverter, convertFunc);
        options.ConverterFactory.Set(key, compiledConverter);
        return convertFunc;
    }
    #endregion
    #region Convert
    /// <summary>
    /// 转换类型
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="options"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static TDest Convert<TSource, TDest>(this IPocoOptions options, TSource source)
    {
        var convertFunc = options.GetConvertFunc<TSource, TDest>()
            ?? throw new InvalidOperationException($"不支持转换的类型：{typeof(TSource).FullName} -> {typeof(TDest).FullName}");
        return convertFunc(source);
    }
    #endregion
    /// <summary>
    /// 编译转化器
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="emit"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static CompiledConverter<TSource, TDest> CompileConverter<TSource, TDest>(this IEmitConverter emit, ParameterExpression source)
    {
        var convertFunc = Compile<TSource, TDest>(emit, source);
        var compiledConverter = new CompiledConverter<TSource, TDest>(emit, convertFunc);
        return compiledConverter;
    }
    #region Compile
    /// <summary>
    /// 编译转换委托
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="emit"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static Func<TSource, TDest> Compile<TSource, TDest>(this IEmitConverter emit, ParameterExpression source)
    {
        var body = emit.Convert(source);
        var lambda = Expression.Lambda<Func<TSource, TDest>>(body, source);
        return lambda.Compile();
    }
    /// <summary>
    /// 编译转换委托
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="emit"></param>
    /// <returns></returns>
    public static Func<TSource, TDest> Compile<TSource, TDest>(this IEmitConverter emit)
        => Compile<TSource, TDest>(emit, Expression.Parameter(typeof(TSource), "source"));
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
