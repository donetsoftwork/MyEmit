using System;
using System.Collections.Generic;

namespace PocoEmit.Enums;

/// <summary>
/// 枚举配置
/// </summary>
public interface IEnumBundle
{
    /// <summary>
    /// 枚举类型
    /// </summary>
    Type EnumType { get; }
    /// <summary>
    /// 基础类型
    /// </summary>
    Type UnderType { get; }
    /// <summary>
    /// 字段
    /// </summary>
    IEnumerable<IEnumField> Fields { get; }
    /// <summary>
    /// 位域标记
    /// </summary>
    bool HasFlag { get; }
    /// <summary>
    /// 按名获取字段
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    IEnumField GetFieldByName(string name);
}
