using PocoEmit.Configuration;
using PocoEmit.Dictionaries.Members;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Dictionaries;

/// <summary>
/// 成员复制到字典扩展方法
/// </summary>
public static partial class PocoDictionariesServices
{
    #region CreatetDictionaryCopyAction
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
    public static Action<TInstance, TDictionary> CreatetDictionaryCopyAction<TInstance, TTarget, TDictionary>(this IMapper mapper, IEnumerable<string> names = null, bool ignoreDefault = true)
        where TDictionary : class
    {
        return ((IMapperOptions)mapper).CreateDictionaryCopier(typeof(TInstance), typeof(TTarget), typeof(TDictionary), names, ignoreDefault)
            .CompileAction<TInstance, TDictionary>(mapper);
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
    public static Action<TInstance, TDictionary> CreatetDictionaryCopyAction<TInstance, TDictionary>(this IMapper mapper, IEnumerable<string> names = null, bool ignoreDefault = true)
        where TDictionary : class
    {
        return ((IMapperOptions)mapper).CreateDictionaryCopier(typeof(TInstance), null, typeof(TDictionary), names, ignoreDefault)
            .CompileAction<TInstance, TDictionary>(mapper);
    }
    #endregion
    #region BuildDictionaryCopier
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
    public static Expression<Action<TInstance, TDictionary>> BuildDictionaryCopier<TInstance, TTarget, TDictionary>(this IMapper mapper, IEnumerable<string> names = null, bool ignoreDefault = true)
        where TDictionary : class
    {
        return ((IMapperOptions)mapper).CreateDictionaryCopier(typeof(TInstance), typeof(TTarget), typeof(TDictionary), names, ignoreDefault)
            ?.Build<TInstance, TDictionary>(mapper);
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
    public static Expression<Action<TInstance, TDictionary>> BuildDictionaryCopier<TInstance, TDictionary>(this IMapper mapper, IEnumerable<string> names = null, bool ignoreDefault = true)
        where TDictionary : class
    {
        return ((IMapperOptions)mapper).CreateDictionaryCopier(typeof(TInstance), null, typeof(TDictionary), names, ignoreDefault)
            ?.Build<TInstance, TDictionary>(mapper);
    }
    #endregion
    #region CreateDictionaryCopier
    /// <summary>
    /// 成员复制到字典
    /// </summary>
    /// <param name="options"></param>
    /// <param name="instanceType"></param>
    /// <param name="targetType"></param>
    /// <param name="dictionaryType"></param>
    /// <param name="names"></param>
    /// <param name="ignoreDefault">是否忽略默认值</param>
    /// <returns></returns>
    internal static DictionaryCopier CreateDictionaryCopier(this IMapperOptions options, Type instanceType, Type targetType, Type dictionaryType, IEnumerable<string> names, bool ignoreDefault)
    {
        if (options.CheckPrimitive(instanceType) || options.CheckPrimitive(dictionaryType))
            return null;
        var bundle = CollectionContainer.Instance.DictionaryCacher.Get(dictionaryType);
        if (bundle == null)
            return null;
        return options.CreateDictionaryCopier(instanceType, targetType, dictionaryType, bundle.KeyType, bundle.ValueType, bundle.Items, names, ignoreDefault);
    }
    /// <summary>
    /// 成员复制到字典
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
    internal static DictionaryCopier CreateDictionaryCopier(this IMapperOptions options, Type instanceType, Type targetType, Type dictionaryType, Type keyType, Type elementType, PropertyInfo itemProperty, IEnumerable<string> names, bool ignoreDefault)
    {
        var keyConverter = options.GetEmitConverter(typeof(string), keyType);
        if (keyConverter is null)
            return null;
        targetType ??= elementType;
        var bundle = options.MemberCacher.Get(instanceType);
        if (MemberIndexVisitor.ValidateDictionary(options, bundle, targetType))
        {
            var elementConverter = options.GetEmitConverter(targetType, elementType);
            MemberIndexVisitor visitor = new(options, bundle, targetType, names);
            return new(dictionaryType, keyType, elementType, itemProperty, visitor, keyConverter, elementConverter, ignoreDefault);
        }
        return null;
    }
    #endregion
}
