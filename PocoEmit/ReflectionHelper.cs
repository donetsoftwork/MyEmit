using PocoEmit.Collections;
using PocoEmit.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    #region Properties
    /// <summary>
    /// 获取所有属性
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetProperties(Type type)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => GetProperties(type.GetTypeInfo());
    /// <summary>
    /// 获取属性信息
    /// </summary>
    /// <param name="typeInfo"></param>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetProperties(TypeInfo typeInfo)
        => typeInfo.DeclaredProperties;
#else
        => type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
#endif
    /// <summary>
    /// 获取所有可读属性
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetReadProperties(Type type)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => GetReadProperties(type.GetTypeInfo());
    /// <summary>
    /// 获取属性信息
    /// </summary>
    /// <param name="typeInfo"></param>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetReadProperties(TypeInfo typeInfo)
        => typeInfo.DeclaredProperties.Where(property => property.CanRead);
#else
        => type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
#endif
    /// <summary>
    /// 获取所有可读属性
    /// </summary>
    /// <param name="declaringType"></param>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetWriteProperties(Type declaringType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => GetWriteProperties(declaringType.GetTypeInfo());
    /// <summary>
    /// 获取所有属性
    /// </summary>
    /// <param name="typeInfo"></param>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetWriteProperties(TypeInfo typeInfo)
        => typeInfo.DeclaredProperties.Where(property => property.CanWrite);
#else
        => declaringType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
#endif
    #endregion
    #region GetFields
    /// <summary>
    /// 获取所有字段
    /// </summary>
    /// <typeparam name="TStructuralType"></typeparam>
    /// <returns></returns>
    public static IEnumerable<FieldInfo> GetFields<TStructuralType>()
        => GetFields(typeof(TStructuralType));
    /// <summary>
    /// 获取所有字段
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<FieldInfo> GetFields(Type declaringType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => GetFields(declaringType.GetTypeInfo());
    /// <summary>
    /// 获取属性信息
    /// </summary>
    /// <param name="declaringTypeInfo"></param>
    /// <returns></returns>
    public static IEnumerable<FieldInfo> GetFields(TypeInfo declaringTypeInfo)
        => declaringTypeInfo.DeclaredFields.Where(field => field.IsPublic && !field.IsStatic);
#else
        => declaringType.GetFields(BindingFlags.Instance | BindingFlags.Public);
#endif
    #endregion
    #region CheckType
    /// <summary>
    /// 判断是否兼容值类型
    /// </summary>
    /// <param name="fromType"></param>
    /// <param name="toType"></param>
    /// <returns></returns>
    public static bool CheckValueType(Type fromType, Type toType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => CheckValueType(fromType.GetTypeInfo(), toType.GetTypeInfo());
    /// <summary>
    /// 判断是否兼容值类型
    /// </summary>
    /// <param name="fromType"></param>
    /// <param name="toType"></param>
    /// <returns></returns>
    public static bool CheckValueType(TypeInfo fromType, TypeInfo toType)
#endif
    {
        if (fromType == toType)
            return true;
        if (toType.IsValueType)
        {
            if (fromType.IsValueType && !toType.IsGenericType)
                return toType.IsAssignableFrom(fromType);
            return false;
        }
        if (fromType.IsValueType)
            return false;
        return toType.IsAssignableFrom(fromType);
    }
    #endregion
    #region CheckMethodCallInstance
    /// <summary>
    /// 检查调用委托目标
    /// </summary>
    /// <param name="delegate"></param>
    /// <returns></returns>
    public static Expression CheckMethodCallInstance(Delegate @delegate)
        => CheckMethodCallInstance(@delegate.Target);
    /// <summary>
    /// 检查调用对象
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public static Expression CheckMethodCallInstance(object instance)
    {
        if (instance is null)
            return null;
        return Expression.Constant(instance);
    }
    #endregion
    #region Is
    /// <summary>
    /// 是否可空类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsNullable(Type type)
        => IsGenericTypeDefinition(type, typeof(Nullable<>));
    /// <summary>
    /// 是否泛型定义
    /// </summary>
    /// <param name="type"></param>
    /// <param name="genericType">泛型</param>
    /// <returns></returns>
    public static bool IsGenericTypeDefinition(Type type, Type genericType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => IsGenericTypeDefinition(type.GetTypeInfo(), genericType);
    /// <summary>
    /// 是否泛型定义
    /// </summary>
    /// <param name="typeInfo"></param>
    /// <param name="genericType"></param>
    /// <returns></returns>
    public static bool IsGenericTypeDefinition(TypeInfo typeInfo, Type genericType)
        => typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == genericType;
#else
        => type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
#endif
    #endregion
    #region ConstructorInfo
    /// <summary>
    /// 获取构造函数
    /// </summary>
    /// <param name="declaringType"></param>
    /// <param name="parameterType"></param>
    /// <returns></returns>
    public static ConstructorInfo GetConstructorByParameterType(Type declaringType, Type parameterType)
        => GetConstructor(
            declaringType,
            parameters => parameters.Length == 1
                && CheckValueType(parameterType, parameters[0].ParameterType));
    /// <summary>
    /// 获取构造函数
    /// </summary>
    /// <param name="declaringType"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public static ConstructorInfo GetConstructor(Type declaringType, Func<ParameterInfo[], bool> filter)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var constructors = declaringType.GetTypeInfo().DeclaredConstructors;
#else
        var constructors = declaringType.GetConstructors();
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
    /// <param name="declaringType"></param>
    /// <returns></returns>
    public static IEnumerable<ConstructorInfo> GetConstructors(Type declaringType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
            => GetConstructors(declaringType.GetTypeInfo());
        /// <summary>
        /// 获取所有构造函数
        /// </summary>
        /// <param name="declaringTypeInfo"></param>
        /// <returns></returns>
        public static IEnumerable<ConstructorInfo> GetConstructors(TypeInfo declaringTypeInfo)
            => declaringTypeInfo.DeclaredConstructors;
#else
        => declaringType.GetConstructors();
#endif
    #endregion
    #region MethodInfo
    /// <summary>
    /// 获取函数
    /// </summary>
    /// <param name="type"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public static MethodInfo GetMethod(Type type, Func<MethodInfo, bool> filter)
     => GetMethods(type).FirstOrDefault(filter);
    /// <summary>
    /// 获取所有函数
    /// </summary>
    /// <param name="declaringType"></param>
    /// <returns></returns>
    public static IEnumerable<MethodInfo> GetMethods(Type declaringType)
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        => GetMethods(declaringType.GetTypeInfo());
    /// <summary>
    /// 获取所有函数
    /// </summary>
    /// <param name="declaringTypeInfo"></param>
    /// <returns></returns>
    public static IEnumerable<MethodInfo> GetMethods(TypeInfo declaringTypeInfo)
        => declaringTypeInfo.DeclaredMethods;
#else
        => declaringType.GetMethods();
#endif
    #endregion
}
