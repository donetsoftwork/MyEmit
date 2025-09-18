using PocoEmit.Complexes;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Activators;

/// <summary>
/// Emit类型激活(初始化)器
/// </summary>
public interface IEmitActivator
{
    /// <summary>
    /// 返回类型
    /// </summary>
    Type ReturnType { get; }
    /// <summary>
    /// 激活
    /// </summary>
    /// <param name="context">复杂类型缓存</param>
    /// <param name="argument">参数</param>
    /// <returns></returns>
    Expression New(IBuildContext context, Expression argument);
    ///// <summary>
    ///// 激活
    ///// </summary>
    ///// <param name="argument">参数</param>
    ///// <returns></returns>
    //Expression New(Expression argument);
}
