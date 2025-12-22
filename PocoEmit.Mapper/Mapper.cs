using PocoEmit.Configuration;
using System;

namespace PocoEmit;

/// <summary>
/// 对象映射配置
/// </summary>
public sealed class Mapper : MapperConfigurationBase
{
    /// <summary>
    /// 对象映射配置
    /// </summary>
    /// <param name="options"></param>
    private Mapper(MapperOptions options)
        : base(options) 
    {
    }
    #region Create
    /// <summary>
    /// 构造映射器
    /// </summary>
    /// <returns></returns>
    public static IMapper Create()
        => Create(new MapperOptions());
    /// <summary>
    /// 构造映射器
    /// </summary>
    /// <returns></returns>
    public static IMapper Create(MapperOptions options)
    {
        _globalOptions?.Invoke(options);
        var mapper = new Mapper(options);
        _globalConfiguration?.Invoke(mapper);
        return mapper;
    }
    #endregion
    #region Global
    /// <summary>
    /// 全局配置
    /// </summary>
    private static Action<MapperOptions> _globalOptions = null;
    /// <summary>
    /// 全局配置
    /// </summary>
    private static Action<IMapper> _globalConfiguration = null;
    /// <summary>
    /// 全局配置
    /// </summary>
    /// <param name="options"></param>
    public static void GlobalOptions(Action<MapperOptions> options)
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
    public static void GlobalConfigure(Action<IMapper> configuration)
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
    public static IMapper Default
        => Inner.Default;
    /// <summary>
    /// 内部延迟初始化
    /// </summary>
    sealed class Inner
    {
        /// <summary>
        /// Emit全局配置
        /// </summary>
        internal static readonly IMapper Default = Create();
    }
    #endregion
}
