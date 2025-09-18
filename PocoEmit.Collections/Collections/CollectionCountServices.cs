using PocoEmit.Builders;
using PocoEmit.Collections.Counters;
using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Collections;

/// <summary>
/// 获取集合数量扩展方法
/// </summary>
public static partial class PocoEmitCollectionServices
{
    #region GetCountFunc
    /// <summary>
    /// 子元素数量获取器
    /// </summary>
    /// <typeparam name="TCollection">集合类型</typeparam>
    /// <param name="container">集合容器</param>
    /// <returns></returns>
    public static Func<TCollection, int> GetCountFunc<TCollection>(this CollectionContainer container)
        => GetCountFuncCore<TCollection>(container, ReflectionHelper.GetElementType(typeof(TCollection)));
    /// <summary>
    /// 子元素数量获取器
    /// </summary>
    /// <typeparam name="TCollection">集合类型</typeparam>
    /// <typeparam name="TElement">子元素类型</typeparam>
    /// <param name="container">集合容器</param>
    /// <returns></returns>
    public static Func<TCollection, int> GetCountFunc<TCollection, TElement>(this CollectionContainer container)
        => GetCountFuncCore<TCollection>(container, typeof(TElement));
    /// <summary>
    /// 子元素数量获取器
    /// </summary>
    /// <typeparam name="TCollection"></typeparam>
    /// <param name="container"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    private static Func<TCollection, int> GetCountFuncCore<TCollection>(CollectionContainer container, Type elementType)
    {
        var key = new PairTypeKey(typeof(TCollection), elementType);
        var emitCounter = container.CountCacher.Get(key);
        if (emitCounter is null)
            return null;
        if (emitCounter.Compiled && emitCounter is ICompiledCounter<TCollection> compiledCounter)
            return compiledCounter.CountFunc;
        var counterFunc = Compile<TCollection>(emitCounter);
        container.CountCacher.Set(key, new CompiledCounter<TCollection>(emitCounter, counterFunc));
        return counterFunc;
    }
    #endregion
    #region GetCounter
    /// <summary>
    /// 子元素数量获取器
    /// </summary>
    /// <typeparam name="TCollection">集合类型</typeparam>
    /// <param name="container">集合容器</param>
    /// <returns></returns>
    public static ICounter<TCollection> GetCounter<TCollection>(this CollectionContainer container)
        => GetCounterCore<TCollection>(container, ReflectionHelper.GetElementType(typeof(TCollection)));
    /// <summary>
    /// 子元素数量获取器
    /// </summary>
    /// <typeparam name="TCollection">集合类型</typeparam>
    /// <typeparam name="TElement">子元素类型</typeparam>
    /// <param name="container">集合容器</param>
    /// <returns></returns>
    public static ICounter<TCollection> GetCounter<TCollection, TElement>(this CollectionContainer container)
        => GetCounterCore<TCollection>(container, typeof(TElement));
    /// <summary>
    /// 子元素数量获取器
    /// </summary>
    /// <typeparam name="TCollection"></typeparam>
    /// <param name="container"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    private static ICounter<TCollection> GetCounterCore<TCollection>(CollectionContainer container, Type elementType)
    {
        var key = new PairTypeKey(typeof(TCollection), elementType);
        var emitCounter = container.CountCacher.Get(key);
        if (emitCounter is null)
            return null;
        if (emitCounter.Compiled && emitCounter is ICompiledCounter<TCollection> compiledCounter)
            return compiledCounter;
        container.CountCacher.Set(key, compiledCounter = new CompiledCounter<TCollection>(emitCounter, Compile<TCollection>(emitCounter)));
        return compiledCounter;
    }
    #endregion
    #region Count
    /// <summary>
    /// 获取子元素数量
    /// </summary>
    /// <typeparam name="TCollection"></typeparam>
    /// <param name="container"></param>
    /// <param name="collection"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static int Count<TCollection>(this CollectionContainer container, TCollection collection)
    {
        var countFunc = container.GetCountFunc<TCollection>()
            ?? throw new InvalidOperationException($"不支持获取子元素数量：{typeof(TCollection).FullName}");
        return countFunc(collection);
    }
    /// <summary>
    /// 获取子元素数量
    /// </summary>
    /// <typeparam name="TCollection"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="container"></param>
    /// <param name="collection"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static int Count<TCollection, TElement>(this CollectionContainer container, TCollection collection)
    {
        var countFunc = container.GetCountFunc<TCollection, TElement>()
            ?? throw new InvalidOperationException($"不支持获取子元素数量：{typeof(TCollection).FullName} -> {typeof(TElement).FullName}");
        return countFunc(collection);
    }
    #endregion
    #region GetEmitCounter
    /// <summary>
    /// 子元素数量获取器
    /// </summary>
    /// <param name="container">集合容器</param>
    /// <param name="collectionType">集合类型</param>
    /// <returns></returns>
    internal static IEmitElementCounter GetEmitCounter(this CollectionContainer container, Type collectionType)
        => container.CountCacher.Get(collectionType, ReflectionHelper.GetElementType(collectionType));
    /// <summary>
    /// 子元素数量获取器
    /// </summary>
    /// <typeparam name="TCollection">集合类型</typeparam>
    /// <param name="container">集合容器</param>
    /// <returns></returns>
    public static IEmitElementCounter GetEmitCounter<TCollection>(this CollectionContainer container)
        => GetEmitCounter(container, typeof(TCollection));
    /// <summary>
    /// 子元素数量获取器
    /// </summary>
    /// <typeparam name="TCollection">集合类型</typeparam>
    /// <typeparam name="TElement">子元素类型</typeparam>
    /// <param name="container">集合容器</param>
    /// <returns></returns>
    public static IEmitElementCounter GetEmitCounter<TCollection, TElement>(this CollectionContainer container)
        => container.CountCacher.Get(typeof(TCollection), typeof(TElement));
    #endregion
    #region Build
    /// <summary>
    /// 转换委托
    /// </summary>
    /// <typeparam name="TCollection"></typeparam>
    /// <param name="emit"></param>
    /// <returns></returns>
    public static Expression<Func<TCollection, int>> Build<TCollection>(this IEmitElementCounter emit)
    {
        var collection = Expression.Parameter(typeof(TCollection), "collection");
        return Expression.Lambda<Func<TCollection, int>>(emit.Count(collection), collection);
    }
    #endregion
    #region Compile
    /// <summary>
    /// 编译数量获取器
    /// </summary>
    /// <typeparam name="TCollection"></typeparam>
    /// <param name="counter"></param>
    /// <returns></returns>
    public static Func<TCollection, int> Compile<TCollection>(IEmitElementCounter counter)
        => Compiler._instance.CompileDelegate(counter.Build<TCollection>());
    #endregion
}
