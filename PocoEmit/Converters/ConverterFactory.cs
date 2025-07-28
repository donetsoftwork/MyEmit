using PocoEmit.Collections;
using PocoEmit.Configuration;
using System;

namespace PocoEmit.Converters;

/// <summary>
/// 转化器工厂
/// </summary>
public sealed class ConverterFactory(IPocoOptions options)
    : CacheBase<MapTypeKey, IEmitConverter>(options)
{
    #region 配置
    private readonly IPocoOptions _options = options;
    /// <summary>
    /// Emit配置
    /// </summary>
    public IPocoOptions Options
        => _options;
    #endregion
    /// <summary>
    /// 获取转化器
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public IEmitConverter Get(Type sourceType, Type destType)
        => Get(new MapTypeKey(sourceType, destType));
    #region CacheBase
    /// <inheritdoc />
    protected override IEmitConverter CreateNew(MapTypeKey key)
    {
        var builder = _options.ConvertBuilder;
        var sourceType = key.SourceType;
        var destType = key.DestType;
        // 同类型
        if (sourceType == destType)
            return builder.BuildForSelf(destType);
        // 字符串
        if (destType == typeof(string))
            return builder.BuildForString(sourceType);
        // object
        if (destType == typeof(object))
            return builder.BuildForObject(sourceType);
        // 兼容类型
        if (ReflectionHelper.CheckValueType(sourceType, destType))
            return builder.BuildForSelf(destType);
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
            IEmitConverter original = Get(originalKey);
            if (original is null)
                return null;
            // 可空类型
            return builder.BuildForNullable(original, sourceType, key.DestType);
        }
        // 普通类型
        return builder.Build(sourceType, destType);
    }
    ///// <summary>
    ///// 构造Emit类型转化原始方法
    ///// </summary>
    ///// <param name="key"></param>
    ///// <returns></returns>
    //private IEmitConverter CreateCore(MapTypeKey key)
    //{
    //    var destType = key.DestType;
    //    if (_options.Primitive.Get(destType))
    //        return null;
    //    var copier = _options.Copper.Get(key);
    //    if (copier is null)
    //        return null;
    //    var activator = _options.Activator.Get(destType);
    //    if (activator is null)
    //        return null;
    //    return new ComplexTypeConverter(activator, copier);
    //}
    #endregion
    ///// <summary>
    ///// 设置方法来转化
    ///// </summary>
    ///// <param name="method"></param>
    ///// <param name="sourceType"></param>
    ///// <param name="destType"></param>
    ///// <returns></returns>
    //public StaticMethodConverter Set(MethodInfo method, Type sourceType, Type destType)
    //{
    //    var key = new MapTypeKey(sourceType, destType);
    //    var converter = new StaticMethodConverter(method);
    //    Set(key, converter);
    //    return converter;
    //}
}
