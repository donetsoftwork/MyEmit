using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PocoEmit;

/// <summary>
/// 反射帮助类
/// </summary>
public static class ReflectionHelper
{
    #region GetTypeMembers
    /// <summary>
    /// 获取类型所有成员
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="poco"></param>
    /// <returns></returns>
    public static MemberBundle GetTypeMembers<TInstance>(this IPoco poco)
        => poco.MemberCacher.Get(typeof(TInstance));
    /// <summary>
    /// 获取类型所有成员
    /// </summary>
    /// <param name="poco"></param>
    /// <param name="instanceType"></param>
    /// <returns></returns>
    public static MemberBundle GetTypeMembers(this IPoco poco, Type instanceType)
        => poco.MemberCacher.Get(instanceType);
    #endregion
    #region GetReadMember
    /// <summary>
    /// 获取可读成员
    /// </summary>
    /// <param name="poco"></param>
    /// <param name="instanceType"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static MemberInfo GetReadMember(this IPoco poco, Type instanceType, string memberName)
    {
        return poco.MemberCacher
            .Get(instanceType)
            ?.GetReadMember(memberName);
    }
    /// <summary>
    /// 获取可读成员
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="poco"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static MemberInfo GetReadMember<TInstance>(this IPoco poco, string memberName)
    {
        return poco.MemberCacher
            .Get(typeof(TInstance))
            ?.GetReadMember(memberName);
    }
    /// <summary>
    /// 获取可读成员
    /// </summary>
    /// <param name="bundle"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static MemberInfo GetReadMember(this MemberBundle bundle, string memberName)
    {
        if (bundle.ReadMembers.TryGetValue(memberName, out var member))
            return member;
        return null;
    }
    #endregion
    #region GetReader
    /// <summary>
    /// 获取可读成员
    /// </summary>
    /// <param name="poco"></param>
    /// <param name="instanceType"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static IEmitMemberReader GetEmitReader(this IPoco poco, Type instanceType, string memberName)
    {
        return poco.MemberCacher
            .Get(instanceType)
            ?.GetEmitReader(memberName);
    }
    /// <summary>
    /// 获取可读成员
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="poco"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static IEmitMemberReader GetEmitReader<TInstance>(this IPoco poco, string memberName)
    {
        return poco.MemberCacher
            .Get(typeof(TInstance))
            ?.GetEmitReader(memberName);
    }
    /// <summary>
    /// 获取可读成员
    /// </summary>
    /// <param name="bundle"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static IEmitMemberReader GetEmitReader(this MemberBundle bundle, string memberName)
    {
        if (bundle.EmitReaders.TryGetValue(memberName, out var reader))
            return reader;
        return null;
    }
    #endregion
    #region GetWriteMember
    /// <summary>
    /// 获取可写成员
    /// </summary>
    /// <param name="poco"></param>
    /// <param name="instanceType"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static MemberInfo GetWriteMember(this IPoco poco, Type instanceType, string memberName)
    {
        return poco.MemberCacher
            .Get(instanceType)
            ?.GetWriteMember(memberName);
    }
    /// <summary>
    /// 获取可写成员
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="poco"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static MemberInfo GetWriteMember<TInstance>(this IPoco poco, string memberName)
    {
        return poco.MemberCacher
            .Get(typeof(TInstance))
            ?.GetWriteMember(memberName);
    }
    /// <summary>
    /// 获取可读成员
    /// </summary>
    /// <param name="bundle"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static MemberInfo GetWriteMember(this MemberBundle bundle, string memberName)
    {
        if (bundle.WriteMembers.TryGetValue(memberName, out var member))
            return member;
        return null;
    }
    #endregion
    #region GetWriter
    /// <summary>
    /// 获取可写成员
    /// </summary>
    /// <param name="poco"></param>
    /// <param name="instanceType"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static IEmitMemberWriter GetEmitWriter(this IPoco poco, Type instanceType, string memberName)
    {
        return poco.MemberCacher
            .Get(instanceType)
            ?.GetEmitWriter(memberName);
    }
    /// <summary>
    /// 获取可写成员
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="poco"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static IEmitMemberWriter GetEmitWriter<TInstance>(this IPoco poco, string memberName)
    {
        return poco.MemberCacher
            .Get(typeof(TInstance))
            ?.GetEmitWriter(memberName);
    }
    /// <summary>
    /// 获取可读成员
    /// </summary>
    /// <param name="bundle"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static IEmitMemberWriter GetEmitWriter(this MemberBundle bundle, string memberName)
    {
        if (bundle.EmitWriters.TryGetValue(memberName, out var writer))
            return writer;
        return null;
    }
    #endregion
    /// <summary>
    /// 筛选属性
    /// </summary>
    /// <param name="declareType"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public static PropertyInfo GetPropery(Type declareType, Func<PropertyInfo, bool> filter)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var properties = declareType.GetTypeInfo().DeclaredProperties;
#else
        var properties = declareType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
#endif
        foreach (var propery in properties)
        {
            if (filter(propery))
                return propery;
        }
        return null;
    }
    #region Properties
    /// <summary>
    /// 获取所有属性
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetProperties(Type declareType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => GetProperties(declareType.GetTypeInfo());
    /// <summary>
    /// 获取属性信息
    /// </summary>
    /// <param name="declaringTypeInfo"></param>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetProperties(TypeInfo declaringTypeInfo)
        => declaringTypeInfo.DeclaredProperties;
#else
        => declareType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
#endif
    #endregion
    #region GetFields
    /// <summary>
    /// 获取所有实例字段
    /// </summary>
    /// <typeparam name="TStructuralType"></typeparam>
    /// <returns></returns>
    public static IEnumerable<FieldInfo> GetFields<TStructuralType>()
        => GetFields(typeof(TStructuralType));
    /// <summary>
    /// 获取所有实例字段
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<FieldInfo> GetFields(Type declareType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => GetFields(declareType.GetTypeInfo());
    /// <summary>
    /// 获取所有实例字段
    /// </summary>
    /// <param name="declaringTypeInfo"></param>
    /// <returns></returns>
    public static IEnumerable<FieldInfo> GetFields(TypeInfo declaringTypeInfo)
        => declaringTypeInfo.DeclaredFields.Where(field => field.IsPublic && !field.IsStatic);
#else
        => declareType.GetFields(BindingFlags.Instance | BindingFlags.Public);
#endif
    #endregion
    #region GetStaticFields
    /// <summary>
    /// 获取所有静态字段
    /// </summary>
    /// <typeparam name="TStructuralType"></typeparam>
    /// <returns></returns>
    public static IEnumerable<FieldInfo> GetStaticFields<TStructuralType>()
        => GetStaticFields(typeof(TStructuralType));
    /// <summary>
    /// 获取所有静态字段
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<FieldInfo> GetStaticFields(Type declareType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => GetStaticFields(declareType.GetTypeInfo());
    /// <summary>
    /// 获取所有静态字段
    /// </summary>
    /// <param name="declaringTypeInfo"></param>
    /// <returns></returns>
    public static IEnumerable<FieldInfo> GetStaticFields(TypeInfo declaringTypeInfo)
        => declaringTypeInfo.DeclaredFields.Where(field => field.IsPublic && field.IsStatic);
#else
        => declareType.GetFields(BindingFlags.Static | BindingFlags.Public);
#endif
    #endregion
    #region IsNullable
    /// <summary>
    /// 是否可空类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsNullable(Type type)
        => IsGenericType(type, typeof(Nullable<>));
    #endregion
    #region IsGenericType
    /// <summary>
    /// 是否泛型定义
    /// </summary>
    /// <param name="type"></param>
    /// <param name="genericType">泛型</param>
    /// <returns></returns>
    public static bool IsGenericType(Type type, Type genericType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => IsGenericType(type.GetTypeInfo(), genericType);
    /// <summary>
    /// 是否泛型定义
    /// </summary>
    /// <param name="typeInfo"></param>
    /// <param name="genericType"></param>
    /// <returns></returns>
    public static bool IsGenericType(TypeInfo typeInfo, Type genericType)
        => typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == genericType;
#else
        => type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
#endif
    #endregion
    #region HasGenericType
    /// <summary>
    /// 判断是否包含泛型定义
    /// </summary>
    /// <param name="type"></param>
    /// <param name="genericType"></param>
    /// <returns></returns>
    public static bool HasGenericType(Type type, Type genericType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => HasGenericType(type.GetTypeInfo(), genericType);
    /// <summary>
    /// 判断是否包含泛型定义
    /// </summary>
    /// <param name="type"></param>
    /// <param name="genericType"></param>
    /// <returns></returns>
    public static bool HasGenericType(TypeInfo type, Type genericType)
#endif
    {
        if (IsGenericType(type, genericType))
            return true;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var interfaces = type.ImplementedInterfaces;
#else
        var interfaces = type.GetInterfaces();
#endif
        foreach ( var subType in interfaces)
        {
            if(IsGenericType(subType, genericType))
                return true;
        }
        return false;
    }
    #endregion
    /// <summary>
    /// 获取子元素类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Type GetElementType(Type type)
    {
        if (type.IsArray)
            return type.GetElementType();
        var arguments = GetGenericArguments(type);
        var count = arguments.Length;
        if (count == 0)
            return null;
        if (HasGenericType(type, typeof(IDictionary<,>)))
        {
            if (count == 2)
                return arguments[1];
        }
        else if (HasGenericType(type, typeof(IEnumerable<>)))
        {
            if (count == 1)
                return arguments[0];
        }
        return null;
    }
    /// <summary>
    /// 获取泛型参数
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Type[] GetGenericArguments(Type type)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var typeInfo = type.GetTypeInfo();
        if (typeInfo.IsGenericType)
            return typeInfo.GenericTypeParameters;
#else
        if (type.IsGenericType)
            return type.GetGenericArguments();
#endif
        return [];
    }
    #region ConstructorInfo
    /// <summary>
    /// 获取构造函数
    /// </summary>
    /// <param name="declareType"></param>
    /// <param name="parameterType"></param>
    /// <returns></returns>
    public static ConstructorInfo GetConstructorByParameterType(Type declareType, Type parameterType)
        => GetConstructor(
            declareType,
            parameters => parameters.Length == 1
                && PairTypeKey.CheckValueType(parameters[0].ParameterType, parameterType));
    /// <summary>
    /// 获取构造函数
    /// </summary>
    /// <param name="declareType"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public static ConstructorInfo GetConstructor(Type declareType, Func<ParameterInfo[], bool> filter)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var constructors = declareType.GetTypeInfo().DeclaredConstructors;
#else
        var constructors = declareType.GetConstructors();
#endif
        foreach (var constructor in constructors)
        {
            if (filter(constructor.GetParameters()))
                return constructor;
        }
        return null;
    }
    /// <summary>
    /// 获取所有构造函数
    /// </summary>
    /// <param name="declareType"></param>
    /// <returns></returns>
    public static IEnumerable<ConstructorInfo> GetConstructors(Type declareType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
            => GetConstructors(declareType.GetTypeInfo());
        /// <summary>
        /// 获取所有构造函数
        /// </summary>
        /// <param name="declaringTypeInfo"></param>
        /// <returns></returns>
        public static IEnumerable<ConstructorInfo> GetConstructors(TypeInfo declaringTypeInfo)
            => declaringTypeInfo.DeclaredConstructors;
#else
        => declareType.GetConstructors();
#endif
#endregion
#region MethodInfo
    /// <summary>
    /// 获取方法
    /// </summary>
    /// <param name="declareType"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static MethodInfo GetMethod(Type declareType, string name)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => declareType.GetTypeInfo().GetDeclaredMethod(name);
#else
        => declareType.GetMethod(name);
#endif
    /// <summary>
    /// 获取方法
    /// </summary>
    /// <param name="declareType"></param>
    /// <param name="name"></param>
    /// <param name="types"></param>
    /// <returns></returns>
    public static MethodInfo GetMethod(Type declareType, string name, Type[] types)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
    {
        return declareType.GetTypeInfo()
            .GetDeclaredMethods(name)
            .FirstOrDefault(method => MatchParameters(method.GetParameters(), types));
    }
#else
        => declareType.GetMethod(name, types);
#endif
    /// <summary>
    /// 获取所有函数
    /// </summary>
    /// <param name="declareType"></param>
    /// <returns></returns>
    public static IEnumerable<MethodInfo> GetMethods(Type declareType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => GetMethods(declareType.GetTypeInfo());
    /// <summary>
    /// 获取所有函数
    /// </summary>
    /// <param name="declaringTypeInfo"></param>
    /// <returns></returns>
    public static IEnumerable<MethodInfo> GetMethods(TypeInfo declaringTypeInfo)
        => declaringTypeInfo.DeclaredMethods;
#else
        => declareType.GetMethods();
#endif
    #endregion
    /// <summary>
    /// 匹配参数
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="types"></param>
    /// <returns></returns>
    public static bool MatchParameters(ParameterInfo[] parameters, Type[] types)
    {
        var count = parameters.Length;
        if (count == types.Length)
        {
            for (int i = 0; i < count; i++)
            {
                if (parameters[i].ParameterType == types[i])
                    continue;
                else
                    return false;
            }
            return true;
        }
        return false;
    }
}
