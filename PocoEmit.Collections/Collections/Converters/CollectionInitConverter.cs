using Hand.Creational;
using Hand.Reflection;
using PocoEmit.Builders;
using PocoEmit.Collections.Saves;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
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
    , ICreator<LambdaExpression>
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
    {
        var dest = builder.Declare(_collectionType, "dest");
        builder.Assign(dest, Expression.ListInit(Expression.New(_collectionType), Expression.ElementInit(_saver.AddMethod, CheckElement(context, builder, source))));
        var cache = context.SetCache(convertContext, _key, source, dest);
        if (cache is not null)
            builder.Add(cache);
        return dest;
    }
    /// <summary>
    /// 检查子元素
    /// </summary>
    /// <param name="context"></param>
    /// <param name="builder"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    private Expression CheckElement(IBuildContext context, IEmitBuilder builder, Expression source)
        => context.Convert(builder, _elementConverter, source);
}
