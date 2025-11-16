using Hand.Cache;
using Hand.Reflection;
using PocoEmit.Collections;
using PocoEmit.Configuration;
using System;
using System.Collections.Generic;

namespace PocoEmit.Copies;

/// <summary>
/// 复制器工厂
/// </summary>
/// <param name="options"></param>
public class CopierFactory(IMapperOptions options)
    : CacheFactoryBase<PairTypeKey, IEmitCopier>(options)
{
    #region 配置
    private readonly IMapperOptions _options = options;
    /// <summary>
    /// Emit配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    #endregion
    #region CacheBase
    /// <inheritdoc />
    protected override IEmitCopier CreateNew(in PairTypeKey key)
    {
        var destType = key.RightType;
        // 数组不支持复制
        if (destType.IsArray)
            return _options.CopierBuilder.ToArray(key);
        if (CheckPrimitive(destType))
            return null;
        var sourceType = key.LeftType;
        // 同类型
        if (sourceType == destType)
            return _options.CopierBuilder.ToSelf(key);
        // 兼容类型
        if (PairTypeKey.CheckValueType(sourceType, destType))
            return _options.CopierBuilder.ToSelf(key);
        if (PairTypeKey.CheckNullable(ref sourceType, ref destType))
        {
            var originalKey = new PairTypeKey(sourceType, destType);
            IEmitCopier original = Get(originalKey);
            if (original is null)
                return null;
            // 可空类型
            return _options.CopierBuilder.ForNullable(original, sourceType, key.RightType);
        }
        if (ReflectionType.HasGenericType(destType, typeof(IDictionary<,>)))
            return _options.CopierBuilder.ToDictionary(key);
        if (ReflectionType.HasGenericType(destType, typeof(IEnumerable<>)))
            return _options.CopierBuilder.ToCollection(key);
        // 普通类型
        return CheckMembers(key, _options.CopierBuilder.Build(key));
    }
    #endregion
    /// <summary>
    /// 是否有自定义检测规则
    /// </summary>
    /// <param name="key"></param>
    /// <param name="original"></param>
    /// <returns></returns>
    private IEmitCopier CheckMembers(in PairTypeKey key, IEmitCopier original)
    {
        var checker = _options.GetCheckMembers(key);
        if(checker is null)
            return original;
        return new CheckEmitCopier(original, checker);
    }
    private bool CheckPrimitive(Type destType)
        => _options.CheckPrimitive(destType) || destType == typeof(object);    
}
