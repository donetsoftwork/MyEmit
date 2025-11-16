using Hand.Reflection;
using PocoEmit.Collections.Bundles;
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
    public IEmitCopier ToCollection(in PairTypeKey key)
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
        var container = CollectionContainer.Instance;
        if(!container.CollectionCacher.Validate(destType, out var destBundle))
            return null;
        if(sourceType.IsArray)
            return ArrayToCollection(sourceType, sourceType.GetElementType(), destType, destBundle, clear);
        if(container.DictionaryCacher.Validate(sourceType, out var dictionaryBundle))
            return DictionaryToCollection(sourceType, dictionaryBundle, destType, destBundle, clear);
        if (container.ListCacher.Validate(sourceType, out var listBundle))
            return ListToCollection(sourceType, listBundle, destType, destBundle, clear);
        if (container.EnumerableCacher.Validate(sourceType, out var enumerableBundle))
            return EnumerableToCollection(sourceType, enumerableBundle, destType, destBundle, clear);
        return null;
    }

    ///// <summary>
    ///// 成员转化为集合
    ///// </summary>
    ///// <param name="sourceType"></param>
    ///// <param name="destType"></param>
    ///// <param name="elementType"></param>
    ///// <param name="saver"></param>
    ///// <param name="clear"></param>
    ///// <returns></returns>
    //public CollectionCopier MembersToCollection(Type sourceType, Type destType, Type elementType, IEmitElementSaver saver, bool clear = true)
    //{
    //    var bundle = _options.MemberCacher.Get(sourceType);
    //    if(bundle is not null && MemberElementVisitor.ValidateCollection(_options, bundle, elementType))
    //    {
    //        var elementConverter = _options.GetEmitConverter(elementType, elementType);
    //        MemberElementVisitor visitor = new(_options, bundle, elementType);
    //        return new(sourceType, elementType, destType, elementType, saver, visitor, elementConverter, clear);
    //    }
    //    return null;
    //}
    /// <summary>
    /// 数组转化为集合
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="sourceElementType"></param>
    /// <param name="destType"></param>
    /// <param name="destBundle"></param>
    /// <param name="clear"></param>
    /// <returns></returns>
    public CollectionCopier ArrayToCollection(Type sourceType, Type sourceElementType, Type destType, CollectionBundle destBundle, bool clear = true)
    {
        var sourceVisitor = CollectionContainer.Instance.VisitorCacher.GetByByArray(sourceType);
        if (sourceVisitor is null)
            return null;
        var destElementType = destBundle.ElementType;
        var elementConverter = _options.GetEmitConverter(sourceElementType, destElementType);
        if (elementConverter is null)
            return null;
        var saver = CollectionContainer.Instance.SaveCacher.GetByCollection(destType, destBundle);
        if (saver is null)
            return null;
        return new CollectionCopier(sourceType, sourceElementType, destType, destElementType, saver, sourceVisitor, elementConverter, clear);
    }
    /// <summary>
    /// 字典转集合
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="sourceBundle"></param>
    /// <param name="destType"></param>
    /// <param name="destBundle"></param>
    /// <param name="clear"></param>
    /// <returns></returns>
    public CollectionCopier DictionaryToCollection(Type sourceType, DictionaryBundle sourceBundle, Type destType, CollectionBundle destBundle, bool clear = true)
    {
        var saver = CollectionContainer.Instance.SaveCacher.GetByCollection(destType, destBundle);
        if (saver is null)
            return null;
        var sourceVisitor = CollectionContainer.Instance.VisitorCacher.GetByDictionary(sourceType, sourceBundle);
        if (sourceVisitor is null)
            return null;
        var sourceElementType = sourceBundle.ValueType;
        var destElementType = destBundle.ElementType;
        var elementConverter = _options.GetEmitConverter(sourceElementType, destElementType);
        if (elementConverter is null)
            return null;
        return new CollectionCopier(sourceType, sourceElementType, destType, destElementType, saver, sourceVisitor, elementConverter, clear);
    }
    /// <summary>
    /// 列表转集合
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="sourceBundle"></param>
    /// <param name="destType"></param>
    /// <param name="destBundle"></param>
    /// <param name="clear"></param>
    /// <returns></returns>
    public IEmitCopier ListToCollection(Type sourceType, ListBundle sourceBundle, Type destType, CollectionBundle destBundle, bool clear = true)
    {
        var saver = CollectionContainer.Instance.SaveCacher.GetByCollection(destType, destBundle);
        if (saver is null)
            return null;
        var sourceVisitor = CollectionContainer.Instance.VisitorCacher.GetByList(sourceType, sourceBundle);
        if (sourceVisitor is null)
            return null;
        var sourceElementType = sourceBundle.ElementType;
        var destElementType = destBundle.ElementType;
        var elementConverter = _options.GetEmitConverter(sourceElementType, destElementType);
        if (elementConverter is null)
            return null;
        return new CollectionCopier(sourceType, sourceElementType, destType, destElementType, saver, sourceVisitor, elementConverter, clear);
    }
    /// <summary>
    /// 迭代转集合
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="sourceBundle"></param>
    /// <param name="destType"></param>
    /// <param name="destBundle"></param>
    /// <param name="clear"></param>
    /// <returns></returns>
    public IEmitCopier EnumerableToCollection(Type sourceType, EnumerableBundle sourceBundle, Type destType, CollectionBundle destBundle, bool clear = true)
    {
        var saver = CollectionContainer.Instance.SaveCacher.GetByCollection(destType, destBundle);
        if (saver is null)
            return null;
        var sourceVisitor = CollectionContainer.Instance.VisitorCacher.GetByEnumerable(sourceType, sourceBundle);
        if (sourceVisitor is null)
            return null;
        var sourceElementType = sourceBundle.ElementType;
        var destElementType = destBundle.ElementType;
        var elementConverter = _options.GetEmitConverter(sourceElementType, destElementType);
        if (elementConverter is null)
            return null;
        return new CollectionCopier(sourceType, sourceElementType, destType, destElementType, saver, sourceVisitor, elementConverter, clear);
    }
}
