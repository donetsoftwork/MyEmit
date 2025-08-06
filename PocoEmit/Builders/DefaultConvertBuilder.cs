using PocoEmit.Converters;
using System;
using System.Reflection;

namespace PocoEmit.Builders;

/// <summary>
/// 默认转换构建器
/// </summary>
public class DefaultConvertBuilder : IConvertBuilder
{
    /// <summary>
    /// 默认转换构建器
    /// </summary>
    protected DefaultConvertBuilder()
    {
    }
    /// <inheritdoc />
    public virtual IEmitConverter Build(Type sourceType, Type destType)
        => new EmitConverter(destType);
    /// <inheritdoc />
    public virtual IEmitConverter BuildForSelf(Type instanceType)
        => PassConverter.Instance;
    /// <inheritdoc />
    public virtual IEmitConverter BuildForObject(Type sourceType)
        => new EmitConverter(typeof(object));
    /// <inheritdoc />
    public virtual IEmitConverter BuildForString(Type sourceType)
        => SelfMethodConverter.ToStringConverter;
    /// <inheritdoc />
    public virtual IEmitConverter BuildForNullable(IEmitConverter original, Type originalSourceType, Type destType)
        => new CompatibleConverter(original, originalSourceType, destType);
    /// <inheritdoc />
    public virtual IEmitConverter BuildByConstructor(ConstructorInfo constructor, Type sourceType)
        => new ConstructorConverter(constructor);
    /// <summary>
    /// 默认转换构建器实例
    /// </summary>
    public static DefaultConvertBuilder Default
        => Inner.Instance;
    /// <summary>
    /// 内部延迟加载
    /// </summary>
    class Inner
    {
        /// <summary>
        /// 默认转换构建器实例
        /// </summary>
        public static readonly DefaultConvertBuilder Instance = new();
    }
}
