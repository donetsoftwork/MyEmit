using Hand.Creational;
using PocoEmit.Builders;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace PocoEmit.ServiceProvider.Builders;

/// <summary>
/// 构造定位器
/// </summary>
/// <param name="providerType"></param>
/// <param name="provider"></param>
/// <param name="builder"></param>
public class ProviderBuilder(Type providerType, Expression provider, EmitBuilder builder)
    : ICreator<Expression>
{
    #region 配置
    private readonly Type _providerType = providerType;
    private readonly Expression _provider = provider;
    private readonly EmitBuilder _builder = CheckProvider(provider, builder);
    /// <summary>
    /// 定位器类型
    /// </summary>
    public Type ProviderType
        => _providerType;
    /// <summary>
    /// 定位器
    /// </summary>
    public Expression Provider
        => _provider;
    /// <summary>
    /// 构造器
    /// </summary>
    public EmitBuilder Builder
        => _builder;
    #endregion
    #region IBuilder<Expression>
    /// <inheritdoc />
    public Expression Create()
        => _builder.Create();
    #endregion
    /// <summary>
    /// 检查定位器表达式是否已存在于构造器的表达式列表中，若不存在则添加该表达式。
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static EmitBuilder CheckProvider(Expression provider, EmitBuilder builder)
    {
        var last = builder.Expressions.LastOrDefault();
        if (last is null || last.Type != provider.Type)
            builder.Add(provider);
        return builder;
    }
}
