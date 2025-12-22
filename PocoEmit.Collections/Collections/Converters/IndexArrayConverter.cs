using Hand.Creational;
using Hand.Reflection;
using PocoEmit.Builders;
using PocoEmit.Collections.Activators;
using PocoEmit.Collections.Counters;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Indexs;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Converters;

/// <summary>
/// 索引转数组
/// </summary>
/// <param name="options"></param>
/// <param name="sourceType"></param>
/// <param name="sourceElementType"></param>
/// <param name="destType"></param>
/// <param name="destElementType"></param>
/// <param name="length"></param>
/// <param name="indexReader"></param>
/// <param name="elementConverter"></param>
public class IndexArrayConverter(IMapperOptions options, Type sourceType, Type sourceElementType, Type destType, Type destElementType, IEmitElementCounter length, IEmitIndexMemberReader indexReader, IEmitConverter elementConverter)
    : ArrayActivator(destType, destElementType, length)
    , IEmitComplexConverter
    , ICreator<LambdaExpression>
{
    #region 配置
    private readonly IMapperOptions _options = options;
    private readonly PairTypeKey _key = new(sourceType, destType);
    private readonly Type _sourceType = sourceType;
    private readonly Type _sourceElementType = sourceElementType;
    private readonly IEmitIndexMemberReader _indexReader = indexReader;
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
    /// <summary>
    /// 源索引读取器
    /// </summary>
    public IEmitIndexMemberReader IndexReader
        => _indexReader;
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
    public Expression BuildFunc(IBuildContext context, ComplexBuilder builder, Expression source, ParameterExpression convertContext)
    {
        var dest = builder.Declare(_collectionType, "dest");
        var count = builder.Declare<int>("count");
        var index = builder.Declare<int>("index");
        var sourceItem = builder.Temp(_sourceElementType);

        builder.Assign(count, _length.Count(source));
        builder.Assign(index, Expression.Constant(0));
        builder.Assign(dest, New(count));
        var cache = context.SetCache(convertContext, _key, source, dest);
        if (cache is not null)
            builder.Add(cache);
        builder.Add(EmitHelper.For(index, count, i => CopyElement(context, builder, source, dest, i, sourceItem, _indexReader, _elementConverter)));
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
    /// <param name="sourceReader"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    public static Expression CopyElement(IBuildContext context, ComplexBuilder builder, Expression source, Expression dest, Expression index, ParameterExpression sourceItem, IEmitIndexMemberReader sourceReader, IEmitConverter converter)
    {
        //builder.Assign(sourceItem, sourceReader.Read(source, index));
        //return Expression.Assign(Expression.ArrayAccess(dest, index), context.Convert(builder, converter, sourceReader.Read(source, index)));
        var scope = builder.CreateScope(sourceItem);
        scope.Assign(sourceReader.Read(source, index));
        var result = context.Convert(scope, converter, sourceItem);
        scope.Assign(Expression.ArrayAccess(dest, index), result);
        return scope.Create();
    }
}
