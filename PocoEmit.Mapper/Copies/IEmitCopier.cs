using PocoEmit.Builders;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using System.Linq.Expressions;

namespace PocoEmit.Copies;

/// <summary>
/// Emit类型复制器
/// </summary>
public interface IEmitCopier
    : IComplexPreview
    , ICompileInfo
{
    ///// <summary>
    ///// 复制
    ///// </summary>
    ///// <param name="context"></param>
    ///// <param name="source"></param>
    ///// <param name="dest"></param>
    ///// <returns></returns>
    //IEnumerable<Expression> Copy(IBuildContext context, Expression source, Expression dest);
    ///// <summary>
    ///// 复制
    ///// </summary>
    ///// <param name="source"></param>
    ///// <param name="dest"></param>
    ///// <returns></returns>
    //IEnumerable<Expression> Copy(Expression source, Expression dest);
    /// <summary>
    /// 转化核心方法
    /// </summary>
    /// <param name="context"></param>
    /// <param name="builder"></param>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    void BuildAction(IBuildContext context, ComplexBuilder builder, Expression source, Expression dest);
}
