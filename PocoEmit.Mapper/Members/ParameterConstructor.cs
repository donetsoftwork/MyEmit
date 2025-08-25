using System;
using System.Reflection;

namespace PocoEmit.Members;

/// <summary>
/// 含参构造函数
/// </summary>
/// <param name="returnType">返回类型</param>
/// <param name="constructor">构造函数</param>
/// <param name="parameters">参数</param>
public class ParameterConstructor(Type returnType, ConstructorInfo constructor, ConstructorParameterMember[] parameters)
    : ConstructorBase(returnType, constructor)
{
    /// <summary>
    /// 含参构造函数激活
    /// </summary>
    /// <param name="returnType"></param>
    /// <param name="constructor"></param>
    /// <param name="parameters"></param>
    public ParameterConstructor(Type returnType, ConstructorInfo constructor, ParameterInfo[] parameters)
        : this(returnType, constructor, ConstructorParameterMember.Convert(constructor, parameters))
    {
    }
    #region 配置
    private readonly ConstructorParameterMember[] _parameters = parameters;
    /// <summary>
    /// 参数列表
    /// </summary>
    public ConstructorParameterMember[] Parameters
        => _parameters;
    #endregion
}
