using PocoEmit.Builders;
using PocoEmit.Collections;
using PocoEmit.Configuration;
using System;

namespace PocoEmit.Copies;

/// <summary>
/// 复制器工厂
/// </summary>
public class CopierFactory
    : CacheBase<MapTypeKey, IEmitCopier>
{
    /// <summary>
    /// 复制器工厂
    /// </summary>
    /// <param name="options"></param>
    public CopierFactory(IMapperOptions options)
        : base(options)
    {
        _options = options;
        _builderForSelf = new CopierBuilderForSelf(this);
        _builder = new CopierBuilder(this);
    }
    #region 配置
    private readonly IMapperOptions _options;
    /// <summary>
    /// Emit配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    private CopierBuilderForSelf _builderForSelf;
    private readonly CopierBuilder _builder;
    #endregion
    #region CacheBase
    /// <inheritdoc />
    protected override IEmitCopier CreateNew(MapTypeKey key)
    {
        var destType = key.DestType;
        if(CheckPrimitive(destType))
            return null;
        var sourceType = key.SourceType;
        // 同类型
        if (sourceType == destType)
            return _builderForSelf.Build(key);
        // 兼容类型
        if (ReflectionHelper.CheckValueType(sourceType, destType))
            return _builderForSelf.Build(key);
        bool isNullable = false;
        if (ReflectionHelper.IsNullable(sourceType))
        {
            isNullable = true;
            sourceType = sourceType.GenericTypeArguments[0];
        }
        if (ReflectionHelper.IsNullable(destType))
        {
            isNullable = true;
            destType = destType.GenericTypeArguments[0];
        }
        if (isNullable)
        {
            var originalKey = new MapTypeKey(sourceType, destType);
            IEmitCopier original = Get(originalKey);
            if (original is null)
                return null;
            // 可空类型
            return BuildForNullable(original, sourceType, key.DestType);
        }
        // 普通类型
        return _builder.Build(key);
    }
    #endregion
    /// <summary>
    /// 为可空类型构建复制器
    /// </summary>
    /// <param name="original"></param>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public static IEmitCopier BuildForNullable(IEmitCopier original, Type sourceType, Type destType)
        => new CompatibleCopier(original, sourceType, destType);
    private bool CheckPrimitive(Type destType)
        => _options.CheckPrimitive(destType) || destType == typeof(object);    
}
