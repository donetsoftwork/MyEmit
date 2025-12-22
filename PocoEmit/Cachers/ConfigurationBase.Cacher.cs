using Hand.Cache;
using System;
using System.Reflection;

namespace PocoEmit.Cachers;

/// <summary>
/// 读成员缓存
/// </summary>
internal class ReadFuncCacher(IPoco poco)
    : CacheFactoryBase<MemberInfo, Func<object, object>>()
{
    #region 配置
    private readonly IPoco _options = poco;
    /// <summary>
    /// Emit配置
    /// </summary>
    public IPoco Poco
        => _options;
    #endregion
    /// <inheritdoc />
    protected override Func<object, object> CreateNew(in MemberInfo key)
        => _options.GetReadFunc<object, object>(key);
}
/// <summary>
/// 写成员缓存
/// </summary>
internal class WriteActionCacher(IPoco poco)
    : CacheFactoryBase<MemberInfo, Action<object, object>>()
{
    #region 配置
    private readonly IPoco _poco = poco;
    /// <summary>
    /// Emit配置
    /// </summary>
    public IPoco Poco
        => _poco;
    #endregion
    /// <inheritdoc />
    protected override Action<object, object> CreateNew(in MemberInfo key)
        => _poco.GetWriteAction<object, object>(key);
}
