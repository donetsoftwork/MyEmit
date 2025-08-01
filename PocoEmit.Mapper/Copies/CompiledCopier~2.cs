using PocoEmit.Configuration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Copies;

/// <summary>
/// 已编译复制器
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
/// <param name="inner"></param>
/// <param name="copyAction"></param>
public sealed class CompiledCopier<TSource, TDest>(IEmitCopier inner, Action<TSource, TDest> copyAction)
    : ICompiledCopier<TSource, TDest>
{
    #region 配置
    private readonly IEmitCopier _inner = inner;
    /// <summary>
    /// 原始复制器
    /// </summary>
    public IEmitCopier Inner
        => _inner;
    private readonly Action<TSource, TDest> _copyAction = copyAction;
    /// <inheritdoc />
    public Action<TSource, TDest> CopyAction
        => _copyAction;
    /// <inheritdoc />
    bool IEmitInfo.Compiled
        => true;
    #endregion
    /// <inheritdoc />
    public IEnumerable<Expression> Copy(Expression source, Expression dest)
        => _inner.Copy(source, dest);
    /// <inheritdoc />
    void IPocoCopier<TSource, TDest>.Copy(TSource from, TDest to)
        => _copyAction(from, to);
    /// <inheritdoc />
    void IObjectCopier.CopyObject(object from, object to)
        => _copyAction((TSource)from, (TDest)to);
}
