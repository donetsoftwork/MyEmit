using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Converters;

/// <summary>
/// Emit实例方法类型转化
/// </summary>
/// <param name="instance"></param>
/// <param name="method"></param>
public class InstanceMethodConverter(object instance, MethodInfo method)
    : IEmitConverter
{
    #region 配置
    private readonly object _instance = instance;
    /// <summary>
    /// 实例
    /// </summary>
    public object Instance 
        => _instance;
    /// <summary>
    /// 方法
    /// </summary>
    protected readonly MethodInfo _method = method;
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
    public virtual Expression Convert(Expression value)
    {
        return _instance == null ?
            //eg: this.ToString()
            Expression.Call(value, _method) :
            //eg: converter.ToDateTime(value)
            Expression.Call(Expression.Constant(_instance), _method, value);
    }
    /// <summary>
    /// ToString
    /// </summary>
    public static InstanceMethodConverter ToStringConverter
        => Inner.ToStringConverter;
    /// <summary>
    /// 内部延迟初始化
    /// </summary>
    class Inner
    {
        /// <summary>
        /// ToString
        /// </summary>
        public static readonly InstanceMethodConverter ToStringConverter = new(null, ReflectionHelper.GetMethod(typeof(object), m => m.Name == "ToString" && m.GetParameters().Length == 0));
    }
}

