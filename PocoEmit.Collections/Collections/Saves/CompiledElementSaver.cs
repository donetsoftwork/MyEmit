using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Collections.Saves;

/// <summary>
/// 已编集合元素保存器
/// </summary>
/// <typeparam name="TCollection"></typeparam>
/// <typeparam name="Element"></typeparam>
/// <param name="inner"></param>
/// <param name="saveAction"></param>
public class CompiledElementSaver<TCollection, Element>(IEmitElementSaver inner, Action<TCollection, Element> saveAction)
    : ICompiledElementSaver<TCollection, Element>
{
    #region 配置
    private readonly IEmitElementSaver _inner = inner;
    private readonly Action<TCollection, Element> _saveAction = saveAction;
    /// <summary>
    /// 内部保存器
    /// </summary>
    public IEmitElementSaver Inner 
        => _inner;
    /// <summary>
    /// 保存委托
    /// </summary>
    public Action<TCollection, Element> SaveAction 
        => _saveAction;
    /// <inheritdoc />
    public Type ElementType 
        => _inner.ElementType;
    /// <inheritdoc />
    MethodInfo IEmitElementSaver.AddMethod
        => _inner.AddMethod;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => true;
    #endregion
    /// <inheritdoc />
    Expression IEmitElementSaver.Add(Expression instance, Expression value)
        => _inner.Add(instance, value);
    /// <inheritdoc />
    public void Add(TCollection collection, Element item)
        => _saveAction(collection, item);
}
