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
public class ConvertContext(IPool<ConvertContext> pool)
    : IConvertContext
{
    #region 配置
    private readonly IPool<ConvertContext> _pool = pool;
    private readonly Dictionary<ConvertCacheKey, object> _cacher = [];
    /// <summary>
    /// 回收池
    /// </summary>
    public IPool<ConvertContext> Pool 
        => _pool;
    #endregion
    #region 功能
    /// <summary>
    /// 转化
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="converter"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public TDest Convert<TSource, TDest>(IContextConverter converter, TSource source)
    {
        var cacheKey = new ConvertCacheKey(source, typeof(TDest));
        if (_cacher.TryGetValue(cacheKey, out var cached))
            return (TDest)cached;
        return converter.Convert<TSource, TDest>(this, source);
    }
    /// <summary>
    /// 设置缓存
    /// </summary>
    /// <param name="source"></param>
    /// <param name="dest"></param>
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
        => Manager.Default.Get();
    /// <summary>
    /// 创建单一类型转化上下文
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <returns></returns>
    public static SingleContext<TSource, TDest> CreateSingle<TSource, TDest>()
        => SingleContext<TSource, TDest>.Manager.Default.Get();
    #endregion
    #region Expression
    /// <summary>
    /// 构造参数
    /// </summary>
    /// <returns></returns>
    public static ParameterExpression CreateParameter()
        => Expression.Parameter(typeof(IConvertContext), "context");
    /// <summary>
    /// 参数赋值初始化
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static Expression InitParameter(ParameterExpression context)
        => Expression.Assign(context, Expression.Call(null, CreateMethod));
    /// <summary>
    /// 单一类型参数赋值初始化
    /// </summary>
    /// <param name="context"></param>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public static Expression InitSingleParameter(ParameterExpression context, Type sourceType, Type destType)
        => Expression.Assign(context, Expression.Call(null, _createSingleMethod.MakeGenericMethod(sourceType, destType)));
    /// <summary>
    /// 调用转化
    /// </summary>
    /// <param name="context"></param>
    /// <param name="key"></param>
    /// <param name="converter"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static Expression CallConvert(ParameterExpression context, PairTypeKey key, Expression converter, Expression source)
        => Expression.Call(context, _convertMethod.MakeGenericMethod(key.LeftType, key.RightType), converter, source);
    /// <summary>
    /// 调用设置缓存
    /// </summary>
    /// <param name="context"></param>
    /// <param name="key"></param>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    public static Expression CallSetCache(ParameterExpression context, PairTypeKey key, Expression source, Expression dest)
        => Expression.Call(context, _setCacheMethod.MakeGenericMethod(key.LeftType, key.RightType), source, dest);
    #endregion
    #region Reflection
    /// <summary>
    /// 反射Convert方法
    /// </summary>
    private static readonly MethodInfo _convertMethod = EmitHelper.GetMethodInfo<IConvertContext, int>(context => context.Convert<int,  int>(null, 0))
        .GetGenericMethodDefinition();
    /// <summary>
    /// 反射SetCache方法
    /// </summary>
    private static readonly MethodInfo _setCacheMethod = EmitHelper.GetActionMethodInfo<IConvertContext>(context => context.SetCache(1, 1))
        .GetGenericMethodDefinition();
    /// <summary>
    /// 反射Create方法
    /// </summary>
    public static readonly MethodInfo CreateMethod = EmitHelper.GetMethodInfo(() => Create());
    /// <summary>
    /// 反射CreateSingle方法
    /// </summary>
    private static readonly MethodInfo _createSingleMethod = EmitHelper.GetMethodInfo(() => CreateSingle<int, int>())
        .GetGenericMethodDefinition();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public static MethodInfo GetCreateSingleMethod(Type sourceType, Type destType)
        => _createSingleMethod.MakeGenericMethod(sourceType, destType);
    #endregion
    /// <summary>
    /// 转换上下文管理器
    /// </summary>
    class Manager()
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
        /// <summary>
        /// 单例
        /// </summary>
        public static readonly Manager Default = new();
    }
}
