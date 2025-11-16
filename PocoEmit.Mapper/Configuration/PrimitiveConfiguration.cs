using Hand.Cache;
using Hand.Reflection;
using PocoEmit.Collections;
using System;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
using System.Reflection;
#endif

namespace PocoEmit.Configuration;

/// <summary>
/// 基础类型配置
/// </summary>
public class PrimitiveConfiguration
    : CacheFactoryBase<Type, bool>
{
    /// <summary>
    /// 基础类型配置
    /// </summary>
    /// <param name="settings"></param>
    public PrimitiveConfiguration(ICacher<Type, bool> settings)
        : base(settings)
    {
        Save(typeof(decimal), true);
        Save(typeof(DateTime), true);
        Save(typeof(DateTimeOffset), true);
        Save(typeof(string), true);
        Save(typeof(Guid), true);
        Save(typeof(Type), true);
    }
    #region CacheBase<Type, bool>
    /// <inheritdoc />
    public override bool Get(in Type key)
    {
        if(IsPrimitive(key))
            return true;
        return base.Get(key);
    }
    /// <inheritdoc />
    protected override bool CreateNew(in Type key)
    {
        if (ReflectionType.IsNullable(key))
        {
            var originalType = Nullable.GetUnderlyingType(key);
            return Get(originalType);
        }
        return false;
    }
    #endregion
    /// <summary>
    /// 判断是否为基础类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsPrimitive(Type type)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var typeInfo = type.GetTypeInfo();
        return typeInfo.IsPrimitive || typeInfo.IsEnum;
#else
        return type.IsPrimitive || type.IsEnum;
#endif
    }
}
