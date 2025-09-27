using PocoEmit.Builders;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Resolves;
using System;

namespace PocoEmit;

/// <summary>
/// 上下文转化扩展方法
/// </summary>
public static partial class MapperServices
{
    #region GetEmitContextConverter
    /// <summary>
    /// 获取Emit类型转化
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static IEmitContextConverter GetEmitContextConverter<TSource, TDest>(this IMapper mapper)
        => GetEmitContextConverter(mapper, typeof(TSource), typeof(TDest));
    /// <summary>
    /// 获取Emit类型转化
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public static IEmitContextConverter GetEmitContextConverter(this IMapper mapper, Type sourceType, Type destType)
    {
        var key = new PairTypeKey(sourceType, destType);
        var options = (IMapperOptions)mapper;
        if (options.TryGetValue(key, out IEmitContextConverter contextConverter))
            return contextConverter;
        if(options.CheckPrimitive(sourceType))
            return null;
        var converter = options.GetEmitConverter(key);
        if(converter is null) 
            return null;
        if (converter.Compiled && converter is IWrapper<IEmitConverter> wrapper)
            converter = wrapper.Original;
        if (converter is not IEmitComplexConverter complexConverter)
            return null;
        var context = new BuildContext(options, ComplexCached.Always);
        complexConverter.Preview(context);
        var bundle = context.GetBundle(key);
        bundle.EnableCache();
        context.Prepare();
        context.BuildContextConvert(bundle);
        return context.GetContexAchieve(key);
    }
    #endregion
    #region GetConvertFunc
    /// <summary>
    /// 获取转换委托
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static Func<IConvertContext, TSource, TDest> GetContextConvertFunc<TSource, TDest>(this IMapper mapper)
    {
        if( mapper.GetEmitContextConverter<TSource, TDest>() is ContextConverter<TSource, TDest> contextConverter)
            return contextConverter.ConvertFunc;
        return null;
    }
    #endregion
}
