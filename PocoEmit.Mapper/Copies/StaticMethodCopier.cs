using PocoEmit.Builders;
using PocoEmit.Complexes;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Copies;

/// <summary>
/// Emit方法类型转化
/// </summary>
/// <param name="target"></param>
/// <param name="method"></param>
public class MethodCopier(Expression target, MethodInfo method)
    : IEmitCopier
{
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
    /// <inheritdoc />
    public virtual bool Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public virtual void BuildAction(IBuildContext context, IEmitBuilder builder, Expression source, Expression dest)
        => builder.Add(CallMethod(source, dest));
    /// <summary>
    /// 调用方法
    /// </summary>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    protected Expression CallMethod(Expression source, Expression dest)
        => Expression.Call(_target, _method, source, dest);
    /// <inheritdoc />
    public virtual void Preview(IComplexBundle parent) { }
}
