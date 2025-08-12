using System.Linq.Expressions;

namespace PocoEmit.Collections.Counters;

/// <summary>
/// 数组长度获取
/// </summary>
public class ArrayLength : IEmitCounter
{
    /// <summary>
    /// 数组长度获取
    /// </summary>
    protected ArrayLength() { }
    #region IEmitCounter
    /// <inheritdoc />
    bool IEmitCounter.CountByProperty
        => true;
    Expression IEmitCounter.Count(Expression collection)
    => Expression.ArrayLength(collection);
    #endregion
    /// <summary>
    /// 获取数量
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    public static Expression Count(Expression array)
        => Expression.ArrayLength(array);
    /// <summary>
    /// 实例
    /// </summary>
    public static readonly ArrayLength Length = new();
}
