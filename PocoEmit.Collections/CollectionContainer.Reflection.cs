using Hand.Reflection;
using System;
using System.Reflection;

namespace PocoEmit;

/// <summary>
/// 集合容器(反射)
/// </summary>
public sealed partial class CollectionContainer
{
    #region ConstructorInfo
    /// <summary>
    /// 获取容量构造函数
    /// </summary>
    /// <param name="collectionType"></param>
    /// <returns></returns>
    public static ConstructorInfo GetCapacityConstructor(Type collectionType)
        => ReflectionMember.GetConstructor(collectionType, CheckCapacityParameter);
    /// <summary>
    /// 判断容量参数
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    private static bool CheckCapacityParameter(ParameterInfo[] parameters)
    {
        if (parameters.Length == 1)
        {
            var parameter = parameters[0];
            return parameter.Name == "capacity" && parameter.ParameterType == typeof(int);
        }
        return false;
    }
    #endregion
    #region PropertyInfo
    /// <summary>
    /// 获取属性Count
    /// </summary>
    /// <param name="collectionType"></param>
    /// <returns></returns>
    public static PropertyInfo GetCountProperty(Type collectionType)
        => ReflectionMember.GetPropery(collectionType, property => property.Name == "Count" && property.CanRead);
    /// <summary>
    /// 获取索引器
    /// </summary>
    /// <param name="collectionType">集合类型</param>
    /// <param name="parameterTypes">参数类型</param>
    /// <returns></returns>
    public static PropertyInfo GetItemProperty(Type collectionType, params Type[] parameterTypes)
        => ReflectionMember.GetPropery(collectionType, property => property.Name == "Item" && ReflectionMember.MatchParameter(property.GetIndexParameters(), parameterTypes));
    #endregion
    #region MethodInfo
    /// <summary>
    /// 获取添加方法
    /// </summary>
    /// <param name="collectionType"></param>
    /// <param name="elementType"></param>
    /// <param name="methodName"></param>
    /// <returns></returns>
    public static MethodInfo GetAddMethod(Type collectionType, Type elementType, string methodName = "Add")
        => ReflectionMember.GetMethod(collectionType, methodName, [elementType]);
    #endregion
}
