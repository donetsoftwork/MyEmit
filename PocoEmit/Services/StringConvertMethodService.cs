using System;
using System.Collections.Generic;
using System.Reflection;

namespace PocoEmit.Services;

/// <summary>
/// 
/// </summary>
public class StringConvertMethodService
{
    /// <summary>
    /// 系统转化类型
    /// </summary>
    static readonly Dictionary<Type, MethodInfo> _method = [];

    //_defaultInstance = new StaticConvertersManager();
    //_defaultInstance.AddConverterClass(typeof(System.Convert));
    //			_defaultInstance.AddConverterClass(typeof(EMConvert));
    //			_defaultInstance.AddConverterClass(typeof(NullableConverter));
    //			_defaultInstance.AddConverterFunc(EMConvert.GetConversionMethod);

}
