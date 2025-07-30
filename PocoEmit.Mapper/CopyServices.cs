using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Copies;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit;

/// <summary>
/// 复制扩展方法
/// </summary>
public static partial class MapperServices
{
    #region GetCopier
    /// <summary>
    /// 获取复制器
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IPocoCopier<TSource, TDest> GetCopier<TSource, TDest>(this IMapperOptions options)
        where TDest : class
    {
        var sourceType = typeof(TSource);
        var destType = typeof(TDest);
        var key = new MapTypeKey(sourceType, destType);
        var emitCopier = options.CopierFactory.Get(key);
        if (emitCopier is null)
            return null;
        if (emitCopier.Compiled && emitCopier is ICompiledCopier<TSource, TDest> compiled)
            return compiled;
        var compiledCopier = emitCopier.CompileCopier<TSource, TDest>(Expression.Parameter(sourceType, "source"), Expression.Parameter(destType, "dest"));
        options.CopierFactory.Set(key, compiledCopier);
        return compiledCopier;
    }
    #endregion
    #region GetObjectCopier
    /// <summary>
    /// 获取弱类型复制器(IObjectCopier)
    /// </summary>
    /// <param name="options"></param>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public static object GetObjectCopier(this IMapperOptions options, Type sourceType, Type destType)
    {
        var key = new MapTypeKey(sourceType, destType);
        var copier = options.CopierFactory.Get(key);
        if (copier is null)
            return null;
        if (copier.Compiled)
            return copier;
        var compileConverter = Inner.CompileCopierMethod.MakeGenericMethod(sourceType, destType);
        var compiled = compileConverter.Invoke(null, [copier, Expression.Parameter(sourceType, "source"), Expression.Parameter(destType, "dest")]) as IEmitCopier;
        if (compiled != null)
            options.CopierFactory.Set(key, compiled);
        return compiled;
    }
    #endregion
    #region GetCopyAction
    /// <summary>
    /// 获取复制委托
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="options"></param>
    /// <returns></returns>
    public static Action<TSource, TDest> GetCopyAction<TSource, TDest>(this IMapperOptions options)
        where TDest : class
    {
        var sourceType = typeof(TSource);
        var destType = typeof(TDest);
        var key = new MapTypeKey(sourceType, destType);
        var emitCopier = options.CopierFactory.Get(key);
        if (emitCopier is null)
            return null;
        if (emitCopier.Compiled && emitCopier is ICompiledCopier<TSource, TDest> compiled)
            return compiled.CopyAction;
        var copyAction = emitCopier.Compile<TSource, TDest>(Expression.Parameter(sourceType, "source"), Expression.Parameter(destType, "dest"));
        options.CopierFactory.Set(key, new CompiledCopier<TSource, TDest>(emitCopier, copyAction));
        return copyAction;
    }
    #endregion
    #region Copy
    /// <summary>
    /// 复制
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="options"></param>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void Copy<TSource, TDest>(this IMapperOptions options, TSource source, TDest dest)
        where TDest : class
    {
        var copyAction = options.GetCopyAction<TSource, TDest>()
            ?? throw new InvalidOperationException($"不支持复制的类型：{typeof(TSource).FullName} -> {typeof(TDest).FullName}");
        copyAction(source, dest);
    }
    #endregion
    /// <summary>
    /// 编译复制器
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="emit"></param>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    public static CompiledCopier<TSource, TDest> CompileCopier<TSource, TDest>(this IEmitCopier emit, ParameterExpression source, ParameterExpression dest)
        where TDest : class
    {
        var copyFunc = Compile<TSource, TDest>(emit, source, dest);
        var compiledCopier = new CompiledCopier<TSource, TDest>(emit, copyFunc);
        return compiledCopier;
    }
    #region Compile
    /// <summary>
    /// 编译转换委托
    /// </summary>
    /// <param name="emit"></param>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    public static Action<TSource, TDest> Compile<TSource, TDest>(this IEmitCopier emit, ParameterExpression source, ParameterExpression dest)
        where TDest : class
    {
        var list = emit.Copy(source, dest).ToArray();
        var body = list.Length == 1 ? list[0] : Expression.Block(list);
        var lambda = Expression.Lambda<Action<TSource, TDest>>(body, source, dest);
        return lambda.Compile();
    }
    /// <summary>
    /// 编译转换委托
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="emit"></param>
    /// <returns></returns>
    public static Action<TSource, TDest> Compile<TSource, TDest>(this IEmitCopier emit)
        where TDest : class
    {
        ParameterExpression source = Expression.Parameter(typeof(TSource), "source");
        ParameterExpression dest = Expression.Parameter(typeof(TDest), "dest");
        var list = emit.Copy(source, dest).ToArray();
        var body = list.Length == 1 ? list[0] : Expression.Block(list);
        var lambda = Expression.Lambda<Action<TSource, TDest>>(body, source, dest);
        return lambda.Compile();
    }
    #endregion
    #region Copier
    /// <summary>
    /// 设置委托来复制
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="settings"></param>
    /// <param name="copy"></param>
    /// <returns></returns>
    public static DelegateCopier<TSource, TDest> SetCopy<TSource, TDest>(this ISettings<MapTypeKey, IEmitCopier> settings, Action<TSource, TDest> copy)
        where TDest : class
    {
        var key = new MapTypeKey(typeof(TSource), typeof(TDest));
        var copier = new DelegateCopier<TSource, TDest>(copy);
        settings.Set(key, copier);
        return copier;
    }
    /// <summary>
    /// 尝试设置委托来复制不覆盖
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="settings"></param>
    /// <param name="copy"></param>
    /// <returns></returns>
    public static IEmitCopier TrySetCopy<TSource, TDest>(this ISettings<MapTypeKey, IEmitCopier> settings, Action<TSource, TDest> copy)
        where TDest : class
    {
        var key = new MapTypeKey(typeof(TSource), typeof(TDest));
        if (settings.TryGetValue(key, out var value0) && value0 is not null)
            return value0;
        var copier = new DelegateCopier<TSource, TDest>(copy);
        settings.Set(key, copier);
        return copier;
    }
    #endregion
    /// <summary>
    /// 内部延迟初始化
    /// </summary>
    class Inner
    {
        /// <summary>
        /// GetConverter
        /// </summary>
        public static readonly MethodInfo CompileCopierMethod = ReflectionHelper.GetMethod(typeof(MapperServices), m => m.Name == "CompileCopier");
    }
}
