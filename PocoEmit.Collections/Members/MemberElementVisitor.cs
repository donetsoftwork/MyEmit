using Hand.Reflection;
using PocoEmit.Builders;
using PocoEmit.Collections;
using PocoEmit.Collections.Bundles;
using PocoEmit.Collections.Visitors;
using PocoEmit.Configuration;
using PocoEmit.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Members;

/// <summary>
/// 成员元素遍历(作为集合)
/// </summary>
/// <param name="options"></param>
/// <param name="bundle"></param>
/// <param name="elementType"></param>
public class MemberElementVisitor(IMapperOptions options, MemberBundle bundle, Type elementType)
    : IEmitElementVisitor
{
    #region 配置
    private readonly IMapperOptions _options = options;
    private readonly MemberBundle _bundle = bundle;
    private readonly Type _elementType = elementType;
    /// <summary>
    /// 配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    /// <summary>
    /// 成员集合
    /// </summary>
    public MemberBundle Bundle
        => _bundle;
    /// <summary>
    /// 元素类型
    /// </summary>
    public Type ElementType
        => _elementType;
    #endregion   
    /// <summary>
    /// 验证是否支持集合
    /// </summary>
    /// <param name="options"></param>
    /// <param name="bundle"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    public static bool ValidateCollection(IMapperOptions options, MemberBundle bundle, Type elementType)
    {
        if (elementType == typeof(object))
            return true;
        var container = CollectionContainer.Instance;
        foreach (var member in bundle.EmitReaders.Values)
        {
            var itemType = member.ValueType;
            if (PairTypeKey.CheckValueType(itemType, elementType))
            {
                return true;
            }
            else if (itemType.IsArray)
            {
                if (itemType.GetElementType() is Type itemElementType
                    && PairTypeKey.CheckValueType(itemElementType, elementType))
                    return true;
            }
            else if(container.DictionaryCacher.Validate(itemType))
            {
                var itemBundle = container.DictionaryCacher.Get(itemType);
                if(itemBundle is null)
                    continue;
                return PairTypeKey.CheckValueType(itemBundle.ValueType, elementType);

            }
            else if (container.ListCacher.Validate(itemType))
            {
                var itemBundle = container.ListCacher.Get(itemType);
                if (itemBundle is null)
                    continue;
                return PairTypeKey.CheckValueType(itemBundle.ElementType, elementType);
            }
            else if (container.EnumerableCacher.Validate(itemType))
            {
                var itemBundle = container.EnumerableCacher.Get(itemType);
                if (itemBundle is null)
                    continue;
                return PairTypeKey.CheckValueType(itemBundle.ElementType, elementType);
            }
            else
            {
                var itemBundle = options.MemberCacher.Get(itemType);
                if (itemBundle.EmitReaders.Count == 0)
                    continue;
                if (ValidateCollection(options, itemBundle, elementType))
                    return true;
            }
        }
        return false;
    }
    /// <inheritdoc />
    Expression IEmitElementVisitor.Travel(IEmitBuilder builder, Expression instance, Func<Expression, Expression> callback)
        => Travel(_options, builder, _bundle, instance, _elementType, callback);
    /// <summary>
    /// 遍历成员
    /// </summary>
    /// <param name="options"></param>
    /// <param name="builder"></param>
    /// <param name="bundle"></param>
    /// <param name="instance"></param>
    /// <param name="elementType"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static Expression Travel(IMapperOptions options, IEmitBuilder builder, MemberBundle bundle, Expression instance, Type elementType, Func<Expression, Expression> callback)
    {
        var members = bundle.EmitReaders;
        //var variables = new List<ParameterExpression>();
        //var expressions = new List<Expression>(members.Count);
        var container = CollectionContainer.Instance;
        foreach (var kv in members)
        {
            var reader = kv.Value;
            var itemType = reader.ValueType;
            if (PairTypeKey.CheckValueType(itemType, elementType))
            {
                builder.Add(callback(reader.Read(instance)));
            }
            else if (elementType == typeof(object))
            {
                builder.Add(callback(Expression.Convert(reader.Read(instance), typeof(object))));
            }
            else if (itemType.IsArray)
            {
                if (itemType.GetElementType() is Type itemElementType
                    && PairTypeKey.CheckValueType(itemElementType, elementType))
                {
                    var array = builder.Declare(itemType, "array");
                    builder.Assign(array, reader.Read(instance));
                    builder.Add(ArrayVisitor.Travel(builder, array, callback));
                }
            }
            else if(options.CheckPrimitive(itemType))
            {
                continue;
            }
            else if (container.DictionaryCacher.Validate(itemType))
            {
                var itemBundle = container.DictionaryCacher.Get(itemType);
                if (itemBundle is null)
                    continue;
                var dic = builder.Declare(itemType, "dic");
                builder.Assign(dic, reader.Read(instance));
                builder.Add(DictionaryValuesVisitor.Travel(builder, dic, itemBundle.Values, callback));
            }
            else if (container.ListCacher.Validate(itemType))
            {
                var itemBundle = container.ListCacher.Get(itemType);
                if (itemBundle is null)
                    continue;
                var list = builder.Declare(itemType, "list");
                builder.Assign(list, reader.Read(instance));
                builder.Add(ListVisitor.Travel(builder, list, itemBundle.Count, itemBundle.Items, (_, item) => callback(item)));
            }
            else if (container.EnumerableCacher.Validate(itemType))
            {
                var itemBundle = container.EnumerableCacher.Get(itemType);
                if (itemBundle is null)
                    continue;
                var enumerable = builder.Declare(itemType, "enumerable");
                builder.Assign(enumerable, reader.Read(instance));
                builder.Add(EnumerableVisitor.Travel(builder, enumerable, itemBundle, callback));
            }
            else
            {

            }
        }
        //return Expression.Block(expressions);
        return null;
    }
    /// <summary>
    /// 遍历成员
    /// </summary>
    /// <param name="members"></param>
    /// <param name="options"></param>
    /// <param name="builder"></param>
    /// <param name="instance"></param>
    /// <param name="elementType"></param>
    /// <param name="callback"></param>
    public static void TravelMembers(IDictionary<string, IEmitMemberReader> members, IMapperOptions options, IEmitBuilder builder, Expression instance, Type elementType, Func<Expression, Expression> callback)
    {
        var container = CollectionContainer.Instance;
        foreach (var kv in members)
        {
            var reader = kv.Value;
            var itemType = reader.ValueType;
            if (PairTypeKey.CheckValueType(itemType, elementType))
            {
                builder.Add(callback(reader.Read(instance)));
            }
            else if (elementType == typeof(object))
            {
                builder.Add(callback(Expression.Convert(reader.Read(instance), typeof(object))));
            }
            else if (itemType.IsArray)
            {
                var array = builder.Declare(itemType, kv.Key);
                TravelArray(options, builder, itemType, elementType, array, callback);
                MergeItem(instance, builder, reader, array);
            }
            else if (container.DictionaryCacher.Validate(itemType))
            {
                var dictionary = builder.Declare(itemType, kv.Key);
                TravelDictionary(options, builder, elementType, itemType, container.DictionaryCacher.Get(itemType), dictionary, callback);
                MergeItem(instance, builder, reader, dictionary);
            }
            else if (container.ListCacher.Validate(itemType))
            {
                var list = builder.Declare(itemType, kv.Key);
                TravelList(options, builder, itemType, elementType, container.ListCacher.Get(itemType), list, callback);
                MergeItem(instance, builder, reader, list);
            }
            else if (container.EnumerableCacher.Validate(itemType))
            {
                var enumerable = builder.Declare(itemType, kv.Key);
                TravelEnumerable(options, builder, itemType, elementType, container.EnumerableCacher.Get(itemType), enumerable, callback);
                MergeItem(instance, builder, reader, enumerable);
            }
            else if(options.CheckPrimitive(itemType))
            {
                continue;
            }
            else
            {
                var itemMembers = options.MemberCacher.Get(itemType).EmitReaders;
                var count = itemMembers.Count;
                if (count == 0)
                    continue;
                var item = builder.Declare(itemType, kv.Key);
                TravelMembers(itemMembers, options, builder, item, elementType, callback);
                MergeItem(instance, builder, reader, item);
            }
        }
    }
    /// <summary>
    /// 遍历数组
    /// </summary>
    /// <param name="options"></param>
    /// <param name="builder"></param>
    /// <param name="arrayType"></param>
    /// <param name="elementType"></param>
    /// <param name="array"></param>
    /// <param name="callback"></param>
    public static void TravelArray(IMapperOptions options, IEmitBuilder builder, Type arrayType, Type elementType, Expression array, Func<Expression, Expression> callback)
    {
        var valueType = arrayType.GetElementType();
        if (PairTypeKey.CheckValueType(valueType, elementType))
        {
            builder.Add(ArrayVisitor.Travel(builder, array, callback));
        }
        else if (options.CheckPrimitive(valueType))
        {
            return;
        }
        else
        {

        }
    }
    /// <summary>
    /// 遍历字典
    /// </summary>
    /// <param name="options"></param>
    /// <param name="builder"></param>
    /// <param name="elementType"></param>
    /// <param name="dictionaryType"></param>
    /// <param name="bundle"></param>
    /// <param name="dictionary"></param>
    /// <param name="callback"></param>
    public static void TravelDictionary(IMapperOptions options, IEmitBuilder builder, Type elementType, Type dictionaryType, DictionaryBundle bundle, Expression dictionary, Func<Expression, Expression> callback)
    {
        if(bundle is null)
            return;
        var keyType = bundle.KeyType;
        var valueType = bundle.ValueType;
        if (PairTypeKey.CheckValueType(valueType, elementType))
        {
            builder.Add(DictionaryValuesVisitor.Travel(builder, dictionary, bundle.Values, callback));
        }
        else if (options.CheckPrimitive(valueType))
        {
            return;
        }
        else
        {

        }
    }
    /// <summary>
    /// 遍历列表
    /// </summary>
    /// <param name="options"></param>
    /// <param name="builder"></param>
    /// <param name="listType"></param>
    /// <param name="elementType"></param>
    /// <param name="bundle"></param>
    /// <param name="list"></param>
    /// <param name="callback"></param>
    public static void TravelList(IMapperOptions options, IEmitBuilder builder, Type listType, Type elementType, ListBundle bundle, Expression list, Func<Expression, Expression> callback)
    {
        if (bundle is null)
            return;
        var valueType = bundle.ElementType;
        if (PairTypeKey.CheckValueType(valueType, elementType))
        {
            builder.Add(ListVisitor.Travel(builder, list, bundle.Count, bundle.Items, (_, item) => callback(item)));
        }
        else if (options.CheckPrimitive(valueType))
        {
            return;
        }
        else
        {

        }
    }
    /// <summary>
    /// 遍历枚举
    /// </summary>
    /// <param name="options"></param>
    /// <param name="builder"></param>
    /// <param name="enumerableType"></param>
    /// <param name="elementType"></param>
    /// <param name="bundle"></param>
    /// <param name="enumerable"></param>
    /// <param name="callback"></param>
    public static void TravelEnumerable(IMapperOptions options, IEmitBuilder builder, Type enumerableType, Type elementType, EnumerableBundle bundle, Expression enumerable, Func<Expression, Expression> callback)
    {
        if (bundle is null)
            return;
        var valueType = bundle.ElementType;
        if (PairTypeKey.CheckValueType(valueType, elementType))
        {
            builder.Add(EnumerableVisitor.Travel(builder, enumerable, bundle, callback));
        }
        else if (options.CheckPrimitive(valueType))
        {
            return;
        }
        else
        {

        }
    }
    /// <summary>
    /// 合并子表达式(及变量)
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="builder"></param>
    /// <param name="reader"></param>
    /// <param name="item"></param>
    public static void MergeItem(Expression instance, IEmitBuilder builder, IEmitMemberReader reader, ParameterExpression item)
    {
        //if (itemExpressions.Count > itemVariables.Count)
        {
            //builder.AddVariable(item);
            builder.Assign(item, reader.Read(instance));
            //variables.AddRange(itemVariables);
            //expressions.AddRange(itemExpressions);
        }
    }
}
