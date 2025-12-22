using Hand.Creational;
using Hand.Reflection;
using PocoEmit.Builders;
using PocoEmit.Collections.Activators;
using PocoEmit.Collections.Counters;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Copies;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Collections.Converters;

/// <summary>
/// 集合类型转化
/// </summary>
/// <param name="options"></param>
/// <param name="sourceType"></param>
/// <param name="collectionType"></param>
/// <param name="elementType"></param>
/// <param name="capacityConstructor"></param>
/// <param name="sourceCount"></param>
/// <param name="copier"></param>
public class CollectionConverter(IMapperOptions options, Type sourceType, Type collectionType, Type elementType, ConstructorInfo capacityConstructor, IEmitCounter sourceCount, IEmitCopier copier)
    : CollectionActivator(collectionType, elementType, capacityConstructor, sourceCount)
    , IEmitComplexConverter
    , ICreator<LambdaExpression>
{
    #region 配置
    private readonly IMapperOptions _options = options;
    private readonly PairTypeKey _key = new(sourceType, collectionType);
    private readonly IEmitCopier _copier = copier;
    /// <inheritdoc />
    public PairTypeKey Key
        => _key;
    /// <summary>
    /// 复制
    /// </summary>
    public IEmitCopier Copier
        => _copier;
    #endregion
    /// <inheritdoc />
    void IComplexPreview.Preview(IComplexBundle parent)
    {
        var bundle = parent.Accept(_key, this, true);
        if (bundle is null)
            return;
        _copier.Preview(bundle);
    }
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
    public Expression BuildFunc(IBuildContext context, ComplexBuilder builder, Expression source, ParameterExpression convertContext)
    {
        var dest = builder.Declare(_collectionType, "dest");
        builder.Assign(dest, New(context, builder, source));
        var cache = context.SetCache(convertContext, _key, source, dest);
        if (cache is not null)
            builder.Add(cache);
        _copier.BuildAction(context, builder, source, dest);
        return dest;
    }
}
