using PocoEmit.Builders;
using PocoEmit.Configuration;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Copies;

/// <summary>
/// 复制成员
/// </summary>
/// <param name="options"></param>
/// <param name="members"></param>
public class ComplexTypeCopier(IMapperOptions options, IEnumerable<IMemberConverter> members)
    : IEmitCopier
{
    private readonly IMapperOptions _options = options;
    private  IEnumerable<IMemberConverter> _members = members;
    /// <summary>
    /// 映射配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    /// <summary>
    /// 成员
    /// </summary>
    public IEnumerable<IMemberConverter> Members 
        => _members;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    /// <inheritdoc />
    public IEnumerable<Expression> Copy(Expression source, Expression dest)
    {
        int index = 0;
        List<ParameterExpression> variables = [];
        List<Expression> converters = [];
        foreach (var member in _members)
        {
            var sourceMember = member.GetSourceMember(source);
            var sourceMemberType = sourceMember.Type;
            if (EmitHelper.CheckComplexSource(sourceMember, _options.CheckPrimitive(sourceMemberType)))
            {
                var sourceMemberVariable = Expression.Variable(sourceMember.Type, "member" + index++);
                variables.Add(sourceMemberVariable);
                converters.Add(Expression.Assign(sourceMemberVariable, sourceMember));
                converters.Add(member.ConvertMember(sourceMemberVariable, dest));
            }
            else
            {
                converters.Add(member.ConvertMember(sourceMember, dest));
            }
        }
        if(converters.Count > 0)
        {
            yield return Expression.Block(variables, converters);
        }
    }
    /// <summary>
    /// 完成
    /// </summary>
    internal void MembersToArray()
    {        
        _members = [.. _members];// 加速
    }
}
