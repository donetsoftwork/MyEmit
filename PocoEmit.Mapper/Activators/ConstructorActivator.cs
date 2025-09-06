using PocoEmit.Converters;
using PocoEmit.Members;
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
    : ConstructorBase(returnType, constructor)
    , IEmitActivator
{
    /// <inheritdoc />
    public virtual Expression New(ComplexContext cacher, Expression argument)
        => Expression.New(_constructor);
}
