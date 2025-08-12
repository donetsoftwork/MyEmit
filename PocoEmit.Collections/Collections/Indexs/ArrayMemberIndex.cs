using PocoEmit.Indexs;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Indexs;

/// <summary>
/// 数组成员索引器
/// </summary>
public class ArrayMemberIndex : IEmitMemberIndex
{
    private ArrayMemberIndex() { }
    /// <inheritdoc />
    public Expression Read(Expression instance, Expression index)
        => Expression.ArrayAccess(instance, index);
    /// <inheritdoc />
    public Expression Write(Expression instance, Expression index, Expression value)
        => Expression.Assign(Expression.ArrayAccess(instance, index), value);
    /// <summary>
    /// 单例
    /// </summary>

    public static readonly ArrayMemberIndex Instance = new();
}
