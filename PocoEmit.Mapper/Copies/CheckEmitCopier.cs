using PocoEmit.Builders;
using PocoEmit.Complexes;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Copies;

/// <summary>
/// 复制后检查
/// </summary>
/// <param name="inner"></param>
/// <param name="target"></param>
/// <param name="method"></param>
public class CheckEmitCopier(IEmitCopier inner, Expression target, MethodInfo method)
    : MethodCopier(target, method)
{
    /// <summary>
    /// 复制后检查
    /// </summary>
    /// <param name="inner"></param>
    /// <param name="action"></param>
    public CheckEmitCopier(IEmitCopier inner, Delegate action)
        : this(inner, EmitHelper.CheckMethodCallInstance(action), action.GetMethodInfo())
    {
    }
    #region 配置
    private readonly IEmitCopier _inner = inner;
    /// <summary>
    /// 内部转换器
    /// </summary>
    public IEmitCopier Inner
        => _inner;
    #endregion
    /// <inheritdoc />
    public override void BuildAction(IBuildContext context, IEmitBuilder builder, Expression source, Expression dest)
    {
        _inner.BuildAction(context, builder, source, dest);
        builder.Add(CallMethod(source, dest));
    }
    /// <inheritdoc />
    public override void Preview(IComplexBundle parent)
        => _inner.Preview(parent);
}
