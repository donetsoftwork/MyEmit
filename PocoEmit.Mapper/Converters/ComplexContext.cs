using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Members;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Converters;

/// <summary>
/// 复杂类型转化上下文
/// </summary>
/// <param name="cacher"></param>
public class ComplexContext(IDictionary<PairTypeKey, LambdaExpression> cacher)
    : DictionaryStorage<PairTypeKey, LambdaExpression>(cacher)
{
    /// <summary>
    /// 复杂类型转化上下文
    /// </summary>
    public ComplexContext()
        :this(new Dictionary<PairTypeKey, LambdaExpression>())
    {
    }
    /// <summary>
    /// 尝试获取
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="expression"></param>
    /// <returns></returns>
    public bool TryGetValue(Type sourceType, Type destType, out LambdaExpression expression)
        => _provider.TryGetValue(new(sourceType, destType), out expression);
    /// <summary>
    /// 转化检查复杂类型
    /// </summary>
    /// <param name="converter"></param>
    /// <param name="source"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public Expression Convert(IEmitConverter converter, Expression source, Type destType)
    {
        if (converter is IEmitComplexConverter complexConverter)
            return complexConverter.Convert(this, source);
        if (TryGetValue(source.Type, destType, out var expression))
            return Expression.Invoke(expression, source);
        return converter.Convert(source);
    }
    /// <summary>
    /// 读取检查复杂类型
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="instance"></param>
    /// <returns></returns>
    public Expression Read(IEmitReader reader, Expression instance)
    {
        if(reader is ConvertValueReader convertReader)
            return Convert(convertReader.Converter, convertReader.Inner.Read(instance), convertReader.ValueType);
        return reader.Read(instance);
    }
}
