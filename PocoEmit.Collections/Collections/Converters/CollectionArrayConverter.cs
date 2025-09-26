using PocoEmit.Builders;
using PocoEmit.Collections.Activators;
using PocoEmit.Collections.Counters;
using PocoEmit.Collections.Visitors;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Collections.Converters;

/// <summary>
/// 集合转数组
/// </summary>
/// <param name="options"></param>
/// <param name="sourceType"></param>
/// <param name="sourceElementType"></param>
/// <param name="destType"></param>
/// <param name="destElementType"></param>
/// <param name="length"></param>
/// <param name="visitor"></param>
/// <param name="elementConverter"></param>
public class CollectionArrayConverter(IMapperOptions options, Type sourceType, Type sourceElementType, Type destType, Type destElementType, IEmitElementCounter length, IEmitElementVisitor visitor, IEmitConverter elementConverter)
    : ArrayActivator(destType, destElementType,  length)
    , IEmitComplexConverter
    , IBuilder<LambdaExpression>
{
    #region 配置
    private readonly IMapperOptions _options = options;
    private readonly PairTypeKey _key = new(sourceType, destType);
    private readonly Type _sourceType = sourceType;
    private readonly Type _sourceElementType = sourceElementType;
    private readonly IEmitElementVisitor _visitor = visitor;
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
    /// 集合访问者
    /// </summary>
    public IEmitElementVisitor Visitor
        => _visitor;
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
        //yield return bundle;
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
        var count = Expression.Variable(typeof(int), "count");
        var index = Expression.Variable(typeof(int), "index");
        var sourceItem = Expression.Variable(_sourceElementType, "sourceItem");

        var list = new List<Expression>() {
            Expression.Assign(count, _length.Count(source)),
            Expression.Assign(index, Expression.Constant(0)),
            Expression.Assign(dest, New(count))
        };
        var cache = context.SetCache(convertContext, _key, source, dest);
        if (cache is not null)
            list.Add(cache);
        yield return Expression.Block(
            [count, index, sourceItem],
            [
                ..list,
                _visitor.Travel(source, item => CopyElement(context, dest, index, item, sourceItem, _elementConverter))
            ]
        );
    }
    /// <summary>
    /// 复制子元素
    /// </summary>
    /// <param name="context"></param>
    /// <param name="dest"></param>
    /// <param name="index"></param>
    /// <param name="item"></param>
    /// <param name="sourceItem"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    public static Expression CopyElement(IBuildContext context, Expression dest, Expression index, Expression item, ParameterExpression sourceItem, IEmitConverter converter)
        => Expression.Block(
            Expression.Assign(sourceItem, item),
            Expression.Assign(Expression.ArrayAccess(dest, Expression.PostIncrementAssign(index)), context.Convert(converter, sourceItem))
            );
}