using PocoEmit.Services;
using System;
using System.Reflection;

namespace PocoEmit;

/// <summary>
/// 属性和字段访问换成
/// </summary>
public static class EmitCaches
{
    #region 功能
    /// <summary>
    /// 读取属性
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    public static Func<object, object> EmitGetter(PropertyInfo property)
        => PropertyGet.Cacher.Get(property);
    /// <summary>
    /// 读取字段
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public static Func<object, object> EmitGetter(FieldInfo field)
        => FieldGet.Cacher.Get(field);
    /// <summary>
    /// 写入属性
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    public static Action<object, object> EmitSetter(PropertyInfo property)
        => PropertySet.Cacher.Get(property);
    /// <summary>
    /// 写入字段
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public static Action<object, object> EmitSetter(FieldInfo field)
        => FieldSet.Cacher.Get(field);
    #endregion
    #region Cacher
    class PropertyGet
    {
        public static readonly PropertyGetCacher Cacher = new();
    }
    class PropertySet
    {
        public static readonly PropertySetCacher Cacher = new();
    }
    class FieldSet
    {
        public static readonly FieldSetCacher Cacher = new();
    }
    class FieldGet
    {
        public static readonly FieldGetCacher Cacher = new();
    }
    #endregion
}
