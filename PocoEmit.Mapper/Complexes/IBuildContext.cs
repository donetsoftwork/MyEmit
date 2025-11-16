using Hand.Reflection;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System.Linq.Expressions;

namespace PocoEmit.Complexes;

/// <summary>
/// 构建器上下文
/// </summary>
public interface IBuildContext
{
    /// <summary>
    /// 构建器上下文
    /// </summary>
    BuildContext Context { get; }
    /// <summary>
    /// 复杂类型成员信息
    /// </summary>
    IComplexBundle Bundle { get; }
    /// <summary>
    /// 是否缓存
    /// </summary>
    bool HasCache { get; }
    /// <summary>
    /// 执行上下文
    /// </summary>
    ParameterExpression ConvertContextParameter { get; }
    /// <summary>
    /// 获取复杂类型成员信息
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    ComplexBundle GetBundle(in PairTypeKey key);
    /// <summary>
    /// 调用
    /// </summary>
    /// <param name="isCicle"></param>
    /// <param name="lambda"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    Expression Call(bool isCicle, LambdaExpression lambda, params Expression[] arguments);
    /// <summary>
    /// 赋值初始化执行上线文
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Expression InitContext(ParameterExpression context);
    /// <summary>
    /// 获取构建结果
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    ICompiledConverter GetAchieve(in PairTypeKey key);
    /// <summary>
    /// 获取上下文构建结果
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    IEmitContextConverter GetContexAchieve(in PairTypeKey key);
}
