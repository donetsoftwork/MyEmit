using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Activators;

/// <summary>
/// 类型激活(初始化)
/// </summary>
public interface IEmitActivator : IEmitInfo
{
    /// <summary>
    /// 返回类型
    /// </summary>
    Type ReturnType { get; }
    /// <summary>
    /// 激活
    /// </summary>
    /// <returns></returns>
    Expression New();
}
