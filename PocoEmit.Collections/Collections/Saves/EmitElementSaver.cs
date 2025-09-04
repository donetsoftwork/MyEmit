using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Collections.Saves;

/// <summary>
/// 集合元素保存器
/// </summary>
/// <param name="collectionType"></param>
/// <param name="elementType"></param>
/// <param name="addMethod"></param>
public class EmitElementSaver(Type collectionType, Type elementType, MethodInfo addMethod)
    : EmitCollectionBase(collectionType, elementType)
    , IEmitElementSaver
{
    #region 配置
    private readonly MethodInfo _addMethod = addMethod;
    /// <summary>
    /// 添加方法
    /// </summary>
    public MethodInfo AddMethod
        => _addMethod;
    #endregion
    /// <inheritdoc />
    public Expression Add(Expression instance, Expression value)
        => Expression.Call(CheckInstance(instance), _addMethod, value);
}
