using PocoEmit.Builders;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Resolves;

/// <summary>
/// 转化执行上下文
/// </summary>
public sealed class ConvertContext(IPool<ConvertContext> pool)
    : IConvertContext
{
    #region 配置
    private readonly IPool<ConvertContext> _pool = pool;
    private readonly Dictionary<ConvertCacheKey, object> _cacher = [];
    #endregion
    #region 功能
    /// <inheritdoc />
    public bool TryGetCache<TSource, TDest>(TSource source, out TDest dest)
    {
        if (_cacher.TryGetValue(new ConvertCacheKey(source, typeof(TDest)), out var cached))
        {
            if (cached is TDest t)
                dest = t;
            else
                dest = default;
            return true;
        }
        dest = default;
        return false;
    }
    /// <inheritdoc />
    public void SetCache<TSource, TDest>(TSource source, TDest dest)
        => _cacher[new ConvertCacheKey(source, typeof(TDest))] = dest;
    /// <summary>
    /// 清空
    /// </summary>
    public void Clear()
        => _cacher.Clear();
    /// <summary>
    /// 收回
    /// </summary>
    public void Dispose()
        => _pool.Return(this);
    /// <summary>
    /// 创建转化上下文
    /// </summary>
    /// <returns></returns>
    public static ConvertContext Create()
        => Pool.Get();
    /// <summary>
    /// 创建单一类型转化上下文
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <returns></returns>
    public static SingleContext<TSource, TDest> CreateSingle<TSource, TDest>()
        => SingleContext<TSource, TDest>.Pool.Get();
    #endregion
    #region Expression
    /// <summary>
    /// 构造参数
    /// </summary>
    /// <returns></returns>
    public static ParameterExpression CreateParameter()
        => Expression.Parameter(typeof(IConvertContext), "context");
    /// <summary>
    /// 调用获取缓存
    /// </summary>
    /// <param name="context"></param>
    /// <param name="key"></param>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    public static Expression CallTryGetCache(ParameterExpression context, in PairTypeKey key, Expression source, Expression dest)
        => Expression.Call(context, _tryGetCacheMethod.MakeGenericMethod(key.LeftType, key.RightType), source, dest);
    /// <summary>
    /// 调用设置缓存
    /// </summary>
    /// <param name="context"></param>
    /// <param name="key"></param>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    public static Expression CallSetCache(ParameterExpression context, in PairTypeKey key, Expression source, Expression dest)
        => Expression.Call(context, _setCacheMethod.MakeGenericMethod(key.LeftType, key.RightType), source, dest);
    #endregion
    #region Reflection
    /// <summary>
    /// 反射Convert方法
    /// </summary>
    private static readonly MethodInfo _convertMethod = EmitHelper.GetMethodInfo<IContextConverter, int>(context => context.Convert<int, int>(null, 0))
        .GetGenericMethodDefinition();
    /// <summary>
    /// 反射Convert方法
    /// </summary>
    private static readonly MethodInfo _tryGetCacheMethod = ReflectionHelper.GetMethod(typeof(IConvertContext), nameof(IConvertContext.TryGetCache));
    /// <summary>
    /// 反射SetCache方法
    /// </summary>
    private static readonly MethodInfo _setCacheMethod = EmitHelper.GetActionMethodInfo<IConvertContext>(context => context.SetCache(1, 1))
        .GetGenericMethodDefinition();
    /// <summary>
    /// 反射Create方法
    /// </summary>
    internal static readonly MethodInfo CreateMethod = EmitHelper.GetMethodInfo(() => Create());
    /// <summary>
    /// 反射CreateSingle方法
    /// </summary>
    private static readonly MethodInfo _createSingleMethod = EmitHelper.GetMethodInfo(() => CreateSingle<int, int>())
        .GetGenericMethodDefinition();
    /// <summary>
    /// 获取泛型Create方法
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    internal static MethodInfo GetCreateSingleMethod(Type sourceType, Type destType)
        => _createSingleMethod.MakeGenericMethod(sourceType, destType);
    #endregion
    #region Pool
    /// <summary>
    /// 池化管理器
    /// </summary>
    public static readonly IPool<ConvertContext> Pool = new PoolManager();
    /// <summary>
    /// 转换上下文管理器
    /// </summary>
    class PoolManager()
        : PoolBase<ConvertContext>(Environment.ProcessorCount, Environment.ProcessorCount << 2)
    {
        /// <inheritdoc />
        protected override ConvertContext CreateNew()
            => new(this);
        /// <inheritdoc />
        protected override bool Clean(ref ConvertContext resource)
        {
            if (CheckMaxSize())
            {
                resource.Clear();
                return true;
            }
            return false;
        }
    }
    #endregion
}
