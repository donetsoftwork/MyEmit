using PocoEmit.Configuration;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Copies;

/// <summary>
/// 空复制器
/// </summary>
public class EmptyCopier : IEmitCopier
{
    private EmptyCopier() { }
    #region IEmitCopier
    bool IEmitInfo.Compiled
        => false;
    IEnumerable<Expression> IEmitCopier.Copy(Expression source, Expression dest)
        => [];
    #endregion
    /// <summary>
    /// 单例
    /// </summary>
    public static EmptyCopier Instance = new();

}
