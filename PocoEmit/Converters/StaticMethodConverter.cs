using PocoEmit.Configuration;
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
/// <param name="destType"></param>
public class MethodConverter(Expression target, MethodInfo method, Type sourceType, Type destType)
    : IEmitConverter
{
    /// <summary>
    /// Emit方法类型转化
    /// </summary>
    /// <param name="target"></param>
    /// <param name="method"></param>
    public MethodConverter(Expression target, MethodInfo method)
        : this(target, method, method.GetParameters()[0].ParameterType, method.ReturnType)
    {
    }
    #region 配置
    private readonly Expression _target = target;
    /// <summary>
    /// 方法
    /// </summary>
    protected readonly MethodInfo _method = method;
    /// <summary>
    /// 实例
    /// </summary>
    public Expression Target
        => _target;
    /// <summary>
    /// 方法
    /// </summary>
    public MethodInfo Method 
        => _method;
    /// <summary>
    /// summary
    /// </summary>
    protected readonly Type _sourceType = sourceType;
    private readonly PairTypeKey _key = new(sourceType, destType);    
    /// <summary>
    /// 映射源类型
    /// </summary>
    public Type SourceType
        => _sourceType;
    /// <summary>
    /// 转化类型
    /// </summary>
    public PairTypeKey Key
        => _key;
    /// <inheritdoc />
    public virtual bool Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public virtual Expression Convert(Expression value)
    {
        if (_method.IsStatic)
            return Expression.Convert(value, _method.ReturnType, _method);
        return Expression.Call(_target, _method, value);
    }
}
