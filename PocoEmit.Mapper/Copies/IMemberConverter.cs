using PocoEmit.Complexes;
using PocoEmit.Members;
using System.Linq.Expressions;

namespace PocoEmit.Copies;

/// <summary>
/// 成员转化器
/// </summary>
public interface IMemberConverter
    : IComplexPreview
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
    /// <param name="context"></param>
    /// <param name="sourceMember"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    Expression ConvertMember(IBuildContext context, Expression sourceMember, Expression dest);
    ///// <summary>
    ///// 转化成员
    ///// </summary>
    ///// <param name="sourceMember"></param>
    ///// <param name="dest"></param>
    ///// <returns></returns>
    //Expression ConvertMember(Expression sourceMember, Expression dest);
    /// <summary>
    /// 源成员读取器
    /// </summary>
    IEmitReader SourceReader { get; }
    /// <summary>
    /// 目标成员写入器
    /// </summary>
    IEmitMemberWriter DestWriter { get; }
}
