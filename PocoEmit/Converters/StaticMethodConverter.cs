using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Converters;

/// <summary>
/// Emit方法类型转化
/// </summary>
/// <param name="target"></param>
/// <param name="method"></param>
/// <param name="sourceType"></param>
public class MethodConverter(object target, MethodInfo method, Type sourceType)
    : IEmitConverter
{
    /// <summary>
    /// Emit方法类型转化
    /// </summary>
    /// <param name="target"></param>
    /// <param name="method"></param>
    public MethodConverter(object target, MethodInfo method)
        : this(target, method, method.GetParameters()[0].ParameterType)
    {
    }
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
    /// <summary>
    /// summary
    /// </summary>
    protected readonly Type _sourceType = sourceType;
    /// <summary>
    /// 映射源类型
    /// </summary>
    public Type SourceType
        => _sourceType;
    /// <inheritdoc />
    public virtual bool Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public virtual Expression Convert(Expression value)
    {
        if (_method.IsStatic)
            return Expression.Convert(value, _method.ReturnType, _method);
        return Expression.Call(Instance, _method, value);
    }
}
