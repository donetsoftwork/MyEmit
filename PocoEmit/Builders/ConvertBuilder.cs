using PocoEmit.Converters;
using System;
using System.Reflection;

namespace PocoEmit.Builders;

/// <summary>
/// 默认转换构建器
/// </summary>
public class ConvertBuilder
{
    /// <summary>
    /// 默认转换构建器
    /// </summary>
    protected ConvertBuilder()
    {
    }
    /// <summary>
    /// 构建转换器
    /// </summary>
    /// <param name="sourceType">源类型</param>
    /// <param name="destType">目标类型</param>
    /// <returns>转换器</returns>
    public virtual IEmitConverter Build(Type sourceType, Type destType)
        => new EmitConverter(destType);
    /// <summary>
    /// 构建同类型转换器
    /// </summary>
    /// <param name="instanceType">类型</param>
    /// <returns>转换器</returns>
    public virtual IEmitConverter BuildForSelf(Type instanceType)
        => PassConverter.Instance;
    /// <summary>
    /// 构建object转换器
    /// </summary>
    /// <param name="sourceType"></param>
    /// <returns></returns>
    public virtual IEmitConverter BuildForObject(Type sourceType)
        => new EmitConverter(typeof(object));
    /// <summary>
    /// 构建字符串转换器
    /// </summary>
    /// <param name="sourceType"></param>
    /// <returns></returns>
    public virtual IEmitConverter BuildForString(Type sourceType)
        => SelfMethodConverter.ToStringConverter;
    /// <summary>
    /// 构建Nullable转换器
    /// </summary>
    /// <param name="original"></param>
    /// <param name="originalSourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public virtual IEmitConverter BuildForNullable(IEmitConverter original, Type originalSourceType, Type destType)
        => new CompatibleConverter(original, originalSourceType, destType);
    /// <summary>
    /// 构造函数转化器
    /// </summary>
    /// <param name="constructor"></param>
    /// <param name="sourceType"></param>
    /// <returns></returns>
    public virtual IEmitConverter BuildByConstructor(ConstructorInfo constructor, Type sourceType)
        => new ConstructorConverter(constructor);
    /// <summary>
    /// 默认转换构建器实例
    /// </summary>
    public static readonly ConvertBuilder Default = new();
}
