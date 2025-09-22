using PocoEmit.Configuration;
using PocoEmit.Converters;

namespace PocoEmit.Complexes;

/// <summary>
/// 复杂类型成员信息接口
/// </summary>
public interface IComplexBundle
{
    /// <summary>
    /// 复杂类型转化上下文
    /// </summary>
    BuildContext Context { get; }
    /// <summary>
    /// 添加子类型
    /// </summary>
    /// <param name="item"></param>
    /// <param name="converter"></param>
    /// <param name="isCollection"></param>
    /// <returns></returns>
    ComplexBundle Accept(PairTypeKey item, IEmitConverter converter, bool isCollection);
    /// <summary>
    /// 获取转化器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    IEmitConverter GetConverter(PairTypeKey key);
}
