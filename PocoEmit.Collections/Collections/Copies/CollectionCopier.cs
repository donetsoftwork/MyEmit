using PocoEmit.Collections.Visitors;
using PocoEmit.Converters;
using PocoEmit.Copies;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Collections.Copies;

/// <summary>
/// 集合复制器(支持含Add和Clear的类和接口)
/// </summary>
public class CollectionCopier : EmitCollectionBase
    , IEmitCopier
{
    /// <summary>
    /// 集合复制器
    /// </summary>
    /// <param name="collectionType"></param>
    /// <param name="elementType"></param>
    /// <param name="methodName"></param>
    /// <param name="sourceVisitor"></param>
    /// <param name="elementConverter"></param>
    /// <param name="clear"></param>
    /// <exception cref="ArgumentException"></exception>
    public CollectionCopier(Type collectionType, Type elementType, string methodName, ICollectionVisitor sourceVisitor, IEmitConverter elementConverter, bool clear = true)
        : base(collectionType, elementType)
    {
        
        _sourceVisitor = sourceVisitor;
        _elementConverter = elementConverter;
        _addMethod = GetAddMethod(methodName) ?? throw new ArgumentException($"type '{_collectionType.Name}' does not have {methodName}.");
        if (clear)
            _clearMethod = GetClearMethod() ?? throw new ArgumentException($"type '{_collectionType.Name}' does not have Clear.");
    }
    /// <summary>
    /// 集合复制器
    /// </summary>
    /// <param name="elementType"></param>
    /// <param name="sourceVisitor"></param>
    /// <param name="elementConverter"></param>
    /// <param name="clear"></param>
    public CollectionCopier(Type elementType, ICollectionVisitor sourceVisitor, IEmitConverter elementConverter, bool clear = true)
        : this(typeof(ICollection<>).MakeGenericType(elementType), elementType, "Add", sourceVisitor, elementConverter, clear)
    {
    }
    #region 配置
    private readonly ICollectionVisitor _sourceVisitor;
    private readonly IEmitConverter _elementConverter;
    //private readonly bool _clear = clear;
    private readonly MethodInfo _addMethod;
    private readonly MethodInfo _clearMethod;
    /// <summary>
    /// 遍历源集合
    /// </summary>
    public ICollectionVisitor SourceVisitor
        => _sourceVisitor;
    /// <summary>
    /// 子元素转化
    /// </summary>
    public IEmitConverter ElementConverter
        => _elementConverter;
    /// <summary>
    /// 添加方法
    /// </summary>
    public MethodInfo AddMethod 
        => _addMethod;
    /// <summary>
    /// 清空类型
    /// </summary>
    public MethodInfo ClearMethod 
        => _clearMethod;
    ///// <summary>
    ///// 是否需要清空目标列表
    ///// </summary>
    //public bool Clear 
    //    => _clear;
    #endregion

    /// <inheritdoc />
    public IEnumerable<Expression> Copy(Expression source, Expression dest)
    {
        yield return dest = CheckInstance(dest);
        if (_clearMethod is not null)
            yield return Expression.Call(dest, _clearMethod);
        yield return _sourceVisitor.Travel(source, item => CopyElement(dest, item, _elementConverter));
    }
    /// <summary>
    /// 复制子元素
    /// </summary>
    /// <param name="dest"></param>
    /// <param name="item"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    public Expression CopyElement(Expression dest, Expression item, IEmitConverter converter)
        => Expression.Call(dest, _addMethod, converter.Convert(item));
    #region MethodInfo
    /// <summary>
    /// 获取清空方法
    /// </summary>
    /// <returns></returns>
    protected virtual MethodInfo GetClearMethod()
        => GetClearMethod(_collectionType);
    /// <summary>
    /// 获取添加方法
    /// </summary>
    /// <param name="methodName"></param>
    /// <returns></returns>
    protected virtual MethodInfo GetAddMethod(string methodName)
        => GetAddMethod(_collectionType, _elementType, methodName);
    /// <summary>
    /// 获取清空方法
    /// </summary>
    /// <param name="collectionType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static MethodInfo GetClearMethod(Type collectionType)
        => ReflectionHelper.GetMethod(collectionType, method => method.Name == "Clear" && method.GetParameters().Length == 0);
    /// <summary>
    /// 获取添加方法
    /// </summary>
    /// <param name="collectionType"></param>
    /// <param name="elementType"></param>
    /// <param name="methodName"></param>
    /// <returns></returns>
    public static MethodInfo GetAddMethod(Type collectionType, Type elementType, string methodName = "Add")
        => ReflectionHelper.GetMethod(collectionType, method => CheckAddMethod(method, elementType, methodName));
    /// <summary>
    /// 筛选添加方法
    /// </summary>
    /// <param name="method"></param>
    /// <param name="elementType"></param>
    /// <param name="methodName"></param>
    /// <returns></returns>
    private static bool CheckAddMethod(MethodInfo method, Type elementType, string methodName)
    {
        if (method.Name == methodName)
        {
            var parameters = method.GetParameters();
            return parameters.Length == 1 && parameters[0].ParameterType == elementType;
        }
        return false;
    }
    #endregion
}
