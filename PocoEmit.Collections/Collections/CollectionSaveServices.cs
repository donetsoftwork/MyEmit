using PocoEmit.Builders;
using PocoEmit.Collections.Saves;
using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Collections;

/// <summary>
/// 集合保存元素扩展方法
/// </summary>
public static partial class PocoEmitCollectionServices
{
    #region GetSaveAction
    /// <summary>
    /// 子元素保存委托
    /// </summary>
    /// <typeparam name="TCollection"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="container"></param>
    /// <returns></returns>
    public static Action<TCollection, TElement> GetSaveAction<TCollection, TElement>(this CollectionContainer container)
    {
        var key = new PairTypeKey(typeof(TCollection), typeof(TElement));
        var emitSaver = container.SaveCacher.Get(key);
        if (emitSaver is null)
            return null;
        if (emitSaver.Compiled && emitSaver is ICompiledElementSaver<TCollection, TElement> compiledSaver)
            return compiledSaver.SaveAction;
        var SaverAction = Compile<TCollection, TElement>(emitSaver);
        container.SaveCacher.Set(key, new CompiledElementSaver<TCollection, TElement>(emitSaver, SaverAction));
        return SaverAction;
    }
    #endregion
    #region GetSaver
    /// <summary>
    /// 子元素保存器
    /// </summary>
    /// <typeparam name="TCollection"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="container"></param>
    /// <returns></returns>
    public static ICollectionSaver<TCollection, TElement> GetSaver<TCollection, TElement>(this CollectionContainer container)
    {
        var key = new PairTypeKey(typeof(TCollection), typeof(TElement));
        var emitSaver = container.SaveCacher.Get(key);
        if (emitSaver is null)
            return null;
        if (emitSaver.Compiled && emitSaver is ICompiledElementSaver<TCollection, TElement> compiledSaver)
            return compiledSaver;
        compiledSaver = new CompiledElementSaver<TCollection, TElement>(emitSaver, Compile<TCollection, TElement>(emitSaver));
        container.SaveCacher.Set(key, compiledSaver);
        return compiledSaver;
    }
    #endregion
    #region Save
    /// <summary>
    /// 保存子元素
    /// </summary>
    /// <typeparam name="TCollection"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="container"></param>
    /// <param name="collection"></param>
    /// <param name="item"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void Save<TCollection, TElement>(this CollectionContainer container, TCollection collection, TElement item)
    {
        var saveAction = container.GetSaveAction<TCollection, TElement>()
            ?? throw new InvalidOperationException($"不支持保存子元素：{typeof(TCollection).FullName} -> {typeof(TElement).FullName}");
        saveAction(collection, item);
    }
    #endregion
    #region GetEmitSaver
    /// <summary>
    /// 子元素数量获取器
    /// </summary>
    /// <typeparam name="TCollection">集合类型</typeparam>
    /// <typeparam name="TElement">子元素类型</typeparam>
    /// <param name="container">集合容器</param>
    /// <returns></returns>
    public static IEmitElementSaver GetEmitSaver<TCollection, TElement>(this CollectionContainer container)
        => container.SaveCacher.Get(typeof(TCollection), typeof(TElement));
    #endregion
    #region Build
    /// <summary>
    /// 转换委托
    /// </summary>
    /// <typeparam name="TCollection"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="saver"></param>
    /// <returns></returns>
    public static Expression<Action<TCollection, TElement>> Build<TCollection, TElement>(this IEmitElementSaver saver)
    {
        var collection = Expression.Parameter(typeof(TCollection), "collection");
        var item = Expression.Parameter(typeof(TElement), "item");
        return Expression.Lambda<Action<TCollection, TElement>>(saver.Add(collection, item), collection, item);
    }
    #endregion
    #region Compile
    /// <summary>
    /// 编译元素保存器
    /// </summary>
    /// <typeparam name="TCollection"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="saver"></param>
    /// <returns></returns>
    public static Action<TCollection, TElement> Compile<TCollection, TElement>(IEmitElementSaver saver)
        => Compiler._instance.CompileAction(saver.Build<TCollection, TElement>());
    #endregion
}
