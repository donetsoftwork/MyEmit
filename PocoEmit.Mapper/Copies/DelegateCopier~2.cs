using PocoEmit.Builders;
using System;
using System.Reflection;

namespace PocoEmit.Copies;

/// <summary>
/// 委托复制器
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
/// <param name="copyAction"></param>
/// <param name="method"></param>
public class DelegateCopier<TSource, TDest>(Action<TSource, TDest> copyAction, MethodInfo method)
    : MethodCopier(EmitHelper.CheckMethodCallInstance(copyAction), method), ICompiledCopier<TSource, TDest>
{
    /// <summary>
    /// 委托复制器
    /// </summary>
    /// <param name="copyAction"></param>
    public DelegateCopier(Action<TSource, TDest> copyAction)
        : this(copyAction, copyAction.GetMethodInfo())
    {
    }
    #region 配置
    private readonly Action<TSource, TDest> _copyAction = copyAction;
    /// <inheritdoc />
    public Action<TSource, TDest> CopyAction
        => _copyAction;
    /// <inheritdoc />
    public override bool Compiled 
        => true;
    #endregion
    /// <inheritdoc />
    void IPocoCopier<TSource, TDest>.Copy(TSource from, TDest to)
        => _copyAction(from, to);
    /// <inheritdoc />
    void IObjectCopier.CopyObject(object from, object to)
        => _copyAction((TSource)from, (TDest)to);
}