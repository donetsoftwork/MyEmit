using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Copies;

/// <summary>
/// Emit静态方法类型转化
/// </summary>
/// <param name="method"></param>
public class StaticMethodCopier(MethodInfo method)
    : IEmitCopier
{
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
    /// <inheritdoc />
    public virtual bool Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public virtual IEnumerable<Expression> Copy(Expression source, Expression dest)
    {
        var call = Expression.Call(null, _method, source, dest);
        return [call];
    }
}
