using System;
using System.Reflection;

namespace PocoEmit.Reflection;

/// <summary>
/// 反射构造函数
/// </summary>
public interface IReflectionConstructor
{
    /// <summary>
    /// 获取构造函数
    /// </summary>
    /// <param name="instanceType"></param>
    /// <returns></returns>
    ConstructorInfo GetConstructor(Type instanceType);
}
