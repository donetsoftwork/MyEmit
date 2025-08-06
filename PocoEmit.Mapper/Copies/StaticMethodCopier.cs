using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Copies;

/// <summary>
/// Emit方法类型转化
/// </summary>
/// <param name="target"></param>
/// <param name="method"></param>
public class MethodCopier(object target, MethodInfo method)
    : IEmitCopier
{
    #region 配置
    private readonly object _target = target;
    /// <summary>
    /// 方法
    /// </summary>
    protected readonly MethodInfo _method = method;
    /// <summary>
    /// 实例
    /// </summary>
    public object Target
        => _target;
    /// <summary>
    /// 方法
    /// </summary>
    public MethodInfo Method
        => _method;
    /// <summary>
    /// 调用实例
    /// </summary>
    public Expression Instance
        => ReflectionHelper.CheckMethodCallInstance(_target);
    /// <inheritdoc />
    public virtual bool Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public virtual IEnumerable<Expression> Copy(Expression source, Expression dest)
    {
        var call = Expression.Call(Instance, _method, source, dest);
        return [call];
    }
}
