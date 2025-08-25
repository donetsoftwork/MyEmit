using System;
using System.Reflection;

namespace PocoEmit.Members;

/// <summary>
/// 构造函数基类
/// </summary>
/// <param name="returnType"></param>
/// <param name="constructor"></param>
public class ConstructorBase(Type returnType, ConstructorInfo constructor)
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
}