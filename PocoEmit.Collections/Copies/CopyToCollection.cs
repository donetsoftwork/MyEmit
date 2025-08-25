using PocoEmit.Collections.Copies;
using PocoEmit.Configuration;
using System;

namespace PocoEmit.Copies;

/// <summary>
/// 复制数据到集合
/// </summary>
public class CopyToCollection(IMapperOptions options)
{
    #region 配置
    /// <summary>
    /// Emit配置
    /// </summary>
    protected readonly IMapperOptions _options = options;
    /// <summary>
    /// Emit配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    #endregion
    /// <summary>
    /// 复制数据到集合
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public IEmitCopier ToCollection(PairTypeKey key)
        => Create(key.LeftType, key.RightType, true);
    /// <summary>
    /// 构造复制器
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="clear"></param>
    /// <returns></returns>
    public IEmitCopier Create(Type sourceType, Type destType, bool clear = true)
    {
        var destElementType = ReflectionHelper.GetElementType(destType);
        if (destElementType == null)
            return null;
        var container = CollectionContainer.Instance;
        var saver = container.SaveCacher.Get(destType, destElementType);
        if (saver is null)
            return null;
        var sourceElementType = ReflectionHelper.GetElementType(sourceType);
        if (sourceElementType == null)
            return null;
        var elementConverter = _options.GetEmitConverter(sourceElementType, destElementType);
        if (elementConverter is null)
            return null;
        var sourceVisitor = container.GetVisitor(sourceType);
        if (sourceVisitor is null)
            return null;
        return new CollectionCopier(sourceType, sourceElementType, destType, destElementType, saver, sourceVisitor, elementConverter, clear);
    }    
}
