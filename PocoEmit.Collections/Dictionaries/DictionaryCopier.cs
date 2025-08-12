using PocoEmit.Collections.Visitors;
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
public class DictionaryCopier
    : EmitDictionaryBase
    , IEmitCopier
{
    /// <summary>
    /// 字典复制
    /// </summary>
    /// <param name="dictionaryType"></param>
    /// <param name="keyType"></param>
    /// <param name="elementType"></param>
    /// <param name="sourceVisitor"></param>
    /// <param name="keyConverter"></param>
    /// <param name="elementConverter"></param>
    /// <param name="clear"></param>
    /// <exception cref="ArgumentException"></exception>
    public DictionaryCopier(Type dictionaryType, Type keyType, Type elementType, IIndexVisitor sourceVisitor, IEmitConverter keyConverter, IEmitConverter elementConverter, bool clear = true)
        :base(dictionaryType, keyType, elementType)
    {
        _sourceVisitor = sourceVisitor;
        _keyConverter = keyConverter;
        _elementConverter = elementConverter;
        _itemProperty = GetItemProperty(dictionaryType);
        if (clear)
            _clearMethod = GetClearMethod() ?? throw new ArgumentException($"type '{_collectionType.Name}' does not have Clear.");
    }
    private readonly PropertyInfo _itemProperty;
    private readonly MethodInfo _clearMethod;
    private readonly IIndexVisitor _sourceVisitor;
    private readonly IEmitConverter _keyConverter;
    private readonly IEmitConverter _elementConverter;

    /// <inheritdoc />
    public IEnumerable<Expression> Copy(Expression source, Expression dest)
    {
        yield return dest = CheckInstance(dest);
        if (_clearMethod is not null)
            yield return Expression.Call(dest, _clearMethod);
        yield return _sourceVisitor.Travel(source, (k, v) => CopyElement(dest, k, v, _keyConverter, _elementConverter));
    }
    /// <summary>
    /// 复制子元素
    /// </summary>
    /// <param name="dest"></param>
    /// <param name="key"></param>
    /// <param name="element"></param>
    /// <param name="keyConverter"></param>
    /// <param name="elementConverter"></param>
    /// <returns></returns>
    public Expression CopyElement(Expression dest, Expression key, Expression element, IEmitConverter keyConverter, IEmitConverter elementConverter)
        => Expression.Assign(Expression.MakeIndex(dest, _itemProperty, [keyConverter.Convert(key)]), elementConverter.Convert(element));
    #region MethodInfo
    /// <summary>
    /// 获取清空方法
    /// </summary>
    /// <returns></returns>
    protected virtual MethodInfo GetClearMethod()
        => GetClearMethod(_collectionType);
    /// <summary>
    /// 获取清空方法
    /// </summary>
    /// <param name="collectionType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static MethodInfo GetClearMethod(Type collectionType)
        => ReflectionHelper.GetMethod(collectionType, method => method.Name == "Clear" && method.GetParameters().Length == 0);
    #endregion
}
