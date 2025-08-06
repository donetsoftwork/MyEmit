using PocoEmit.Activators;
using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Copies;
using PocoEmit.Maping;
using System;

namespace PocoEmit;

/// <summary>
/// 对象映射
/// </summary>
public interface IMapper
    : IPoco
    , IConfigure<MapTypeKey, IMemberMatch>
    , IConfigure<MapTypeKey, IEmitCopier>
    , IStore<MapTypeKey, IEmitCopier>
    , IConfigure<Type, IEmitActivator>
    , IConfigure<MapTypeKey, IEmitActivator>
    , IConfigure<Type, bool>
    , IConfigure<Type, object>
{
    /// <summary>
    /// 获取成员匹配
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    IMemberMatch GetMemberMatch(MapTypeKey key);
    /// <summary>
    /// 识别器
    /// </summary>
    IRecognizer Recognizer { get; }
    #region IEmitCopier
    /// <summary>
    /// 获取Emit类型复制器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    IEmitCopier GetEmitCopier(MapTypeKey key);
    #endregion
}
