using System.Linq.Expressions;

namespace PocoEmit.Enums;

/// <summary>
/// 枚举字段
/// </summary>
public interface IEnumField
{
    /// <summary>
    /// 枚举名
    /// </summary>
    string Name { get; }
    /// <summary>
    /// 序列化成员名
    /// </summary>
    string Member { get; }
    /// <summary>
    /// 枚举值
    /// </summary>
    ConstantExpression Expression { get; }
    /// <summary>
    /// 基础值
    /// </summary>
    ConstantExpression Under { get; }
    /// <summary>
    /// 匹配Name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    bool Match(string name);
    /// <summary>
    /// 匹配Member
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    bool MatchMember(string member);
}
