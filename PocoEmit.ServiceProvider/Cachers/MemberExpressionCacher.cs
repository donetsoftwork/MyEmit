using Hand.Cache;
using Hand.Creational;
using PocoEmit.Builders;
using PocoEmit.Collections;
using PocoEmit.Members;
using System.Linq.Expressions;

namespace PocoEmit.ServiceProvider.Cachers;

/// <summary>
/// 服务默认值解析缓存
/// </summary>
/// <param name="builder"></param>
public class MemberExpressionCacher(ServiceDefaultValueBuilder builder)
    : CacheFactoryBase<IEmitMemberWriter, ICreator<Expression>>(builder)
{
    #region 配置
    private readonly ServiceDefaultValueBuilder _builder = builder;
    /// <summary>
    /// 默认值构造器
    /// </summary>
    public ServiceDefaultValueBuilder Builder
        => _builder;
    #endregion
    /// <inheritdoc />
    protected override ICreator<Expression> CreateNew(in IEmitMemberWriter key)
    {
        // 从配置获取
        if(_builder.TryGetConfig(key.Info, out var serviceBuilder)
            || _builder.TryGetConfig(key.ValueType, out serviceBuilder))
            return serviceBuilder;
        return null;
    }
}
