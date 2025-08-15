using PocoEmit.Builders;
using PocoEmit.Configuration;

namespace PocoEmit;

/// <summary>
/// 集合配置扩展方法
/// </summary>
public static partial class CollectionConfigureServices
{
    /// <summary>
    /// 启用集合功能(转化和复制)
    /// </summary>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static IMapper UseCollection(this IMapper mapper)
    {
        if (mapper is not MapperConfigurationBase configuration)
            return mapper;
        if (configuration.ConvertBuilder is not CollectionConvertBuilder)
            configuration.ConvertBuilder = new CollectionConvertBuilder(configuration);
        if (configuration.CopierBuilder is not CollectionCopierBuilder)
            configuration.CopierBuilder = new CollectionCopierBuilder(configuration);
        return mapper;
    }
    /// <summary>
    /// 获取集合复制器
    /// </summary>
    /// <param name="mapper"></param>
    /// <returns></returns>
    internal static CollectionCopierBuilder GetCollectionCopier(this IMapperOptions mapper)
    {
        if (mapper is not MapperConfigurationBase configuration)
            return null;
        if (configuration.CopierBuilder is not CollectionCopierBuilder builder)
        {
            builder = new CollectionCopierBuilder(mapper);
            configuration.CopierBuilder = builder;
        }
        return builder;
    }
}
