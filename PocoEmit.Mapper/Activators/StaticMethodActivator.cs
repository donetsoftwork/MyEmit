using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Activators;

/// <summary>
/// 静态方法激活类型
/// </summary>
/// <param name="method"></param>
public class StaticMethodActivator(MethodInfo method)
    : IEmitActivator
{
    #region 配置
    private readonly MethodInfo _method = method;
    /// <inheritdoc />
    public Type ReturnType 
        => _method.ReturnType;
    /// <summary>
    /// 方法
    /// </summary>
    public MethodInfo Method 
        => _method;
    /// <inheritdoc />
    public virtual bool Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression New()
        => Expression.Call(_method);
}
