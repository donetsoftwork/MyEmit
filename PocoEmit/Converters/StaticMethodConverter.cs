using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Converters;

/// <summary>
/// Emit静态方法类型转化
/// </summary>
/// <param name="method"></param>
/// <param name="sourceType"></param>
public class StaticMethodConverter(MethodInfo method, Type sourceType)
    : IEmitConverter
{
    /// <summary>
    /// Emit方法类型转化
    /// </summary>
    /// <param name="method"></param>
    public StaticMethodConverter(MethodInfo method)
        : this(method, method.GetParameters()[0].ParameterType)
    {
    }
    #region 配置
    /// <summary>
    /// 方法
    /// </summary>
    protected readonly MethodInfo _method = method;
    /// <summary>
    /// 方法
    /// </summary>
    public MethodInfo Method 
        => _method;
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
        => Expression.Convert(value, _method.ReturnType, _method);
}
