using Hand.Creational;
using Hand.Reflection;
using Hand.Structural;
using PocoEmit.Builders;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Converters;

/// <summary>
/// 包装转化器
/// </summary>
/// <param name="options"></param>
/// <param name="sourceType"></param>
/// <param name="destType"></param>
/// <param name="original"></param>
public class WrapConverter(IMapperOptions options, Type sourceType, Type destType, IComplexIncludeConverter original)
    : IWrapper<IComplexIncludeConverter>
    , IEmitComplexConverter
    , ICreator<LambdaExpression>
{
    #region 配置
    /// <summary>
    /// Emit配置
    /// </summary>
    private readonly IMapperOptions _options = options;
    private readonly PairTypeKey _key = new(sourceType, destType);
    private readonly IComplexIncludeConverter _original = original;
    /// <summary>
    /// Emit配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    /// <inheritdoc />
    public PairTypeKey Key
        => _key;
    /// <inheritdoc />
    public IComplexIncludeConverter Original
        => _original;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    #region IEmitConverter
    /// <inheritdoc />
    Expression IEmitConverter.Convert(Expression source)
        => throw new NotImplementedException();
    #endregion
    #region IBuilder<LambdaExpression>
    /// <summary>
    /// 构造表达式
    /// </summary>
    /// <returns></returns>
    public LambdaExpression Create()
        => BuildContext.WithPrepare(_options, this)
        .Build(this);
    #endregion
    #region IEmitComplexConverter
    /// <inheritdoc />
    public LambdaExpression Build(IBuildContext context)
        => context.Context.Build(this);
    /// <inheritdoc />
    public LambdaExpression BuildWithContext(IBuildContext context)
        => context.Context.BuildWithContext(this);
    #endregion
    /// <inheritdoc />
    public Expression BuildFunc(IBuildContext context, IEmitBuilder builder, Expression source, ParameterExpression convertContext)
        => _original.BuildFunc(context, builder, source, convertContext);
    /// <inheritdoc />
    void IComplexPreview.Preview(IComplexBundle parent)
    {
        parent.Accept(_key, this, true);
        _original.Preview(parent);
    }
}
