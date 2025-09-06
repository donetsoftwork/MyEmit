using PocoEmit.Configuration;
using PocoEmit.Converters;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Copies;

/// <summary>
/// Emit类型复制器
/// </summary>
public interface IEmitCopier : ICompileInfo
{
    /// <summary>
    /// 复制
    /// </summary>
    /// <param name="cacher"></param>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    IEnumerable<Expression> Copy(ComplexContext cacher, Expression source, Expression dest);
}
