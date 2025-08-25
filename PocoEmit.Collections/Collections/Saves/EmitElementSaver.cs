using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Collections.Saves;

/// <summary>
/// 集合元素保存器
/// </summary>
public class EmitElementSaver
    : EmitCollectionBase
    , IEmitElementSaver
{
    /// <summary>
    /// 集合元素保存器
    /// </summary>
    /// <param name="collectionType"></param>
    /// <param name="elementType"></param>
    /// <param name="methodName"></param>
    /// <exception cref="ArgumentException"></exception>
    public EmitElementSaver(Type collectionType, Type elementType, string methodName)
        : base(collectionType, elementType)
    {
        _addMethod = GetAddMethod(methodName) ?? throw new ArgumentException($"type '{_collectionType.Name}' does not have {methodName}.");
    }
    /// <summary>
    /// 集合元素保存器
    /// </summary>
    /// <param name="elementType"></param>
    public EmitElementSaver(Type elementType)
        : this(typeof(ICollection<>).MakeGenericType(elementType), elementType, "Add")
    {
    }
    #region 配置
    private readonly MethodInfo _addMethod;
    /// <summary>
    /// 添加方法
    /// </summary>
    public MethodInfo AddMethod
        => _addMethod;
    #endregion
    #region MethodInfo
    /// <summary>
    /// 获取添加方法
    /// </summary>
    /// <param name="methodName"></param>
    /// <returns></returns>
    protected virtual MethodInfo GetAddMethod(string methodName)
        => GetAddMethod(_collectionType, _elementType, methodName);
    /// <summary>
    /// 获取添加方法
    /// </summary>
    /// <param name="collectionType"></param>
    /// <param name="elementType"></param>
    /// <param name="methodName"></param>
    /// <returns></returns>
    public static MethodInfo GetAddMethod(Type collectionType, Type elementType, string methodName = "Add")
        => ReflectionHelper.GetMethod(collectionType, methodName, [elementType]);
    #endregion
    /// <inheritdoc />
    public Expression Add(Expression instance, Expression value)
        => Expression.Call(CheckInstance(instance), _addMethod, value);
}
