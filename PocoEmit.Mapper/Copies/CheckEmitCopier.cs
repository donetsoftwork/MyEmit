using PocoEmit.Builders;
using PocoEmit.Converters;
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// 复制
    /// </summary>
    /// <param name="cacher"></param>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    public override IEnumerable<Expression> Copy(ComplexContext cacher, Expression source, Expression dest)
    {
        foreach(var item in _inner.Copy(cacher, source, dest))
            yield return item;
        yield return CallMethod(source, dest);
    }
}
