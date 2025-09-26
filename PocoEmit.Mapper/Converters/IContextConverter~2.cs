using PocoEmit.Resolves;

namespace PocoEmit.Converters;

/// <summary>
/// 上下文转化
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
public interface IContextConverter<TSource, TDest>
{
    /// <summary>
    /// 转化
    /// </summary>
    /// <returns></returns>
    TDest Convert(IConvertContext context, TSource source);
}
