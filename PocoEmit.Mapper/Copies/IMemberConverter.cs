using PocoEmit.Members;
using System.Linq.Expressions;

namespace PocoEmit.Copies;

/// <summary>
/// 成员转化器
/// </summary>
public interface IMemberConverter
{
    /// <summary>
    /// 获取源成员
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    Expression GetSourceMember(Expression source);
    /// <summary>
    /// 转化成员
    /// </summary>
    /// <param name="sourceMember"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    Expression ConvertMember(Expression sourceMember, Expression dest);

    /// <summary>
    /// 源成员读取器
    /// </summary>
    IEmitReader SourceReader { get; }
    /// <summary>
    /// 目标成员写入器
    /// </summary>
    IEmitWriter DestWriter { get; }
}
