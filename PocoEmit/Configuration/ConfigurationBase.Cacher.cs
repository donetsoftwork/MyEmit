using PocoEmit.Collections;
using System;
using System.Reflection;

namespace PocoEmit.Configuration;

/// <summary>
/// Emit配置
/// </summary>
public abstract partial class ConfigurationBase
{
    #region Cacher
    /// <summary>
    /// 读成员缓存
    /// </summary>
    class ReadFuncCacher(IPocoOptions options)
        : CacheBase<MemberInfo, Func<object, object>>(options)
    {
        #region 配置
        private readonly IPocoOptions _options = options;
        /// <summary>
        /// Emit配置
        /// </summary>
        public IPocoOptions Options
            => _options;
        #endregion
        /// <inheritdoc />
        protected override Func<object, object> CreateNew(MemberInfo key)
            => _options.GetReadFunc<object, object>(key);
    }
    /// <summary>
    /// 写成员缓存
    /// </summary>
    class WriteActionCacher(IPocoOptions options)
        : CacheBase<MemberInfo, Action<object, object>>(options)
    {
        #region 配置
        private readonly IPocoOptions _options = options;
        /// <summary>
        /// Emit配置
        /// </summary>
        public IPocoOptions Options
            => _options;
        #endregion
        /// <inheritdoc />
        protected override Action<object, object> CreateNew(MemberInfo key)
            => _options.GetWriteAction<object, object>(key);
    }
    #endregion
}
