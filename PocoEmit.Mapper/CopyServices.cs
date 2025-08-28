using PocoEmit.Builders;
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
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static IPocoCopier<TSource, TDest> GetCopier<TSource, TDest>(this IMapper mapper)
        where TDest : class
    {
        var sourceType = typeof(TSource);
        var destType = typeof(TDest);
        var key = new PairTypeKey(sourceType, destType);
        var emitCopier = mapper.GetEmitCopier(key);
        if (emitCopier is null)
            return null;
        if (emitCopier.Compiled && emitCopier is ICompiledCopier<TSource, TDest> compiled)
            return compiled;
        var compiledCopier = new CompiledCopier<TSource, TDest>(emitCopier, Compile<TSource, TDest>(emitCopier));
        mapper.Set(key, compiledCopier);
        return compiledCopier;
    }
    #endregion
    #region GetObjectCopier
    /// <summary>
    /// 获取弱类型复制器(IObjectCopier)
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public static object GetObjectCopier(this IMapper mapper, Type sourceType, Type destType)
    {
        var key = new PairTypeKey(sourceType, destType);
        var copier = mapper.GetEmitCopier(key);
        if (copier is null)
            return null;
        if (copier.Compiled)
            return copier;
        var compiled = Inner.Compile(sourceType, destType, copier) as IEmitCopier;
        if (compiled != null)
            mapper.Set(key, compiled);
        return compiled;
    }
    #endregion
    #region GetCopyAction
    /// <summary>
    /// 获取复制委托
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static Action<TSource, TDest> GetCopyAction<TSource, TDest>(this IMapper mapper)
        where TDest : class
    {
        var sourceType = typeof(TSource);
        var destType = typeof(TDest);
        var key = new PairTypeKey(sourceType, destType);
        var emitCopier = mapper.GetEmitCopier(key);
        if (emitCopier is null)
            return null;
        if (emitCopier.Compiled && emitCopier is ICompiledCopier<TSource, TDest> compiled)
            return compiled.CopyAction;
        var copyAction = Compile<TSource, TDest>(emitCopier);
        mapper.Set(key, new CompiledCopier<TSource, TDest>(emitCopier, copyAction));
        return copyAction;
    }
    #endregion
    #region GetEmitCopier
    /// <summary>
    /// 获取Emit类型复制器
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public static IEmitCopier GetEmitCopier(this IMapper mapper, Type sourceType, Type destType)
        => mapper.GetEmitCopier(new(sourceType, destType));
    #endregion
    #region Copy
    /// <summary>
    /// 复制
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void Copy<TSource, TDest>(this IMapper mapper, TSource source, TDest dest)
        where TDest : class
    {
        var copyAction = mapper.GetCopyAction<TSource, TDest>()
            ?? throw new InvalidOperationException($"不支持复制的类型：{typeof(TSource).FullName} -> {typeof(TDest).FullName}");
        copyAction(source, dest);
    }
    #endregion
    #region Build
    /// <summary>
    /// 编译转换委托
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static Expression<Action<TSource, TDest>> BuildCopier<TSource, TDest>(this IMapper mapper)
        where TDest : class
    {
        return mapper.GetEmitCopier(typeof(TSource), typeof(TDest))
            .Build<TSource, TDest>();
    }
    /// <summary>
    /// 转换委托
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="emit"></param>
    /// <returns></returns>
    public static Expression<Action<TSource, TDest>> Build<TSource, TDest>(this IEmitCopier emit)
        where TDest : class
    {
        var source = Expression.Parameter(typeof(TSource), "source");
        var dest = Expression.Parameter(typeof(TDest), "dest");
        var list = emit.Copy(source, dest).ToArray();
        var body = list.Length == 1 ? list[0] : Expression.Block(list);
        return Expression.Lambda<Action<TSource, TDest>>(body, source, dest);
    }
    #endregion
    /// <summary>
    /// 编译复制器
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="copier"></param>
    /// <returns></returns>
    public static Action<TSource, TDest> Compile<TSource, TDest>(IEmitCopier copier)
        where TDest : class
        => Compiler._instance.CompileAction(copier.Build<TSource, TDest>());
    /// <summary>
    /// 内部延迟初始化
    /// </summary>
    class Inner
    {
        /// <summary>
        /// 反射Compile方法
        /// </summary>
        private static readonly MethodInfo CompileMethod = EmitHelper.GetActionMethodInfo<IEmitCopier>(emit => Compile<long, object>(emit))
            .GetGenericMethodDefinition();
        /// <summary>
        /// 反射调用编译方法
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="destType"></param>
        /// <param name="copier"></param>
        /// <returns></returns>
        internal static object Compile(Type sourceType, Type destType, IEmitCopier copier)
        {
            return CompileMethod.MakeGenericMethod(sourceType, destType)
                .Invoke(null, [copier]);
        }
    }
}
