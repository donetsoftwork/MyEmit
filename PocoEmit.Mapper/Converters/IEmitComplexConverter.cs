using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// Emit复杂类型转化
/// </summary>
public interface IEmitComplexConverter
{
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="cacher">复杂转化缓存</param>
    /// <param name="source">源表达式</param>
    /// <returns></returns>
    Expression Convert(ComplexContext cacher, Expression source);
}
