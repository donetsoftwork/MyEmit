using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Activators;

/// <summary>
/// 构造函数激活
/// </summary>
/// <param name="returnType">返回类型</param>
/// <param name="constructor">构造函数</param>
public class ConstructorActivator(Type returnType, ConstructorInfo constructor)
    : IEmitActivator
{
    #region 配置
    /// <summary>
    /// 返回类型
    /// </summary>
    protected readonly Type _returnType = returnType;
    /// <summary>
    /// 构造函数
    /// </summary>
    protected readonly ConstructorInfo _constructor = constructor;
    /// <inheritdoc />
    public Type ReturnType
        => _returnType;
    /// <summary>
    /// 构造函数
    /// </summary>
    public ConstructorInfo Constructor
        => _constructor;
    #endregion
    /// <inheritdoc />
    public virtual Expression New(Expression argument)
        => Expression.New(_constructor);
}
