using PocoEmit.Builders;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using System.Linq.Expressions;

namespace PocoEmit.Copies;

/// <summary>
/// 委托复制器
/// </summary>
/// <param name="poco"></param>
/// <param name="copyAction"></param>
public class ActionCopier(IPocoOptions poco, LambdaExpression copyAction)
    : ActionCallBuilder(poco, copyAction)
    , IEmitCopier
{
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    /// <inheritdoc />
    public void BuildAction(IBuildContext context, ComplexBuilder builder, Expression source, Expression dest)
        => builder.Add(Call(source, dest));
    /// <inheritdoc />
    void IComplexPreview.Preview(IComplexBundle parent) { }
}