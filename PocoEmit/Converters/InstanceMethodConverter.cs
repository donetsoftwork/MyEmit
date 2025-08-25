using PocoEmit.Builders;
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
    public Expression Convert(Expression source)
        => Expression.Call(source, _method);
    /// <summary>
    /// ToString方法信息
    /// </summary>
    public static readonly MethodInfo ToStringMethod = EmitHelper.GetMethodInfo<object, string>(obj => obj.ToString());
    /// <summary>
    /// ToString
    /// </summary>
    public static readonly SelfMethodConverter ToStringConverter = new(ToStringMethod);
}
