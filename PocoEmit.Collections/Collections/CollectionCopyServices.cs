using PocoEmit.Collections.Copies;
using PocoEmit.Configuration;
using PocoEmit.Members;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Collections;

/// <summary>
/// 成员复制到集合扩展方法
/// </summary>
public static partial class PocoCollectionServices
{
    #region CollectionCopy
    /// <summary>
    /// 复制到集合
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="instance"></param>
    /// <param name="list"></param>
    public static void CollectionCopy<TInstance>(this IMapper mapper, TInstance instance, ICollection<object> list)
    {
        var instanceType = typeof(TInstance);
        var bundle = mapper.MemberCacher.Get(instanceType);
        var members = bundle.ReadMembers;
        foreach (var kv in members)
        {
            var name = kv.Key;
            var member = kv.Value;
            var func = mapper.GetReadFunc(kv.Value);
            if (func == null)
                continue;
            list.Add(func(instance));
        }
    }
    #endregion
    #region GetCollectionCopyAction
    /// <summary>
    /// 获取复制到集合委托
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static Action<TInstance, ICollection<object>> GetCollectionCopyAction<TInstance>(this IMapper mapper)
    {
        var instanceType = typeof(TInstance);
        var bundle = mapper.MemberCacher.Get(instanceType);
        var members = bundle.ReadMembers;
        Action<TInstance, ICollection<object>> actions = null;
        foreach (var kv in members)
        {
            var name = kv.Key;
            var member = kv.Value;
            var func = mapper.GetReadFunc(kv.Value);
            if (func == null)
                continue;
            actions += (instance, list) => list.Add(func(instance));
        }
        return actions ?? ((_, _) => { });
    }
    #endregion
    #region CreatetCollectionCopy
    /// <summary>
    /// 构建复制到集合委托
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TCollection"></typeparam>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static Action<TInstance, TCollection> CreatetCollectionCopy<TInstance, TCollection>(this IMapper mapper)
        where TCollection : class
    {
        return CreateCollectionCopier((IMapperOptions)mapper, typeof(TInstance), typeof(TCollection))
            .CompileAction<TInstance, TCollection>(mapper);
    }
    #endregion
    #region BuildCollectionCopier
    /// <summary>
    /// 构建复制到集合表达式
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TCollection"></typeparam>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static Expression<Action<TInstance, TCollection>> BuildCollectionCopier<TInstance, TCollection>(this IMapper mapper)
        where TCollection : class
    {
        var collectionType = typeof(TCollection);
        return CreateCollectionCopier((IMapperOptions)mapper, typeof(TInstance), collectionType)
            ?.Build<TInstance, TCollection>(mapper);
    }
    #endregion
    #region CreateCollectionCopier
    /// <summary>
    /// 成员复制到集合
    /// </summary>
    /// <param name="options"></param>
    /// <param name="instanceType">实体类型</param>
    /// <param name="collectionType">集合类型</param>
    /// <returns></returns>
    internal static CollectionCopier CreateCollectionCopier(IMapperOptions options, Type instanceType, Type collectionType)
    {
        var elementType = ReflectionHelper.GetElementType(collectionType);
        if (elementType is null)
            return null;
        if (options.CheckPrimitive(instanceType))
            return null;
        var saver = CollectionContainer.Instance.SaveCacher.Get(collectionType, elementType);
        if (saver is null)
            return null;
        var bundle = options.MemberCacher.Get(instanceType);
        if (bundle is not null && MemberElementVisitor.ValidateCollection(options, bundle, elementType))
        {
            var elementConverter = options.GetEmitConverter(elementType, elementType);
            MemberElementVisitor visitor = new(options, bundle, elementType);
            return new(instanceType, elementType, collectionType, elementType, saver, visitor, elementConverter, false);
        }
        return null;
    }
    #endregion
}
