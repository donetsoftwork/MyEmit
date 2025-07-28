using PocoEmit.Converters;
using System;

namespace PocoEmit.Builders;

/// <summary>
/// 构建转换器接口
/// </summary>
public interface IConvertBuilder
{
    /// <summary>
    /// 构建转换器
    /// </summary>
    /// <param name="sourceType">源类型</param>
    /// <param name="destType">目标类型</param>
    /// <returns>转换器</returns>
    IEmitConverter Build(Type sourceType, Type destType);
    /// <summary>
    /// 构建同类型转换器
    /// </summary>
    /// <param name="instanceType">类型</param>
    /// <returns>转换器</returns>
    IEmitConverter BuildForSelf(Type instanceType);
    /// <summary>
    /// 构建字符串转换器
    /// </summary>
    /// <param name="sourceType"></param>
    /// <returns></returns>

    IEmitConverter BuildForString(Type sourceType);
    /// <summary>
    /// 构建object转换器
    /// </summary>
    /// <param name="sourceType"></param>
    /// <returns></returns>
    IEmitConverter BuildForObject(Type sourceType);
    /// <summary>
    /// 构建Nullable转换器
    /// </summary>
    /// <param name="original"></param>
    /// <param name="originalSourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    IEmitConverter BuildForNullable(IEmitConverter original, Type originalSourceType, Type destType);
}
