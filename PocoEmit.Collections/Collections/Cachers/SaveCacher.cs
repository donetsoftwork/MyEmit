using PocoEmit.Collections.Bundles;
using PocoEmit.Collections.Saves;
using PocoEmit.Configuration;
using System;
using System.Reflection;

namespace PocoEmit.Collections.Cachers;

/// <summary>
/// 元素保存器缓存
/// </summary>
/// <param name="container"></param>
internal class SaveCacher(CollectionContainer container)
   : CacheBase<PairTypeKey, IEmitElementSaver>(container)
{
    #region 配置
    private readonly CollectionContainer _container = container;
    /// <summary>
    /// 集合容器
    /// </summary>
    public CollectionContainer Container
        => _container;
    #endregion
    /// <inheritdoc />
    protected override IEmitElementSaver CreateNew(in PairTypeKey key)
    {
        var collectionType = key.LeftType;
        var elementType = key.RightType;
        // 不支持数组
        if (collectionType.IsArray)
            return null;
        return CreateByType(Container, collectionType, elementType);
    }
    /// <summary>
    /// 按类型构造元素保存器
    /// </summary>
    /// <param name="container"></param>
    /// <param name="collectionType"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    public static EmitElementSaver CreateByType(CollectionContainer container, Type collectionType, Type elementType)
    {
        MethodInfo addMethod = null;
        if (container.CollectionCacher.Validate(collectionType, out var bundle) && bundle.ElementType == elementType)
            addMethod = bundle.AddMethod;
        addMethod ??= CollectionContainer.GetAddMethod(collectionType, elementType);
        if (addMethod is null)
            return null;
        return CreateByMethod(addMethod, elementType);
    }
    /// <summary>
    /// 构造集合元素保存
    /// </summary>
    /// <param name="bundle"></param>
    /// <returns></returns>
    private static EmitElementSaver CreateByCollection(CollectionBundle bundle)
    {
        var addMethod = bundle.AddMethod;
        if (addMethod is null)
            return null;
        return CreateByMethod(addMethod, bundle.ElementType);
    }
    /// <summary>
    /// 获取集合元素保存
    /// </summary>
    /// <param name="collectionType"></param>
    /// <param name="bundle"></param>
    /// <returns></returns>
    public IEmitElementSaver GetByCollection(Type collectionType, CollectionBundle bundle)
    {
        var key = new PairTypeKey(collectionType, bundle.ElementType);
        if (TryGetCache(key, out IEmitElementSaver counter))
            return counter;
        Set(key, counter = CreateByCollection(bundle));
        return counter;
    }
    /// <summary>
    /// 按方法保存
    /// </summary>
    /// <param name="addMethod"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    private static EmitElementSaver CreateByMethod(MethodInfo addMethod, Type elementType)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var declaringType = addMethod.DeclaringType;
#else
        var declaringType = addMethod.ReflectedType;
#endif
        return new(declaringType, elementType, addMethod);
    }

}