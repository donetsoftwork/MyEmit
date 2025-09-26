using PocoEmit.Builders;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Members;

namespace PocoEmit;

/// <summary>
/// 预览服务
/// </summary>
public static partial class MapperServices
{
    /// <summary>
    /// 预览检测
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="converter"></param>
    public static void Visit(this IComplexBundle parent, IEmitConverter converter)
    {
        var key = converter.Key;
        if (converter.Compiled)
        {
            if(converter is IWrapper<IEmitConverter> wrapper)
            {
                Visit(parent, wrapper.Original);
            }
            else
            {
                // 不再解析
                parent.Accept(key, converter, false);
            }      
        }
        else if(converter is IComplexPreview preview)
        {
            preview.Preview(parent);
        }
        else if(converter is FuncConverter)
        {
            // 已解析表达式,不再解析
            parent.Accept(key, converter, false);
        }
        else if (converter is IWrapper<IEmitConverter> wrapper)
        {
            Visit(parent, wrapper.Original);
        }
    }
    /// <summary>
    /// 预览检测
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="reader"></param>
    public static void Visit(this IComplexBundle parent, IEmitReader reader)
    {
        if (reader is ConvertInstanceReader instanceReader)
        {
            Visit(parent, instanceReader.Converter);
            Visit(parent, instanceReader.Original);
        }
        else if (reader is ConvertValueReader valueReader)
        {
            Visit(parent, valueReader.Converter);
            Visit(parent, valueReader.Original);
        }
        else if (reader is IWrapper<IEmitReader> emitReader)
        {
            Visit(parent.Context, emitReader.Original);
        }
        else if (reader is IWrapper<IEmitMemberReader> memberReader)
        {
            Visit(parent, memberReader.Original);
        }
    }
    /// <summary>
    /// 预览检测
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static void Visit(this IComplexBundle parent, in PairTypeKey key)
    {
        var converter = parent.GetConverter(key);
        if (converter is null)
            return;
        Visit(parent, converter);
    }
    /// <summary>
    /// 预览检测
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static void Visit<TSource, TDest>(this IComplexBundle parent)
        => Visit(parent, new PairTypeKey(typeof(TSource), typeof(TDest)));
}
