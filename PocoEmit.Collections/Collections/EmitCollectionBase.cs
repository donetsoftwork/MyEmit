using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Collections;

/// <summary>
/// 集合基类
/// </summary>
/// <param name="collectionType"></param>
/// <param name="elementType"></param>
public abstract class EmitCollectionBase(Type collectionType, Type elementType)
    : ICompileInfo
{
    #region 配置
    /// <summary>
    /// 集合类型
    /// </summary>
    protected readonly Type _collectionType = collectionType;
    /// <summary>
    /// 子元素类型
    /// </summary>
    protected readonly Type _elementType = elementType;
    /// <summary>
    /// 子元素类型
    /// </summary>
    public Type ElementType
        => _elementType;
    /// <summary>
    /// 集合类型
    /// </summary>
    public Type CollectionType
        => _collectionType;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <summary>
    /// 确认集合类型
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    protected Expression CheckInstance(Expression instance)
        => ReflectionHelper.CheckValueType(instance.Type, _collectionType) ? instance : Expression.Convert(instance, _collectionType);
}
