using System.Collections.Generic;
using System.Reflection;

namespace PocoEmit.Members;

/// <summary>
/// 构造函数参数成员
/// </summary>
/// <param name="constructor"></param>
/// <param name="parameter"></param>
public class ConstructorParameterMember(ConstructorInfo constructor, ParameterInfo parameter)
    : MemberAccessor<ConstructorInfo>(constructor.DeclaringType, constructor, parameter.Name, parameter.ParameterType)
{
    #region 配置
    private readonly ParameterInfo _parameter = parameter;
    /// <summary>
    /// 参数
    /// </summary>
    public ParameterInfo Parameter 
        => _parameter;
    #endregion
    /// <summary>
    /// 转化为构造函数参数成员
    /// </summary>
    /// <param name="constructor"></param>
    /// <returns></returns>
    public static ConstructorParameterMember[] Convert(ConstructorInfo constructor)
        => Convert(constructor, constructor.GetParameters());
    /// <summary>
    /// 转化为构造函数参数成员
    /// </summary>
    /// <param name="constructor"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static ConstructorParameterMember[] Convert(ConstructorInfo constructor, ParameterInfo[] parameters)
    {
        var members = new ConstructorParameterMember[parameters.Length];
        int index = 0;
        foreach (var parameter in parameters)
        {
            members[index++] = new(constructor, parameter);
        }
        return members;
    }
}
