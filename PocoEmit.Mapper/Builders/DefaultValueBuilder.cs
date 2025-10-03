using PocoEmit.Configuration;
using PocoEmit.Members;
using System.Linq.Expressions;

namespace PocoEmit.Builders;

/// <summary>
/// 默认值构造器
/// </summary>
public class DefaultValueBuilder(IMapperOptions options)
{
    #region 配置
    /// <summary>
    /// 映射配置
    /// </summary>
    protected readonly IMapperOptions _options = options;
    /// <summary>
    /// 映射配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    #endregion
    /// <summary>
    /// 构造属性默认值
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    public virtual IBuilder<Expression> Build(IEmitMemberWriter member)
    {
        var info = member.Info;
        if (_options.TryGetConfig(info, out var memberBuilder))
            return memberBuilder;
        _options.TryGetConfig(member.ValueType, out var builder);
        return builder;
    }
    /// <summary>
    /// 构造参数默认值
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public virtual IBuilder<Expression> Build(ConstructorParameterMember parameter)
    {
        _options.TryGetConfig(parameter.ValueType, out var builder);
        return builder;
    }
    ///// <summary>
    ///// 获取配置
    ///// </summary>
    ///// <param name="key"></param>
    ///// <param name="builder"></param>
    ///// <returns></returns>
    //protected bool TryGetConfig(Type key, out IBuilder<Expression> builder)
    //    => _options.TryGetConfig(key, out builder);
    ///// <summary>
    ///// 构造属性默认值
    ///// </summary>
    ///// <param name="member"></param>
    ///// <param name="memberType"></param>
    ///// <returns></returns>
    //protected virtual IBuilder<Expression> BuildCore(MemberInfo member, Type memberType)
    //{
    //    TryGetConfig(memberType, out var builder);
    //    return builder;
    //}
    ///// <summary>
    ///// 构造参数默认值
    ///// </summary>
    ///// <param name="parameter"></param>
    ///// <param name="parameterType"></param>
    ///// <returns></returns>
    //protected virtual IBuilder<Expression> BuildCore(ParameterInfo parameter, Type parameterType)
    //{
    //    TryGetConfig(parameterType, out var builder);
    //    return builder;
    //}
}
