using PocoEmit.Builders;
using PocoEmit.Collections;
using PocoEmit.Collections.Bundles;
using PocoEmit.Collections.Visitors;
using PocoEmit.Configuration;
using PocoEmit.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Members;

/// <summary>
/// 成员索引遍历(作为字典)
/// </summary>
/// <param name="options"></param>
/// <param name="bundle"></param>
/// <param name="elementType"></param>
/// <param name="names"></param>
public class MemberIndexVisitor(IMapperOptions options, MemberBundle bundle, Type elementType, IEnumerable<string> names = null)
    : IElementIndexVisitor
{
    #region 配置
    private readonly IMapperOptions _options = options;
    private readonly MemberBundle _bundle = bundle;
    private readonly Type _elementType = elementType;
    private readonly IEnumerable<string> _names = names ?? bundle.EmitReaders.Keys;
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
    /// <summary>
    /// 成员名
    /// </summary>
    public IEnumerable<string> Names
        => _names;
    /// <inheritdoc />
    Type IElementIndexVisitor.KeyType
        => typeof(string);
    #endregion
    Expression IIndexVisitor.Travel(Expression instance, Func<Expression, Expression, Expression> callback)
        => TravelIndex(_options, string.Empty, _bundle, _names, instance, _elementType, callback);
    /// <summary>
    /// 索引遍历
    /// </summary>
    /// <param name="options"></param>
    /// <param name="prefix"></param>
    /// <param name="bundle"></param>
    /// <param name="names"></param>
    /// <param name="instance"></param>
    /// <param name="elementType"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static Expression TravelIndex(IMapperOptions options, string prefix, MemberBundle bundle, IEnumerable<string> names, Expression instance, Type elementType, Func<Expression, Expression, Expression> callback)
    {
        var members = bundle.EmitReaders;
        var variables = new List<ParameterExpression>();
        var expressions = new List<Expression>(members.Count);
        TravelMembers(prefix, members, names, options, instance, variables, expressions, elementType, callback);
        return Expression.Block(variables, expressions);
    }
    /// <summary>
    /// 遍历成员
    /// </summary>
    /// <param name="prefix"></param>
    /// <param name="members"></param>
    /// <param name="names"></param>
    /// <param name="options"></param>
    /// <param name="instance"></param>
    /// <param name="variables"></param>
    /// <param name="expressions"></param>
    /// <param name="elementType"></param>
    /// <param name="callback"></param>
    public static void TravelMembers(string prefix, IDictionary<string, IEmitMemberReader> members, IEnumerable<string> names, IMapperOptions options, Expression instance, List<ParameterExpression> variables, List<Expression> expressions, Type elementType, Func<Expression, Expression, Expression> callback)
    {
        var dictionaryCacher = CollectionContainer.Instance.DictionaryCacher;
        foreach (var name in names)
        {
            var reader = members[name];
            if (reader == null)
                continue;
            string fullName = prefix + name;
            var itemType = reader.ValueType;
            if (PairTypeKey.CheckValueType(itemType, elementType))
            {
                expressions.Add(callback(Expression.Constant(fullName), reader.Read(instance)));
            }
            else if (elementType == typeof(object))
            {
                expressions.Add(callback(Expression.Constant(fullName), Expression.Convert(reader.Read(instance), typeof(object))));
            }
            else if (ReflectionHelper.HasGenericType(itemType, typeof(IDictionary<,>)))
            {
                TravelDictionary(fullName, instance, variables, expressions, itemType, dictionaryCacher.Get(itemType), elementType, reader, callback);
            }
            else if (itemType.IsArray
                || ReflectionHelper.HasGenericType(itemType, typeof(IEnumerable<>))
                || options.CheckPrimitive(itemType))
            {
                continue;
            }
            else
            {
                var itemMembers = options.MemberCacher.Get(itemType).EmitReaders;
                var count = itemMembers.Count;
                if (count == 0)
                    continue;
                List<ParameterExpression> itemVariables = [];
                List<Expression> itemExpressions = new(count);
                var item = Expression.Parameter(itemType, name);
                TravelMembers(fullName, itemMembers, itemMembers.Keys, options, item, itemVariables, itemExpressions, elementType, callback);
                if (itemExpressions.Count > 0)
                {
                    variables.Add(item);
                    expressions.Add(Expression.Assign(item, reader.Read(instance)));
                    variables.AddRange(itemVariables);
                    expressions.AddRange(itemExpressions);
                }
            }
        }
    }
    /// <summary>
    /// 验证是否支持字典
    /// </summary>
    /// <param name="options"></param>
    /// <param name="bundle"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    public static bool ValidateDictionary(IMapperOptions options, MemberBundle bundle, Type elementType)
        => ValidateDictionary(options, [], bundle, elementType);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <param name="bundles"></param>
    /// <param name="bundle"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    public static bool ValidateDictionary(IMapperOptions options, HashSet<MemberBundle> bundles, MemberBundle bundle, Type elementType)
    {
        if(bundles.Contains(bundle))
            return false;
        bundles.Add(bundle);
        if (elementType == typeof(object))
            return true;
        var dictionaryCacher = CollectionContainer.Instance.DictionaryCacher;
        foreach (var member in bundle.EmitReaders.Values)
        {
            var itemType = member.ValueType;
            if (PairTypeKey.CheckValueType(member.ValueType, elementType))
                return true;
            else if (ReflectionHelper.HasGenericType(itemType, typeof(IDictionary<,>)))
                return ValidateDictionaryType(dictionaryCacher.Get(itemType), elementType);

            else if (itemType.IsArray
                || ReflectionHelper.HasGenericType(itemType, typeof(IEnumerable<>))
                || options.CheckPrimitive(itemType))
                continue;
            else if (ValidateDictionary(options, bundles, options.MemberCacher.Get(itemType), elementType))
                return true;
        }
        return false;
    }
    /// <summary>
    /// 验证字典类型是否支持
    /// </summary>
    /// <param name="bundle"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    public static bool ValidateDictionaryType(DictionaryBundle bundle, Type elementType)
        => bundle is not null && PairTypeKey.CheckValueType(bundle.KeyType, elementType);
    /// <summary>
    /// 遍历字典
    /// </summary>
    /// <param name="prefix"></param>
    /// <param name="parent"></param>
    /// <param name="variables"></param>
    /// <param name="expressions"></param>
    /// <param name="dictionaryType"></param>
    /// <param name="bundle"></param>
    /// <param name="elementType"></param>
    /// <param name="dicReader"></param>
    /// <param name="callback"></param>
    public static void TravelDictionary(string prefix, Expression parent, List<ParameterExpression> variables, List<Expression> expressions, Type dictionaryType, DictionaryBundle bundle, Type elementType, IEmitMemberReader dicReader, Func<Expression, Expression, Expression> callback)
    {
        if (bundle is null)
            return;
        var dicElementType = bundle.ValueType;
        if (!PairTypeKey.CheckValueType(dicElementType, elementType))
            return;
        var dic = Expression.Parameter(dictionaryType, "dic");
        variables.Add(dic);
        expressions.Add(Expression.Assign(dic, dicReader.Read(parent)));
        expressions.Add(DictionaryIndexVisitor.Travel(dic, bundle.Keys, bundle.Items, (k, v) => callback(Expression.Call(null, _concatMethod, Expression.Constant(prefix), k), v)));
    }
    /// <summary>
    /// string.Concat
    /// </summary>

    private static MethodInfo _concatMethod = EmitHelper.GetMethodInfo<string>(() => string.Concat("prefix", "Item"));
}
