using Hand.Creational;
using PocoEmit.Builders;
using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
using System.Reflection;
#endif

namespace PocoEmit;

/// <summary>
/// 构造器扩展方法
/// </summary>
public static partial class MapperServices
{
    /// <summary>
    /// 构造委托表达式
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static Expression<Func<TValue>> BuildCreateFunc<TValue>(this IMapper mapper)
    {
        IMapperOptions options = (IMapperOptions)mapper;
        var builder = options.DefaultValueProvider.BuildCore(typeof(TValue));
        if (builder is null)
            return null;
        var expr = builder.Create();
        return expr as Expression<Func<TValue>> ?? Expression.Lambda<Func<TValue>>(expr);
    }
    #region GetCreateFunc
    /// <summary>
    /// 获取构造委托
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static Func<TValue> GetCreateFunc<TValue>(this IMapper mapper)
    {
        IMapperOptions options = (IMapperOptions)mapper;
        var builder = options.DefaultValueProvider.BuildCore(typeof(TValue));
        if (builder is null)
            return null;
        if (builder is ICreator<TValue> creator)
            return creator.Create;
        var expr = builder.Create();
        var lambda = expr as Expression<Func<TValue>> ?? Expression.Lambda<Func<TValue>>(expr);
        return Compiler.Instance.CompileDelegate(lambda);
    }

    #endregion
    /// <summary>
    /// 构造属性(字段)委托表达式
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static Expression<Action<TValue>> BuildCheckAction<TValue>(this IMapper mapper)
    {
        IMapperOptions options = (IMapperOptions)mapper;
        var entity = Expression.Parameter(typeof(TValue), "entity");
        var builder = new VariableBuilder(entity, []);
        options.DefaultValueProvider.CheckMembers(builder, entity);
        if (builder.Count == 0)
            return null;
        ParameterExpression[] parameters = [entity];
        var body = builder.Create(parameters);
        var lambda = Expression.Lambda<Action<TValue>>(body, parameters);
        return lambda;
    }
    /// <summary>
    /// 获取检查属性(字段)委托
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static Action<TValue> GetCheckAction<TValue>(this IMapper mapper)
    {
        IMapperOptions options = (IMapperOptions)mapper;
        var entity = Expression.Parameter(typeof(TValue), "entity");
        var builder = new VariableBuilder(entity, []);
        options.DefaultValueProvider.CheckMembers(builder, entity);
        if (builder.Count == 0)
            return _ => { };
        ParameterExpression[] parameters = [entity];
        var body = builder.Create(parameters);
        var lambda = Expression.Lambda<Action<TValue>>(body, parameters);
        return Compiler.Instance.CompileDelegate(lambda);
    }    
}
