using PocoEmit.Resolves;
using System;

namespace PocoEmit.Converters;

/// <summary>
/// 上下文转化
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
/// <param name="convertFunc"></param>
public class ContextConverter<TSource, TDest>(Func<ConvertContext, TSource, TDest> convertFunc)
{
    #region 配置
    private readonly Func<ConvertContext, TSource, TDest> _convertFunc = convertFunc;
    /// <summary>
    /// 类型转化方法
    /// </summary>
    public Func<ConvertContext, TSource, TDest> ConvertFunc
        => _convertFunc;
    #endregion
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="context"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public TDest Convert(ConvertContext context, TSource source)
        => _convertFunc(context, source);
}
