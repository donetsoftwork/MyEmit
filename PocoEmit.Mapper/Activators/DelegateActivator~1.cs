using System;
using System.Reflection;

namespace PocoEmit.Activators;

/// <summary>
/// 委托激活类型
/// </summary>
/// <typeparam name="TInstance"></typeparam>
/// <param name="activator"></param>
public class DelegateActivator<TInstance>(Func<TInstance> activator)
    : StaticMethodActivator(activator.GetMethodInfo()), IGenericActivator<TInstance>
{
    #region 配置
    private readonly Func<TInstance> _activator = activator;
    /// <inheritdoc />
    public Func<TInstance> Activator
        => _activator;
    /// <inheritdoc />
    public override bool Compiled 
        => true;
    #endregion
}
