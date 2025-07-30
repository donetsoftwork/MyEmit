namespace PocoEmit.Converters;

/// <summary>
/// 弱类型转化(用于类型反射场景)
/// </summary>
public interface IObjectConverter
{
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    object ConvertObject(object source);
}
