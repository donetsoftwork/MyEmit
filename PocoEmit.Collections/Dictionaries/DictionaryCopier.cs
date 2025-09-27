using PocoEmit.Builders;
using PocoEmit.Collections.Visitors;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Copies;
using System;
using System.Collections.Generic;
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
    public IEnumerable<Expression> Copy(IBuildContext context, Expression source, Expression dest)
    {
        dest = CheckInstance(dest);
        yield return _sourceVisitor.Travel(source, (k, v) => CopyElement(context, dest, k, v, _keyConverter, _elementConverter));
    }
    /// <summary>
    /// 复制子元素
    /// </summary>
    /// <param name="context"></param>
    /// <param name="dest"></param>
    /// <param name="key"></param>
    /// <param name="element"></param>
    /// <param name="keyConverter"></param>
    /// <param name="elementConverter"></param>
    /// <returns></returns>
    public Expression CopyElement(IBuildContext context, Expression dest, Expression key, Expression element, IEmitConverter keyConverter, IEmitConverter elementConverter)
    {
        if (_ignoreDefault)
        {
            var elementType = element.Type;
            var elementConvert = context.Convert(elementConverter, element);
            if (EmitHelper.CheckComplexSource(element, false))
            {
                var value0 = Expression.Parameter(elementType, "value0");
                return Expression.Block([value0],
                    Expression.Assign(value0, element),
                    Expression.IfThen(Expression.NotEqual(element, Expression.Default(elementType)), Expression.Assign(Expression.MakeIndex(dest, _itemProperty, [context.Convert(keyConverter, key)]), context.Convert(elementConverter, value0)))
                );
            }
            else
            {
                return Expression.IfThen(Expression.NotEqual(element, Expression.Default(elementType)), Expression.Assign(Expression.MakeIndex(dest, _itemProperty, [context.Convert(keyConverter, key)]), context.Convert(elementConverter, element)));
            }                
        }
        return Expression.Assign(Expression.MakeIndex(dest, _itemProperty, [context.Convert(keyConverter, key)]), context.Convert(elementConverter, element));
    }
}
