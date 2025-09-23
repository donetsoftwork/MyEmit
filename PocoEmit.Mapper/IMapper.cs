using PocoEmit.Activators;
using PocoEmit.Builders;
using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Copies;
using PocoEmit.Maping;
using System;
using System.Linq.Expressions;

namespace PocoEmit;

/// <summary>
/// 对象映射
/// </summary>
public interface IMapper
    : IPoco
    , IConfigure<PairTypeKey, IMemberMatch>
    , IConfigure<PairTypeKey, IEmitCopier>
    , IStore<PairTypeKey, IEmitCopier>
    , IConfigure<Type, IEmitActivator>
    , IConfigure<PairTypeKey, IEmitActivator>
    , IConfigure<PairTypeKey, Delegate>
    , IConfigure<Type, bool>
    , IConfigure<Type, IBuilder<Expression>>    
{
    /// <summary>
    /// 获取成员匹配
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    IMemberMatch GetMemberMatch(in PairTypeKey key);
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
    IEmitCopier GetEmitCopier(in PairTypeKey key);
    #endregion
}
