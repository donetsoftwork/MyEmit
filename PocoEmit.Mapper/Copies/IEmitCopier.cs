using PocoEmit.Configuration;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Copies;

/// <summary>
/// Emit类型复制器
/// </summary>
public interface IEmitCopier : IEmitInfo
{
    /// <summary>
    /// 复制
    /// </summary>
    /// <param name="source">源</param>
    /// <param name="dest">目标</param>
    /// <returns></returns>
    IEnumerable<Expression> Copy(Expression source, Expression dest);
}
