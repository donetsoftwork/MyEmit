using Hand.Reflection;
using PocoEmit.Builders;
using PocoEmit.Collections.Visitors;
using PocoEmit.Complexes;
using PocoEmit.Converters;
using PocoEmit.Copies;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Dictionaries;

/// <summary>
/// 字典复制
/// </summary>
/// <param name="dictionaryType"></param>
/// <param name="keyType"></param>
/// <param name="elementType"></param>
/// <param name="itemProperty"></param>
/// <param name="sourceVisitor"></param>
/// <param name="keyConverter"></param>
/// <param name="elementConverter"></param>
/// <param name="ignoreDefault"></param>
public class DictionaryCopier(Type dictionaryType, Type keyType, Type elementType, PropertyInfo itemProperty, IIndexVisitor sourceVisitor, IEmitConverter keyConverter, IEmitConverter elementConverter, bool ignoreDefault)
    : EmitDictionaryBase(dictionaryType, keyType, elementType)
    , IEmitCopier
{
    #region 配置
    private readonly PropertyInfo _itemProperty = itemProperty;
    private readonly IIndexVisitor _sourceVisitor = sourceVisitor;
    private readonly IEmitConverter _keyConverter = keyConverter;
    private readonly IEmitConverter _elementConverter = elementConverter;
    private readonly bool _ignoreDefault = ignoreDefault;
    /// <summary>
    /// 索引器属性
    /// </summary>
    public PropertyInfo ItemProperty
        => _itemProperty;
    /// <summary>
    /// 数据源遍历
    /// </summary>
    public IIndexVisitor SourceVisitor
        => _sourceVisitor;
    /// <summary>
    /// 键类型转化
    /// </summary>
    public IEmitConverter KeyConverter 
        => _keyConverter;
    /// <summary>
    /// 子元素类型转化
    /// </summary>
    public IEmitConverter ElementConverter
        => _elementConverter;
    /// <summary>
    /// 是否忽略默认值
    /// </summary>
    public bool IgnoreDefault 
        => _ignoreDefault;
    #endregion
    /// <inheritdoc />
    void IComplexPreview.Preview(IComplexBundle parent)
        => parent.Visit(_elementConverter);
    /// <inheritdoc />
    public void BuildAction(IBuildContext context, IEmitBuilder builder, Expression source, Expression dest)
    {
        dest = CheckInstance(dest);
        builder.Add(_sourceVisitor.Travel(builder, source, (k, v) => CopyElement(context, builder, dest, k, v, _keyConverter, _elementConverter)));
    }
    /// <summary>
    /// 复制子元素
    /// </summary>
    /// <param name="context"></param>
    /// <param name="builder"></param>
    /// <param name="dest"></param>
    /// <param name="sourceKey"></param>
    /// <param name="sourceElement"></param>
    /// <param name="keyConverter"></param>
    /// <param name="elementConverter"></param>
    /// <returns></returns>
    public Expression CopyElement(IBuildContext context, IEmitBuilder builder, Expression dest, Expression sourceKey, Expression sourceElement, IEmitConverter keyConverter, IEmitConverter elementConverter)
    {        
        var scope = builder.CreateScope();
        var key = scope.Declare(_keyType, "key");        
        scope.Assign(key, context.Convert(scope, keyConverter, sourceKey));
        var sourceItem = scope.Temp(sourceElement.Type, sourceElement);
        var item = Expression.MakeIndex(dest, _itemProperty, [key]);

        var assignScope = builder.CreateScope();
        var result = context.Convert(assignScope, elementConverter, sourceItem);
        assignScope.Assign(item, result);

        if (PairTypeKey.CheckNullCondition(_keyType))
        {
            scope.IfNotDefault(key, assignScope.Create());
        }
        else
        {
            scope.Add(assignScope.Create());
        }
        return scope.Create();
    }
}
