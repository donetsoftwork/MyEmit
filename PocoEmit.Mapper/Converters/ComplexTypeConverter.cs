using Hand.Creational;
using Hand.Reflection;
using PocoEmit.Activators;
using PocoEmit.Builders;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Copies;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// 复合类型转化器
/// </summary>
/// <param name="options"></param>
/// <param name="key"></param>
/// <param name="destActivator"></param>
/// <param name="copier"></param>
public class ComplexTypeConverter(IMapperOptions options,in PairTypeKey key, IEmitActivator destActivator, IEmitCopier copier)
    : IEmitComplexConverter
    , ICreator<LambdaExpression>
{
    #region 配置
    /// <summary>
    /// Emit配置
    /// </summary>
    protected readonly IMapperOptions _options = options;
    private readonly PairTypeKey _key = key;
    private readonly Type _sourceType = key.LeftType; 
    private readonly Type _destType = key.RightType;
    private readonly IEmitActivator _destActivator = destActivator;
    private readonly IEmitCopier _copier = copier;

    /// <summary>
    /// Emit配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    /// <inheritdoc />
    public PairTypeKey Key
        => _key;
    /// <summary>
    /// 映射源类型
    /// </summary>
    public Type SourceType
        => _sourceType;
    /// <summary>
    /// 目标类型
    /// </summary>
    public Type DestType
        => _destType;
    /// <summary>
    /// 激活映射目标
    /// </summary>
    public IEmitActivator DestActivator 
        => _destActivator;
    /// <summary>
    /// 复制
    /// </summary>
    public IEmitCopier Copier 
        => _copier;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    #region IEmitConverter
    /// <inheritdoc />
    Expression IEmitConverter.Convert(Expression source)
    {
        throw new NotImplementedException();
        //return BuildContext.WithPrepare(_options, this)
        //.Enter(_key)
        //.CallComplexConvert(_key, source);
    }
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
    public Expression BuildFunc(IBuildContext context, ComplexBuilder builder, Expression source, ParameterExpression convertContex)
    {
        var dest = builder.Declare(_destType, "dest");
        builder.Assign(dest, _destActivator.New(context, builder, source));
        var cache = context.SetCache(convertContex, _key, source, dest);
        if (cache is not null)
            builder.Add(cache);
        _copier?.BuildAction(context, builder, source, dest);
        return dest;
    }
    /// <inheritdoc />
    public void Preview(IComplexBundle parent)
    {
        var bundle = parent.Accept(_key, this, false);
        if (bundle is null)
            return;
        if (_destActivator is IComplexPreview previewActivator)
            previewActivator.Preview(bundle);
        _copier?.Preview(bundle);
    }
}
