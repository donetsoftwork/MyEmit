using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Activators;

/// <summary>
/// 类型激活(一般结构体使用)
/// </summary>
/// <param name="returnType"></param>
public class TypeActivator(Type returnType)
    : IEmitActivator
{
    #region 配置
    private readonly Type _returnType = returnType;
    /// <inheritdoc />
    public Type ReturnType
        => _returnType;

    bool IEmitInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression New(Expression argument)
        => Expression.New(_returnType);
}
