using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Activators;

/// <summary>
/// 构造函数激活
/// </summary>
/// <param name="returnType"></param>
/// <param name="constructor"></param>
public class ConstructorActivator(Type returnType, ConstructorInfo constructor)
    : IEmitActivator
{
    #region 配置
    private readonly Type _returnType = returnType;
    private readonly ConstructorInfo _constructor = constructor;
    /// <inheritdoc />
    public Type ReturnType
        => _returnType;
    /// <summary>
    /// 构造函数
    /// </summary>
    public ConstructorInfo Constructor
        => _constructor;

    bool IEmitInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression New()
        => Expression.New(_constructor);
}
