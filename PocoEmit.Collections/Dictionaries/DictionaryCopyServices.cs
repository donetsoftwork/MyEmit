using PocoEmit.Configuration;
using PocoEmit.Members;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Dictionaries;

/// <summary>
/// 成员复制到字典扩展方法
/// </summary>
public static partial class PocoDictionaryServices
{
    #region DictionaryCopy
    /// <summary>
    /// 复制到字典
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="mapper"></param>
    /// <param name="instance"></param>
    /// <param name="dic"></param>
    public static void DictionaryCopy<TInstance>(this IMapper mapper, TInstance instance, IDictionary<string, object> dic)
    {
        var instanceType = typeof(TInstance);
        var bundle = mapper.MemberCacher.Get(instanceType);
        var members = bundle.ReadMembers;
        foreach (var kv in members)
        {
            var func = mapper.GetReadFunc(kv.Value);
            if (func == null)
                continue;
            dic[kv.Key] = func(instance);
        }
    }
    #endregion
    #region GetDictionaryCopyAction
    /// <summary>
    /// 获取复制到字典委托
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static Action<TInstance, IDictionary<string, object>> GetDictionaryCopyAction<TInstance>(this IMapper mapper)
    {
        var instanceType = typeof(TInstance);
        var bundle = mapper.MemberCacher.Get(instanceType);
        var members = bundle.ReadMembers;
        Action<TInstance, IDictionary<string, object>> actions = null;
        foreach (var kv in members)
        {
            var func = mapper.GetReadFunc(kv.Value);
            if (func == null)
                continue;
            actions += (instance, dic) => dic[kv.Key] = func(instance);
        }
        return actions ?? ((_, _) => { });
    }
    #endregion
}
