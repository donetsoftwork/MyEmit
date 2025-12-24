using Hand.Reflection;
using PocoEmit.Builders;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Copies;
using PocoEmit.Visitors;
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
        var compiledCopier = CompileCopier<TSource, TDest>(emitCopier, mapper);
        mapper.Save(key, compiledCopier);
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
        var compiled = Inner.Compile(sourceType, destType, copier, mapper) as IEmitCopier;
        if (compiled != null)
            mapper.Save(key, compiled);
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
        var copyAction = CompileAction<TSource, TDest>(emitCopier, mapper);
        mapper.Save(key, new CompiledCopier<TSource, TDest>(emitCopier, copyAction));
        return copyAction;
    }
    #endregion
    #region GetEmitCopier
    /// <summary>
    /// 获取Emit类型复制器
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static IEmitCopier GetEmitCopier<TSource, TDest>(this IMapper mapper)
        => mapper.GetEmitCopier(new(typeof(TSource), typeof(TDest)));
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
        return mapper.GetEmitCopier(new(typeof(TSource), typeof(TDest)))
            .Build<TSource, TDest>(mapper);
    }
    /// <summary>
    /// 转换委托
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="emit"></param>
    /// <returns></returns>
    public static Expression<Action<TSource, TDest>> Build<TSource, TDest>(this IEmitCopier emit, IMapper mapper)
        where TDest : class
    {
        //var sourceType = typeof(TSource);
        //var destType = typeof(TDest);
        var source = Expression.Parameter(typeof(TSource), "source");
        var dest = Expression.Parameter(typeof(TDest), "dest");
        var options = (IMapperOptions)mapper;
        var context = BuildContext.WithPrepare(options, emit)
            .Enter(new PairTypeKey(typeof(TSource), typeof(TDest)));

        ParameterExpression[] parameters = [source, dest];
        var builder = new ArgumentBuilder(source);

        var convertContextParameter = context.ConvertContextParameter;
        Expression body;
        if (convertContextParameter is null)
        {
            emit.BuildAction(context, builder, source, dest);
            body = builder.CreateAction(parameters);
        }
        else
        {
            builder.AddVariable(convertContextParameter);
            builder.Add(context.InitContext(convertContextParameter));
            emit.BuildAction(context, builder, source, dest);
            builder.Add(EmitDispose.Dispose(convertContextParameter));
            body = builder.CreateAction(parameters);
        }
        return Expression.Lambda<Action<TSource, TDest>>(body, parameters);
    }
    #endregion
    /// <summary>
    /// 编译复制器
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="copier"></param>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static Action<TSource, TDest> CompileAction<TSource, TDest>(this IEmitCopier copier, IMapper mapper)
        where TDest : class
        => Compiler._instance.CompileDelegate(copier.Build<TSource, TDest>(mapper));
    /// <summary>
    /// 编译
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="copier"></param>
    /// <param name="mapper"></param>
    /// <returns></returns>
    internal static CompiledCopier<TSource, TDest> CompileCopier<TSource, TDest>(this IEmitCopier copier, IMapper mapper)
         where TDest : class
        => new(copier, CompileAction<TSource, TDest>(copier, mapper));
    /// <summary>
    /// 内部延迟初始化
    /// </summary>
    class Inner
    {
        /// <summary>
        /// 反射Compile方法
        /// </summary>
        private static readonly MethodInfo CompileMethod = EmitHelper.GetActionMethodInfo<IEmitCopier, IMapper>((emit, mapper) => CompileCopier<long, object>(emit, mapper))
            .GetGenericMethodDefinition();
        /// <summary>
        /// 反射调用编译方法
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="destType"></param>
        /// <param name="copier"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        internal static object Compile(Type sourceType, Type destType, IEmitCopier copier, IMapper mapper)
        {
            return CompileMethod.MakeGenericMethod(sourceType, destType)
                .Invoke(null, [copier, mapper]);
        }
    }
}
