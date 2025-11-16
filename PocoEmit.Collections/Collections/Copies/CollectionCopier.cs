using Hand.Reflection;
using PocoEmit.Collections.Saves;
using PocoEmit.Collections.Visitors;
using PocoEmit.Complexes;
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
    /// <param name="sourceType"></param>
    /// <param name="sourceElementType"></param>
    /// <param name="destType"></param>
    /// <param name="destElementType"></param>
    /// <param name="saver"></param>
    /// <param name="sourceVisitor"></param>
    /// <param name="elementConverter"></param>
    /// <param name="clear"></param>
    public CollectionCopier(Type sourceType, Type sourceElementType, Type destType, Type destElementType, IEmitElementSaver saver, IEmitElementVisitor sourceVisitor, IEmitConverter elementConverter, bool clear = true)
        : base(destType, destElementType)
    {
        //_sourceType = sourceType;
        _sourceElementType = sourceElementType;
        _sourceVisitor = sourceVisitor;
        _elementConverter = elementConverter;
        _saver = saver;
        if (clear)
            _clearMethod = GetClearMethod();
    }
    #region 配置
    //private readonly Type _sourceType;
    private readonly Type _sourceElementType;
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
    void IComplexPreview.Preview(IComplexBundle parent)
        => parent.Visit(_elementConverter);
    /// <inheritdoc />
    public IEnumerable<Expression> Copy(IBuildContext context, Expression source, Expression dest)
    {
        dest = CheckInstance(dest);
        if (_clearMethod is not null)
            yield return Expression.Call(dest, _clearMethod);
        yield return _sourceVisitor.Travel(source, item => CopyElement(context, dest, item));
    }
    /// <summary>
    /// 复制子元素
    /// </summary>
    /// <param name="context"></param>
    /// <param name="dest"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public Expression CopyElement(IBuildContext context, Expression dest, Expression item)
    {
        var sourceItem = Expression.Parameter(_sourceElementType, "sourceItem");
        var destItem = Expression.Parameter(_elementType, "destItem");
        return Expression.Block(
            [sourceItem, destItem],
            Expression.Assign(sourceItem, item),
            Expression.Assign(destItem, context.Convert(_elementConverter, sourceItem)),
            _saver.Add(dest, destItem)
            );
    }
    #region MethodInfo
    /// <summary>
    /// 获取清空方法
    /// </summary>
    /// <returns></returns>
    protected virtual MethodInfo GetClearMethod()
        => ReflectionMember.GetMethod(_collectionType, "Clear");
    #endregion
}
