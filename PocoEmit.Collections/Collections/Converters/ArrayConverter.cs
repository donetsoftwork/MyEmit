using Hand.Creational;
using Hand.Reflection;
using PocoEmit.Builders;
using PocoEmit.Collections.Activators;
using PocoEmit.Collections.Counters;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Converters;

/// <summary>
/// 数组转数组
/// </summary>
/// <param name="options"></param>
/// <param name="sourceType"></param>
/// <param name="sourceElementType"></param>
/// <param name="destType"></param>
/// <param name="destElementType"></param>
/// <param name="elementConverter"></param>
public class ArrayConverter(IMapperOptions options, Type sourceType, Type sourceElementType, Type destType, Type destElementType, IEmitConverter elementConverter)
    : ArrayActivator(destType, destElementType, ArrayLength.Length)
    , IEmitComplexConverter
    , ICreator<LambdaExpression>
{
    #region 配置
    private readonly IMapperOptions _options = options;
    private readonly PairTypeKey _key = new(sourceType, destType);
    private readonly Type _sourceType = sourceType;
    private readonly Type _sourceElementType = sourceElementType;
    private readonly IEmitConverter _elementConverter = elementConverter;
    /// <inheritdoc />
    public PairTypeKey Key
        => _key;
    /// <summary>
    /// 源类型
    /// </summary>
    public Type SourceType
        => _sourceType;
    /// <summary>
    /// 源子元素类型
    /// </summary>
    public Type SourceElementType
        => _sourceElementType;
    /// <summary>
    /// 子元素转化
    /// </summary>
    public IEmitConverter ElementConverter
        => _elementConverter;
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
    {
        var dest = builder.Declare(_collectionType, "dest");
        var count = builder.Declare<int>("count");
        var index = builder.Declare<int>("index");
        var sourceItem = builder.Temp(_sourceElementType);

        builder.Assign(count, Expression.ArrayLength(source));
        builder.Assign(index, Expression.Constant(0));
        builder.Assign(dest, New(count));
        var cache = context.SetCache(convertContext, _key, source, dest);
        if (cache is not null)
            builder.Add(cache);
        builder.Add(EmitHelper.For(index, count, i => CopyElement(context, builder, source, dest, i, sourceItem, _elementConverter)));
        return dest;
    }
    /// <summary>
    /// 复制子元素
    /// </summary>
    /// <param name="context"></param>
    /// <param name="builder"></param>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <param name="index"></param>
    /// <param name="sourceItem"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    public static Expression CopyElement(IBuildContext context, IEmitBuilder builder, Expression source, Expression dest, Expression index, ParameterExpression sourceItem, IEmitConverter converter)
    {
        var scope = builder.CreateScope(sourceItem);
        scope.Assign(Expression.ArrayIndex(source, index));
        var reslut = context.Convert(scope, converter, sourceItem);
        scope.Assign(Expression.ArrayAccess(dest, index), reslut);
        return scope.Create();
    }
    /// <inheritdoc />
    void IComplexPreview.Preview(IComplexBundle parent)
    {
        var bundle = parent.Accept(_key, this, true);
        if (bundle is null)
            return;
        bundle.Visit(_elementConverter);
    }
}
