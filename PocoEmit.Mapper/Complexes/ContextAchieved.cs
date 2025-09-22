using PocoEmit.Builders;
using PocoEmit.Converters;
using PocoEmit.Resolves;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Complexes;

/// <summary>
/// 上下文构建结果
/// </summary>
public class ContextAchieved
    : IContextConverter
{
    #region 配置
    private LambdaExpression _lambda = null;
    /// <summary>
    /// 表达式
    /// </summary>
    public LambdaExpression Lambda 
        => _lambda;
    private Delegate _func = null;
    /// <summary>
    /// 委托
    /// </summary>
    public Delegate Func
        => _func;
    #endregion
    /// <summary>
    /// 构建
    /// </summary>
    /// <param name="lambda"></param>
    public void Build(LambdaExpression lambda)
    {
        if (lambda is null)
            throw new ArgumentNullException(nameof(lambda));            
        _lambda = lambda;
        _func = Compiler._instance.CompileDelegate(lambda);
    }

    /// <summary>
    /// 转化
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <returns></returns>
    public TDest Convert<TSource, TDest>(IConvertContext context, TSource source)
    {
        if (_func is Func<IConvertContext, TSource, TDest> convertFunc)
            return convertFunc(context, source);
        return default;
    }
    /// <summary>
    /// 构造上下文构建结果
    /// </summary>
    /// <param name="converter"></param>
    /// <returns></returns>
    public static ContextAchieved CreateByConverter(IContextConverter converter)
    {
        if (converter is null)
            return new();
        if(converter is ContextAchieved achieved)
            return achieved;
        else
            return new() { _lambda = converter.Lambda, _func = converter.Func };
    }
}
