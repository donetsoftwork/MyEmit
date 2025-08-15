using PocoEmit.Collections.Saves;
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
    /// <param name="saver"></param>
    /// <param name="sourceVisitor"></param>
    /// <param name="elementConverter"></param>
    /// <param name="clear"></param>
    /// <exception cref="ArgumentException"></exception>
    public CollectionCopier(Type collectionType, Type elementType, IEmitElementSaver saver, IEmitElementVisitor sourceVisitor, IEmitConverter elementConverter, bool clear = true)
        : base(collectionType, elementType)
    {
        
        _sourceVisitor = sourceVisitor;
        _elementConverter = elementConverter;
        _saver = saver;
        if (clear)
            _clearMethod = GetClearMethod();
    }
    #region 配置
    private readonly IEmitElementVisitor _sourceVisitor;
    private readonly IEmitConverter _elementConverter;
    //private readonly bool _clear = clear;
    private readonly IEmitElementSaver _saver;
    private readonly MethodInfo _clearMethod;
    /// <summary>
    /// 遍历源集合
    /// </summary>
    public IEmitElementVisitor SourceVisitor
        => _sourceVisitor;
    /// <summary>
    /// 子元素转化
    /// </summary>
    public IEmitConverter ElementConverter
        => _elementConverter;
    /// <summary>
    /// 元素保持器
    /// </summary>
    public IEmitElementSaver Saver 
        => _saver;
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
        yield return _sourceVisitor.Travel(source, item => CopyElement(dest, item));
    }
    /// <summary>
    /// 复制子元素
    /// </summary>
    /// <param name="dest"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public Expression CopyElement(Expression dest, Expression item)
        => _saver.Add(dest, _elementConverter.Convert(item));
    #region MethodInfo
    /// <summary>
    /// 获取清空方法
    /// </summary>
    /// <returns></returns>
    protected virtual MethodInfo GetClearMethod()
        => GetClearMethod(_collectionType);
    /// <summary>
    /// 获取清空方法
    /// </summary>
    /// <param name="collectionType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static MethodInfo GetClearMethod(Type collectionType)
        => ReflectionHelper.GetMethod(collectionType, method => method.Name == "Clear" && method.GetParameters().Length == 0);
    #endregion
}
