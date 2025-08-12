using PocoEmit.Activators;
using PocoEmit.Configuration;
using PocoEmit.Copies;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// 复合类型转化器
/// </summary>
/// <param name="destActivator"></param>
/// <param name="copier"></param>
public class ComplexTypeConverter(IEmitActivator destActivator, IEmitCopier copier)
    : IEmitConverter
{
    #region 配置
    private readonly IEmitActivator _destActivator = destActivator;
    private readonly IEmitCopier _copier = copier;
    /// <summary>
    /// 激活映射目标
    /// </summary>
    public IEmitActivator DestActivator 
        => _destActivator;
    /// <summary>
    /// 复制
    /// </summary>
    public IEmitCopier Copier 
        => _copier;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public Expression Convert(Expression source)
    {
        var destype = _destActivator.ReturnType;
        var destTarget = Expression.Label(destype, "returndest");
        var dest = Expression.Variable(destype, "dest");
        var assign = Expression.Assign(dest, _destActivator.New(source));
        var list = new List<Expression>() { assign };
         if(_copier is not null)
            list.AddRange(_copier.Copy(source, dest));
        //list.Add(Expression.Return(destTarget, dest));     
        list.Add(Expression.Label(destTarget, dest));
        return Expression.Block([dest], list);
    }
}
