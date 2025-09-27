using PocoEmit.Builders;
using PocoEmit.Collections.Saves;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Converters;

/// <summary>
/// 集合直接初始化转化
/// </summary>
/// <param name="options"></param>
/// <param name="collectionType"></param>
/// <param name="elementType"></param>
/// <param name="saver"></param>
/// <param name="elementConverter"></param>
public sealed class CollectionInitConverter(IMapperOptions options, Type collectionType, Type elementType, IEmitElementSaver saver, IEmitConverter elementConverter)
    : EmitCollectionBase(collectionType, elementType)
    , IEmitComplexConverter
    , IBuilder<LambdaExpression>
{
    #region 配置
    private readonly IMapperOptions _options = options;
    private readonly PairTypeKey _key = new(elementType, collectionType);
    private readonly IEmitElementSaver _saver = saver;
    private readonly IEmitConverter _elementConverter = elementConverter;
    /// <inheritdoc />
    public PairTypeKey Key
        => _key;
    /// <summary>
    /// 集合元素保存器
    /// </summary>
    public IEmitElementSaver Saver
        => _saver;
    /// <summary>
    /// 子元素转化
    /// </summary>
    public IEmitConverter ElementConverter
        => _elementConverter;
    #endregion
    /// <inheritdoc />
    void IComplexPreview.Preview(IComplexBundle parent)
    {
        var bundle = parent.Accept(_key, this, true);
        if (bundle is null)
            return;
        bundle.Visit(_elementConverter);
    }
    #region IEmitConverter
    /// <inheritdoc />
    Expression IEmitConverter.Convert(Expression source)
        => BuildContext.WithPrepare(_options, this)
        .Enter(_key)
        .CallComplexConvert(_key, source);
    #endregion
    #region IBuilder<LambdaExpression>
    /// <summary>
    /// 构造表达式
    /// </summary>
    /// <returns></returns>
    public LambdaExpression Build()
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
    public IEnumerable<Expression> BuildBody(IBuildContext context, Expression source, Expression dest, ParameterExpression convertContext)
    {
        yield return Expression.Assign(dest, Expression.ListInit(Expression.New(_collectionType), Expression.ElementInit(_saver.AddMethod, CheckElement(context, source))));
        var cache = context.SetCache(convertContext, _key, source, dest);
        if (cache is not null)
            yield return cache;
    }
    /// <summary>
    /// 检查子元素
    /// </summary>
    /// <param name="context"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    private Expression CheckElement(IBuildContext context, Expression source)
        => context.Convert(_elementConverter, source);
}
