using PocoEmit.Collections.Converters;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

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
    #region CreateDictionaryConvertFunc
    /// <summary>
    /// 构建复制到字典委托
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    /// <typeparam name="TDictionary"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="names">成员名</param>
    /// <param name="ignoreDefault">是否忽略默认值</param>
    /// <returns></returns>
    public static Func<TInstance, TDictionary> CreateDictionaryConvertFunc<TInstance, TTarget, TDictionary>(this IMapper mapper, IEnumerable<string> names = null, bool ignoreDefault = true)
        where TDictionary : class
    {
        return CreateDictionaryConverter((IMapperOptions)mapper, typeof(TInstance), typeof(TTarget), typeof(TDictionary), names, ignoreDefault)
            ?.CompileFunc<TInstance, TDictionary>();
    }
    /// <summary>
    /// 构建复制到字典委托
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TDictionary"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="names">成员名</param>
    /// <param name="ignoreDefault">是否忽略默认值</param>
    /// <returns></returns>
    public static Func<TInstance, TDictionary> CreateDictionaryConvertFunc<TInstance, TDictionary>(this IMapper mapper, IEnumerable<string> names = null, bool ignoreDefault = true)
        where TDictionary : class
    {
        return CreateDictionaryConverter((IMapperOptions)mapper, typeof(TInstance), null, typeof(TDictionary), names, ignoreDefault)
            ?.CompileFunc<TInstance, TDictionary>();
    }
    #endregion
    #region BuildDictionaryConverter
    /// <summary>
    /// 构建复制到字典表达式
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    /// <typeparam name="TDictionary"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="names">成员名</param>
    /// <param name="ignoreDefault">是否忽略默认值</param>
    /// <returns></returns>
    public static Expression<Func<TInstance, TDictionary>> BuildDictionaryConverter<TInstance, TTarget, TDictionary>(this IMapper mapper, IEnumerable<string> names = null, bool ignoreDefault = true)
        where TDictionary : class
    {
        return CreateDictionaryConverter((IMapperOptions)mapper, typeof(TInstance), typeof(TTarget), typeof(TDictionary), names, ignoreDefault)
            ?.Build<TInstance, TDictionary>();
    }
    /// <summary>
    /// 构建复制到字典表达式
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TDictionary"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="names">成员名</param>
    /// <param name="ignoreDefault">是否忽略默认值</param>
    /// <returns></returns>
    public static Expression<Func<TInstance, TDictionary>> BuildDictionaryConverter<TInstance, TDictionary>(this IMapper mapper, IEnumerable<string> names = null, bool ignoreDefault = true)
        where TDictionary : class
    {
        return CreateDictionaryConverter((IMapperOptions)mapper, typeof(TInstance), null, typeof(TDictionary), names, ignoreDefault)
            ?.Build<TInstance, TDictionary>();
    }
    #endregion
    #region CreateDictionaryConverter
    /// <summary>
    /// 成员转化为字典
    /// </summary>
    /// <param name="options"></param>
    /// <param name="instanceType">实体类型</param>
    /// <param name="targetType"></param>
    /// <param name="dictionaryType">字典类型</param>
    /// <param name="names">成员名</param>
    /// <param name="ignoreDefault">是否忽略默认值</param>
    /// <returns></returns>
    internal static WrapConverter CreateDictionaryConverter(this IMapperOptions options, Type instanceType, Type targetType, Type dictionaryType, IEnumerable<string> names, bool ignoreDefault)
    {
        if (options.CheckPrimitive(instanceType) || options.CheckPrimitive(dictionaryType))
            return null;
        var bundle = CollectionContainer.Instance.DictionaryCacher.Get(dictionaryType);
        if (bundle == null)
            return null;
        var dictionaryConverter = CreateDictionaryConverter(options, instanceType, targetType, dictionaryType, bundle.KeyType, bundle.ValueType, bundle.Items, names, ignoreDefault);
        return new(options, instanceType, dictionaryType, dictionaryConverter);
    }
    /// <summary>
    /// 成员转化为字典
    /// </summary>
    /// <param name="options"></param>
    /// <param name="instanceType">实体类型</param>
    /// <param name="targetType"></param>
    /// <param name="dictionaryType">字典类型</param>
    /// <param name="keyType">键类型</param>
    /// <param name="elementType">值类型</param>
    /// <param name="itemProperty">索引器属性</param>
    /// <param name="names">成员名</param>
    /// <param name="ignoreDefault">是否忽略默认值</param>
    /// <returns></returns>
    internal static DictionaryConverter CreateDictionaryConverter(this IMapperOptions options, Type instanceType, Type targetType, Type dictionaryType, Type keyType, Type elementType, PropertyInfo itemProperty, IEnumerable<string> names, bool ignoreDefault)
    {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var isInterface = dictionaryType.GetTypeInfo().IsInterface;
#else
        var isInterface = dictionaryType.IsInterface;
#endif
        if (isInterface)
            dictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, elementType);
        targetType ??= elementType;
        DictionaryCopier copier = options.CreateDictionaryCopier(instanceType, targetType, dictionaryType, keyType, elementType, itemProperty, names, ignoreDefault);
        if(copier is null)
            return null;
        return new(instanceType, dictionaryType, keyType, elementType, copier);
    }
    #endregion
}
