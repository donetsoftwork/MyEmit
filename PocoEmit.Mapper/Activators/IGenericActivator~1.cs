using System;

namespace PocoEmit.Activators;

/// <summary>
/// 强类型激活
/// </summary>
/// <typeparam name="TInstance"></typeparam>
public interface IGenericActivator<TInstance> : IEmitActivator
{
    /// <summary>
    /// 激活委托
    /// </summary>
    Func<TInstance> Activator { get; }
}
