using PocoEmit.Enums;
using PocoEmit.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
#else
using System.Runtime.Serialization;
#endif


namespace PocoEmit.Collections;

/// <summary>
/// 字段成员缓存
/// </summary>
/// <param name="cacher"></param>
internal class FieldCacher(ICacher<FieldInfo, FieldAccessor> cacher)
    : CacheBase<FieldInfo, FieldAccessor>(cacher)
{
    #region CacheBase<MemberInfo, IMemberWriter>
    /// <inheritdoc />
    protected override FieldAccessor CreateNew(FieldInfo key)
        => new(key);
    #endregion
}

/// <summary>
/// 属性成员缓存
/// </summary>
/// <param name="cacher"></param>
internal class PropertyCacher(ICacher<PropertyInfo, PropertyAccessor> cacher)
        : CacheBase<PropertyInfo, PropertyAccessor>(cacher)
{
    #region CacheBase<MemberInfo, IMemberReader>
    /// <inheritdoc />
    protected override PropertyAccessor CreateNew(PropertyInfo key)
            => new(key);
    #endregion
}

/// <summary>
/// 枚举缓存
/// </summary>
/// <param name="cacher"></param>
internal class EnumCacher(ICacher<Type, IEnumBundle> cacher)
        : CacheBase<Type, IEnumBundle>(cacher)
{
    #region CacheBase<MemberInfo, IMemberReader>
    /// <inheritdoc />
    protected override IEnumBundle CreateNew(Type key)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var isEnum = key.GetTypeInfo().IsEnum;
#else
        var isEnum = key.IsEnum;
#endif
        if(isEnum)
            return CreateCore(key);
        return null;
    }
    #endregion
    /// <summary>
    /// 原始构建方法
    /// </summary>
    /// <param name="enumType"></param>
    /// <returns></returns>
    private static IEnumBundle CreateCore(Type enumType)
    {
        var underType = Enum.GetUnderlyingType(enumType);
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var hasFlag = false;
#else
        var hasFlag = enumType.IsDefined(typeof(FlagsAttribute), false);
#endif
        var fields = ReflectionHelper.GetStaticFields(enumType).ToArray();
        if (hasFlag)
        {
            FlagEnumBundle flagBundle = new(enumType, underType, fields.Length);
            CheckFields(flagBundle, fields);
            return flagBundle;
        }
        EnumBundle bundle = new(enumType, underType, fields.Length);
        CheckFields(bundle, fields);
        return bundle;
    }

    private static void CheckFields(EnumBundle bundle, FieldInfo[] fields)
    {
        var underType = bundle.UnderType;
        foreach (var field in fields)
            bundle.AddField(CreateField(bundle.EnumType, underType, field));
    }
    private static void CheckFields(FlagEnumBundle bundle, FieldInfo[] fields)
    {
        var underType = bundle.UnderType;
        foreach (var field in fields)
            bundle.AddField(CreateFlagField(bundle.EnumType, underType, field));
    }
    /// <summary>
    /// 构建枚举字段
    /// </summary>
    /// <param name="enumType"></param>
    /// <param name="underType"></param>
    /// <param name="field"></param>
    /// <returns></returns>
    private static FlagEnumField CreateFlagField(Type enumType, Type underType, FieldInfo field)
    {
        var value = field.GetValue(null);
        var under = Convert.ChangeType(value, underType);
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        string member = string.Empty;
#else
        string member = field.GetCustomAttribute<EnumMemberAttribute>()?.Value;
#endif
        var flag = Convert.ToUInt64(under);
        return new FlagEnumField(field.Name, member, Expression.Constant(value, enumType), Expression.Constant(under, underType), flag);
    }
    /// <summary>
    /// 构建枚举字段
    /// </summary>
    /// <param name="enumType"></param>
    /// <param name="underType"></param>
    /// <param name="field"></param>
    /// <returns></returns>
    private static EnumField CreateField(Type enumType, Type underType, FieldInfo field)
    {
        var value = field.GetValue(null);
        var under = Convert.ChangeType(value, underType);
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        string member = string.Empty;
#else
        string member = field.GetCustomAttribute<EnumMemberAttribute>()?.Value;
#endif        
        return new EnumField(field.Name, member, Expression.Constant(value, enumType), Expression.Constant(under, underType));
    }
}
