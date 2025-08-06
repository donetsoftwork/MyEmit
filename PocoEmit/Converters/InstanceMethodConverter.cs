using PocoEmit.Configuration;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Converters;

/// <summary>
/// Emit本实例方法类型转化
/// </summary>
/// <param name="method"></param>
public sealed class SelfMethodConverter(MethodInfo method)
    : IEmitConverter
{
    #region 配置
    /// <summary>
    /// 方法
    /// </summary>
    private readonly MethodInfo _method = method;
    /// <summary>
    /// 方法
    /// </summary>
    public MethodInfo Method
        => _method;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Convert(Expression value)
        => Expression.Call(value, _method);
    /// <summary>
    /// ToString
    /// </summary>
    public static SelfMethodConverter ToStringConverter
        => Inner.ToStringConverter;
    /// <summary>
    /// 内部延迟初始化
    /// </summary>
    class Inner
    {
        /// <summary>
        /// ToString
        /// </summary>
        public static readonly SelfMethodConverter ToStringConverter = new(ReflectionHelper.GetMethod(typeof(object), m => m.Name == "ToString" && m.GetParameters().Length == 0));
    }
}

