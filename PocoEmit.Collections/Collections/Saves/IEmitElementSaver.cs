using PocoEmit.Configuration;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Saves;

/// <summary>
/// 集合元素保存器
/// </summary>
public interface IEmitElementSaver
    : IEmitCollection, ICompileInfo
{
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    Expression Add(Expression instance, Expression value);
}
