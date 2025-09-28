using PocoEmit.Complexes;
using PocoEmit.Configuration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Dictionaries;

/// <summary>
/// 成员转化为字典扩展方法
/// </summary>
public static partial class PocoDictionariesServices
{
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
    internal static IEmitComplexConverter CreateDictionaryConverter(this IMapperOptions options, Type instanceType, Type targetType, Type dictionaryType, IEnumerable<string> names, bool ignoreDefault)
    {
        if (options.CheckPrimitive(instanceType) || options.CheckPrimitive(dictionaryType))
            return null;
        var bundle = CollectionContainer.Instance.DictionaryCacher.Get(dictionaryType);
        if (bundle == null)
            return null;
        return CreateDictionaryConverter(options, instanceType, targetType, dictionaryType, bundle.KeyType, bundle.ValueType, bundle.Items, names, ignoreDefault);
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
    internal static IEmitComplexConverter CreateDictionaryConverter(this IMapperOptions options, Type instanceType, Type targetType, Type dictionaryType, Type keyType, Type elementType, PropertyInfo itemProperty, IEnumerable<string> names, bool ignoreDefault)
    {
        var copier = options.CreateDictionaryCopier(instanceType, targetType ?? elementType, dictionaryType, keyType, elementType, itemProperty, names, ignoreDefault);
        if (copier is null)
            return null;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var isInterface = dictionaryType.GetTypeInfo().IsInterface;
#else
        var isInterface = dictionaryType.IsInterface;
#endif
        return options.CreateDictionaryConverter(instanceType, isInterface, dictionaryType, keyType, elementType, copier);
    }
    #endregion
}
