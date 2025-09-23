using PocoEmit.Builders;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Members;
using System.Collections.Generic;

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
    public static IEnumerable<ComplexBundle> Visit(this IComplexBundle parent, IEmitConverter converter)
    {
        var key = converter.Key;
        if (converter.Compiled)
        {
            if(converter is IWrapper<IEmitConverter> wrapper)
            {
                foreach (var item in Visit(parent, wrapper.Original))
                    yield return item;
            }
            else
            {
                // 不再解析
                parent.Accept(key, converter, false);
            }      
        }
        else if(converter is IComplexPreview preview)
        {
            foreach (var item in preview.Preview(parent))
                yield return item;
        }
        else if(converter is FuncConverter)
        {
            // 已解析表达式,不再解析
            parent.Accept(key, converter, false);
        }
        else if (converter is IWrapper<IEmitConverter> wrapper)
        {
            foreach (var item in Visit(parent, wrapper.Original))
                yield return item;
        }
    }
    /// <summary>
    /// 预览检测
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="reader"></param>
    public static IEnumerable<ComplexBundle> Visit(this IComplexBundle parent, IEmitReader reader)
    {
        if (reader is ConvertInstanceReader instanceReader)
        {
            foreach (var item in Visit(parent, instanceReader.Converter))
                yield return item;
            foreach (var item in Visit(parent, instanceReader.Original))
                yield return item;
        }
        else if (reader is ConvertValueReader valueReader)
        {
            foreach (var item in Visit(parent, valueReader.Converter))
                yield return item;
            foreach (var item in Visit(parent, valueReader.Original))
                yield return item;
        }
        else if (reader is IWrapper<IEmitReader> emitReader)
        {
            foreach (var item in Visit(parent.Context, emitReader.Original))
                yield return item;
        }
        else if (reader is IWrapper<IEmitMemberReader> memberReader)
        {
            foreach (var item in Visit(parent, memberReader.Original))
                yield return item;
        }
    }
    /// <summary>
    /// 预览检测
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static IEnumerable<ComplexBundle> Visit(this IComplexBundle parent, in PairTypeKey key)
    {
        var converter = parent.GetConverter(key);
        if (converter is null)
            return [];
        return Visit(parent, converter);
    }
    /// <summary>
    /// 预览检测
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static IEnumerable<ComplexBundle> Visit<TSource, TDest>(this IComplexBundle parent)
        => Visit(parent, new PairTypeKey(typeof(TSource), typeof(TDest)));
}
