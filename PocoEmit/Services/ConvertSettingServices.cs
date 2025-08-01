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
    #region 配置
    /// <summary>
    /// 设置委托来转化
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="settings"></param>
    /// <param name="convertFunc"></param>
    /// <returns></returns>
    public static DelegateConverter<TSource, TDest> SetConvertFunc<TSource, TDest>(this IPocoOptions settings, Func<TSource, TDest> convertFunc)
    {
        var key = new MapTypeKey(typeof(TSource), typeof(TDest));
        var converter = new DelegateConverter<TSource, TDest>(convertFunc);
        settings.SetConvertSetting(key, converter);
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
    public static IEmitConverter TrySetConvertFunc<TSource, TDest>(this IPocoOptions settings, Func<TSource, TDest> convertFunc)
    {
        var key = new MapTypeKey(typeof(TSource), typeof(TDest));
        if (settings.TryGetValue(key, out var value0) && value0 is not null)
            return value0;
        var converter = new DelegateConverter<TSource, TDest>(convertFunc);
        settings.SetConvertSetting(key, converter);
        return converter;
    }
    /// <summary>
    /// 加载System.Convert
    /// </summary>
    /// <typeparam name="TSettings"></typeparam>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static TSettings SetSystemConvert<TSettings>(this TSettings settings)
        where TSettings : IPocoOptions
        => AddStaticConverter(settings, typeof(Convert));
    /// <summary>
    /// 加载字符串转化基础类型
    /// </summary>
    /// <param name="settings"></param>
    public static TSettings SetStringConvert<TSettings>(this TSettings settings)
        where TSettings : IPocoOptions
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
    public static void AddStaticConverter<TConverter>(this IPocoOptions settings)
        => AddStaticConverter(settings, typeof(TConverter));
    /// <summary>
    /// 添加静态转化
    /// </summary>
    /// <typeparam name="TSettings"></typeparam>
    /// <param name="settings"></param>
    /// <param name="converterType"></param>
    /// <returns></returns>
    public static TSettings AddStaticConverter<TSettings>(this TSettings settings, Type converterType)
        where TSettings : IPocoOptions
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
                settings.SetConvertSetting(key, converter);
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
        where TSettings : IPocoOptions
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
                settings.SetConvertSetting(key, converter);
            }
        }
        return settings;
    }
    #endregion
}
