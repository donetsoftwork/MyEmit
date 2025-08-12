using PocoEmit.Configuration;
using System;

namespace PocoEmit;

/// <summary>
/// 简单对象配置
/// </summary>
public sealed class Poco
    : ConfigurationBase
{
    /// <summary>
    /// 简单对象配置
    /// </summary>
    private Poco(PocoOptions options)
        : base(options)
    {
    }
    #region 功能
    /// <summary>
    /// 简单对象处理对象
    /// </summary>
    /// <returns></returns>
    public static IPoco Create()
    {
        var options = new PocoOptions();
        _globalOptions?.Invoke(options);
        var poco = new Poco(options);
        _globalConfiguration?.Invoke(poco);
        return poco;
    }
    #endregion
    #region Global
    /// <summary>
    /// 全局配置
    /// </summary>
    private static Action<PocoOptions> _globalOptions = null;
    /// <summary>
    /// 全局配置
    /// </summary>
    private static Action<IPoco> _globalConfiguration = null;
    /// <summary>
    /// 全局配置
    /// </summary>
    /// <param name="options"></param>
    public static void GlobalOptions(Action<PocoOptions> options)
    {
        if (options is null)
            return;
        if (_globalConfiguration is null)
            _globalOptions = options;
        else
            _globalOptions += options;
    }
    /// <summary>
    /// 全局配置
    /// </summary>
    /// <param name="configuration"></param>
    public static void GlobalConfigure(Action<IPoco> configuration)
    {
        if (configuration is null)
            return;
        if (_globalConfiguration is null)
            _globalConfiguration = configuration;
        else
            _globalConfiguration += configuration;
    }
    #endregion
    #region Default
    /// <summary>
    /// 默认实例
    /// </summary>
    public static IPoco Default
        => Inner.Default;
    /// <summary>
    /// 内部延迟初始化
    /// </summary>
    sealed class Inner
    {
        /// <summary>
        /// Emit全局配置
        /// </summary>
        internal static readonly IPoco Default = Create();
    }
    #endregion
}