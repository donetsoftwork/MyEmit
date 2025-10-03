using Microsoft.AspNetCore.Mvc;
using PocoEmit.Builders;
using PocoEmit.Collections;
using PocoEmit.Members;
using System.Linq.Expressions;

namespace PocoEmit.Mvc.Cachers;

/// <summary>
/// Mvc属性默认值解析缓存
/// </summary>
/// <param name="builder"></param>
public class MvcMemberExpressionCacher(MvcDefaultValueBuilder builder)
    : CacheBase<IEmitMemberWriter, IBuilder<Expression>>(builder)
{
    #region 配置
    private readonly MvcDefaultValueBuilder _builder = builder;
    /// <summary>
    /// 默认值构造器
    /// </summary>
    public MvcDefaultValueBuilder Builder
        => _builder;
    #endregion
    /// <inheritdoc />
    protected override IBuilder<Expression> CreateNew(in IEmitMemberWriter key)
    {
        var member = key.Info;
        var memberType = key.ValueType;
        // 从配置获取
        if(_builder.TryGetConfig(member, out var serviceBuilder)
            || _builder.TryGetConfig(memberType, out serviceBuilder))
            return serviceBuilder;
        if (member.IsDefined(typeof(FromServicesAttribute), false))
            return _builder.BuildFromServices(memberType);
        return null;
    }
}
