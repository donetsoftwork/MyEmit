using PocoEmit.Collections.Converters;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Copies;
using System;
using System.Collections.Generic;

namespace PocoEmit.Dictionaries;

/// <summary>
/// 成员转化为字典扩展方法
/// </summary>
public static partial class PocoDictionaryServices
{
    #region ToDictionary
    /// <summary>
    /// 转化为字典
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="instance"></param>
    /// <param name="ignoreNull"></param>
    /// <returns></returns>
    public static IDictionary<string, object> ToDictionary<TInstance>(this IMapper mapper, TInstance instance, bool ignoreNull = true)
    {
        if (instance is IDictionary<string, object> dic0)
            return dic0;
        var instanceType = typeof(TInstance);
        var bundle = mapper.MemberCacher.Get(instanceType);
        var members = bundle.ReadMembers;
        int count = members.Count;
        Dictionary<string, object> dic = new(count);
        if (count == 0)
            return dic;
        foreach (var kv in members)
        {
            var readFunc = mapper.GetReadFunc(kv.Value);
            if (readFunc == null)
                continue;
            var value = readFunc(instance);
            if (ignoreNull && value is null)
                continue;
            dic[kv.Key] = value;
        }
        return dic;
    }
    #endregion
    #region GetToDictionaryFunc
    /// <summary>
    /// 获取转化为字典委托
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="ignoreNull"></param>
    /// <returns></returns>
    public static Func<TInstance, IDictionary<string, object>> GetToDictionaryFunc<TInstance>(this IMapper mapper, bool ignoreNull = true)
    {
        if (PairTypeKey.CheckValueType(typeof(TInstance), typeof(IDictionary<string, object>)))
            return instance => instance as IDictionary<string, object>;
        var instanceType = typeof(TInstance);
        var bundle = mapper.MemberCacher.Get(instanceType);
        Action<Dictionary<string, object>, TInstance> actions = null;
        int index = 0;
        var members = bundle.ReadMembers;
        foreach (var kv in members)
        {
            var readFunc = mapper.GetReadFunc(kv.Value);
            if (readFunc == null)
                continue;
            Action<Dictionary<string, object>, TInstance> action;
            if (ignoreNull)
            {
                action = (dic, instance) =>
                {
                    var value = readFunc(instance);
                    if (value is not null)
                        dic[kv.Key] = value;
                };
            }
            else
            {
                action = (dic, instance) => dic[kv.Key] = readFunc(instance);
            }
            actions += action;
            index++;
        }
        if (actions is null)
            return instance => new Dictionary<string, object>();
        return instance =>
        {
            Dictionary<string, object> dic = new(index);
            actions(dic, instance);
            return dic;
        };
    }
    #endregion
    #region FromDictionary
    /// <summary>
    /// 字典转化为实体
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static TInstance FromDictionary<TInstance>(this IMapper mapper, IDictionary<string, object> source)
        where TInstance : class, new()
    {
        if (source is TInstance instance0)
            return instance0;
        var instanceType = typeof(TInstance);
        var bundle = mapper.MemberCacher.Get(instanceType);
        var instance = new TInstance();
        var members = bundle.EmitWriters;
        int count = members.Count;
        if (count == 0)
            return instance;
        foreach (var kv in members)
        {
            var name = kv.Key;
            var member = kv.Value;
            var writeAction = mapper.GetWriteAction(kv.Value.Info);
            if (writeAction == null)
                continue;
            if (source.TryGetValue(name, out var value) && value is not null)
            {
                var valueType = value.GetType();
                var memberType = member.ValueType;
                if (PairTypeKey.CheckValueType(valueType, memberType))
                {
                    writeAction(instance, value);
                }
                else
                {
                    if (mapper.GetObjectConverter(valueType, memberType) is IObjectConverter converter)
                        writeAction(instance, converter.ConvertObject(value));
                }
            }
        }
        return instance;
    }
    #endregion
    #region GetFromDictionaryFunc
    /// <summary>
    /// 获取字典转化为实体委托
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static Func<IDictionary<string, object>, TInstance> GetFromDictionaryFunc<TInstance>(this IMapper mapper)
        where TInstance : class, new()
    {
        if (PairTypeKey.CheckValueType(typeof(IDictionary<string, object>), typeof(TInstance)))
            return dic => dic as TInstance;
        var instanceType = typeof(TInstance);
        var bundle = mapper.MemberCacher.Get(instanceType);
        Action<TInstance, IDictionary<string, object>> actions = null;
        var members = bundle.EmitWriters;
        foreach (var kv in members)
        {
            var name = kv.Key;
            var member = kv.Value;
            var writeAction = mapper.GetWriteAction(kv.Value.Info);
            if (writeAction == null)
                continue;
            actions += (instance, source) =>
            {
                if (source.TryGetValue(name, out var value) && value is not null)
                {
                    var valueType = value.GetType();
                    var memberType = member.ValueType;
                    if (PairTypeKey.CheckValueType(valueType, memberType))
                    {
                        writeAction(instance, value);
                    }
                    else
                    {
                        if (mapper.GetObjectConverter(valueType, memberType) is IObjectConverter converter)
                            writeAction(instance, converter.ConvertObject(value));
                    }
                }
            };
        }
        if (actions is null)
            return instance => new TInstance();
        return dic =>
        {
            var instance = new TInstance();
            actions(instance, dic);
            return instance;
        };
    }
    #endregion
    /// <summary>
    /// 构造字典转化器
    /// </summary>
    /// <param name="options"></param>
    /// <param name="sourceType"></param>
    /// <param name="isInterface"></param>
    /// <param name="dictionaryType"></param>
    /// <param name="keyType"></param>
    /// <param name="elementType"></param>
    /// <param name="copier"></param>
    /// <returns></returns>
    internal static IEmitComplexConverter CreateDictionaryConverter(this IMapperOptions options, Type sourceType, bool isInterface, Type dictionaryType, Type keyType, Type elementType, IEmitCopier copier)
    {
        if (isInterface)
        {
            var implType = typeof(Dictionary<,>).MakeGenericType(keyType, elementType);
            var dictionary = new DictionaryConverter(options, sourceType, implType, keyType, elementType, copier);
            return new WrapConverter(options, sourceType, dictionaryType, dictionary);
        }
        return new DictionaryConverter(options, sourceType, dictionaryType, keyType, elementType, copier);
    }
}
