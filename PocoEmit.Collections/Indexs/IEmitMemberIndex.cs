using System.Linq.Expressions;

namespace PocoEmit.Indexs;

/// <summary>
/// 成员索引器
/// </summary>
public interface IEmitMemberIndex : IEmitIndexMemberReader, IEmitIndexMemberWriter;
/// <summary>
/// 成员索引读取器
/// </summary>
public interface IEmitIndexMemberReader
{
    /// <summary>
    /// 读取
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    Expression Read(Expression instance, Expression index);
}
/// <summary>
/// 成员索引写入器
/// </summary>
public interface IEmitIndexMemberWriter
{
    /// <summary>
    /// 写入
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="index"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    Expression Write(Expression instance, Expression index, Expression value);
}