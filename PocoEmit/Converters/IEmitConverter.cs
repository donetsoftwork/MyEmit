using PocoEmit.Configuration;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// Emit类型转化
/// </summary>
public interface IEmitConverter
    : IEmitInfo
{
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="source"></param>
    Expression Convert(Expression source);
}
