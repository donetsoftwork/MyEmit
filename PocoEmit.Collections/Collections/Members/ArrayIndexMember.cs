using PocoEmit.Configuration;
using PocoEmit.Members;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Members;

/// <summary>
/// 索引一维数组成员
/// </summary>
/// <param name="index"></param>
public class ArrayIndexMember(int index)
    : IEmitReader, IEmitWriter
{
    private readonly int _index = index;
    /// <summary>
    /// 索引值
    /// </summary>
    public int Index 
        => _index;
    #region 配置
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Read(Expression instance)
        => Expression.ArrayAccess(instance, Expression.Constant(_index));
    /// <inheritdoc />
    public Expression Write(Expression instance, Expression value)
        => Expression.Assign(Expression.ArrayAccess(instance, Expression.Constant(_index)), value);
}
