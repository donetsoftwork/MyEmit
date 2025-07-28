using System.Linq.Expressions;

namespace PocoEmit.Copies;

/// <summary>
/// 成员转化器
/// </summary>
public interface IMemberConverter
{
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    Expression Convert(Expression source, Expression dest);
}
