using System;

namespace PocoEmit.Enums;

/// <summary>
/// 枚举配置
/// </summary>
/// <param name="enumType"></param>
/// <param name="underType"></param>
/// <param name="capacity"></param>
public class EnumBundle(Type enumType, Type underType, int capacity)
    : EnumBundleBase<EnumField>(enumType, underType, capacity)    
{
}
