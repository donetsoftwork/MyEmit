using PocoEmit.Builders;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
using System.Reflection;
#endif

namespace PocoEmit;

/// <summary>
/// 类型转换配置扩展方法
/// </summary>
public static partial class PocoEmitServices
{
    /// <summary>
    /// 设置委托来转化
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="poco"></param>
    /// <param name="convertFunc"></param>
    /// <returns></returns>
    public static IPoco UseConvertFunc<TSource, TDest>(this IPoco poco, Func<TSource, TDest> convertFunc)
    {
        var key = new PairTypeKey(typeof(TSource), typeof(TDest));
        poco.Configure(key, new DelegateConverter<TSource, TDest>(convertFunc));
        return poco;
    }
    /// <summary>
    /// 添加静态转化方法
    /// </summary>
    /// <typeparam name="TConverter"></typeparam>
    /// <param name="poco"></param>
    /// <returns></returns>
    public static IPoco UseStaticConverter<TConverter>(this IPoco poco)
        => UseStaticConverter(poco, typeof(TConverter));
    /// <summary>
    /// 添加静态转化方法
    /// </summary>
    /// <typeparam name="TConfiguration"></typeparam>
    /// <param name="configuration"></param>
    /// <param name="converterType"></param>
    /// <returns></returns>
    public static TConfiguration UseStaticConverter<TConfiguration>(this TConfiguration configuration, Type converterType)
        where TConfiguration : IPoco
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
                MethodConverter converter = new(null, method);
                PairTypeKey key = new(parameters[0].ParameterType, returnType);
                configuration.Configure(key, converter);
            }
        }
        return configuration;
    }
    /// <summary>
    /// 添加实例转化
    /// </summary>
    /// <typeparam name="TConfiguration"></typeparam>
    /// <param name="configuration"></param>
    /// <param name="instance"></param>
    /// <returns></returns>
    public static TConfiguration UseConverter<TConfiguration>(this TConfiguration configuration, object instance)
        where TConfiguration : IPoco
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
                MethodConverter converter = new(EmitHelper.CheckMethodCallInstance(instance), method);
                PairTypeKey key = new(parameters[0].ParameterType, returnType);
                configuration.Configure(key, converter);
            }
        }
        return configuration;
    }
    /// <summary>
    /// 加载System.Convert
    /// </summary>
    /// <typeparam name="TConfiguration"></typeparam>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static TConfiguration UseSystemConvert<TConfiguration>(this TConfiguration configuration)
        where TConfiguration : IPoco
        => UseStaticConverter(configuration, typeof(Convert));
    /// <summary>
    /// 加载字符串转化基础类型
    /// </summary>
    /// <param name="configuration"></param>
    public static TConfiguration SetStringConvert<TConfiguration>(this TConfiguration configuration)
        where TConfiguration : IPoco
    {
        configuration.UseConvertFunc<string, bool>(System.Convert.ToBoolean);
        configuration.UseConvertFunc<string, byte>(System.Convert.ToByte);
        configuration.UseConvertFunc<string, char>(System.Convert.ToChar);
        configuration.UseConvertFunc<string, DateTime>(System.Convert.ToDateTime);
        configuration.UseConvertFunc<string, decimal>(System.Convert.ToDecimal);
        configuration.UseConvertFunc<string, float>(System.Convert.ToSingle);
        configuration.UseConvertFunc<string, double>(System.Convert.ToDouble);
        configuration.UseConvertFunc<string, short>(System.Convert.ToInt16);
        configuration.UseConvertFunc<string, int>(System.Convert.ToInt32);
        configuration.UseConvertFunc<string, long>(System.Convert.ToInt64);
        configuration.UseConvertFunc<string, sbyte>(System.Convert.ToSByte);
        configuration.UseConvertFunc<string, ushort>(System.Convert.ToUInt16);
        configuration.UseConvertFunc<string, uint>(System.Convert.ToUInt32);
        configuration.UseConvertFunc<string, ulong>(System.Convert.ToUInt64);
        return configuration;
    }
}
