using PocoEmit.Builders;
using PocoEmit.Complexes;
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
    void IComplexPreview.Preview(IComplexBundle parent)
    {
        foreach (var member in _members)
            member.Preview(parent);
    }
    /// <inheritdoc />
    public void BuildAction(IBuildContext context, IEmitBuilder builder, Expression source, Expression dest)
    {
        foreach (var member in _members)
        {
            var sourceMember = builder.Execute(member.SourceReader, source);
            var sourceMemberType = sourceMember.Type;
            if (EmitHelper.CheckComplexSource(sourceMember, _options.CheckPrimitive(sourceMemberType)))
            {
                var sourceMemberVariable = builder.Temp(sourceMemberType, sourceMember);
                builder.Add(member.ConvertMember(context, builder, sourceMemberVariable, dest));
            }
            else
            {
                builder.Add(member.ConvertMember(context, builder, sourceMember, dest));
            }
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
