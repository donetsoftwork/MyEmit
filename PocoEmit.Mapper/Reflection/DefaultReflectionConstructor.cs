using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PocoEmit.Reflection;

/// <summary>
/// 默认反射构造函数
/// </summary>
public class DefaultReflectionConstructor(bool parameterCountDesc)
    : IReflectionConstructor
{
    #region 配置
    private readonly bool _parameterCountDesc = parameterCountDesc;
    /// <summary>
    /// 获取参数最多的还是最少的
    /// </summary>
    public bool ParameterCountDesc 
        => _parameterCountDesc;
    #endregion
    /// <inheritdoc />
    public virtual ConstructorInfo GetConstructor(Type instanceType)
    {
        return Sort(GetConstructors(instanceType))
            .FirstOrDefault();
    }
    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    protected virtual IEnumerable<ConstructorInfo> Sort(IEnumerable<ConstructorInfo> list)
    {
        if (_parameterCountDesc)
            return list.OrderByDescending(c => c.GetParameters().Length);
        return list.OrderBy(c => c.GetParameters().Length);
    }
    /// <summary>
    /// 获取所有构造函数
    /// </summary>
    /// <param name="instanceType"></param>
    /// <returns></returns>
    protected virtual IEnumerable<ConstructorInfo> GetConstructors(Type instanceType)
        => ReflectionHelper.GetConstructors(instanceType);

    /// <summary>
    /// 默认反射构造函数实例
    /// </summary>
    public static readonly DefaultReflectionConstructor Default = new(false);
}
