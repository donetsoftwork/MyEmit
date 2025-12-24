using Hand.Creational;
using Hand.Reflection;
using PocoEmit.Builders;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Copies;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Dictionaries;

/// <summary>
/// 字典转化
/// </summary>
/// <param name="options"></param>
/// <param name="instanceType"></param>
/// <param name="dictionaryType"></param>
/// <param name="keyType"></param>
/// <param name="elementType"></param>
/// <param name="copier"></param>
public class DictionaryConverter(IMapperOptions options, Type instanceType, Type dictionaryType, Type keyType, Type elementType, IEmitCopier copier)
    : EmitDictionaryBase(dictionaryType, keyType, elementType)
    , IEmitComplexConverter
    , ICreator<LambdaExpression>
{
    #region 配置
    private readonly IMapperOptions _options = options;
    private readonly PairTypeKey _key = new(instanceType, dictionaryType);
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
    public Expression BuildFunc(IBuildContext context, IEmitBuilder builder, Expression source, ParameterExpression convertContext)
    {
        var dest = builder.Declare(_collectionType, "dest");
        builder.Assign(dest, Expression.New(_collectionType));
        var cache = context.SetCache(convertContext, _key, source, dest);
        if (cache is not null)
            builder.Add( cache);
        _copier.BuildAction(context, builder, source, dest);
        return dest;
    }
}
