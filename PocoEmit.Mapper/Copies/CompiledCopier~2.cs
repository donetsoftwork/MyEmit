using PocoEmit.Builders;
using PocoEmit.Complexes;
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
/// <param name="original"></param>
/// <param name="copyAction"></param>
public sealed class CompiledCopier<TSource, TDest>(IEmitCopier original, Action<TSource, TDest> copyAction)
    : ICompiledCopier<TSource, TDest>
    , IWrapper<IEmitCopier>
{
    #region 配置
    private readonly IEmitCopier _original = original;
    /// <summary>
    /// 原始复制器
    /// </summary>
    public IEmitCopier Original
        => _original;
    private readonly Action<TSource, TDest> _copyAction = copyAction;
    /// <inheritdoc />
    public Action<TSource, TDest> CopyAction
        => _copyAction;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => true;
    #endregion
    /// <inheritdoc />
    public IEnumerable<Expression> Copy(IBuildContext context, Expression source, Expression dest)
        => _original.Copy(context, source, dest);
    /// <inheritdoc />
    void IPocoCopier<TSource, TDest>.Copy(TSource from, TDest to)
        => _copyAction(from, to);
    /// <inheritdoc />
    void IObjectCopier.CopyObject(object from, object to)
        => _copyAction((TSource)from, (TDest)to);
    /// <inheritdoc />
    IEnumerable<ComplexBundle> IComplexPreview.Preview(IComplexBundle parent)
        => _original.Preview(parent);
}
