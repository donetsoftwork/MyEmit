using PocoEmit.Collections;
using PocoEmit.Collections.Saves;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PocoEmit.Members;

/// <summary>
/// 成员转集合
/// </summary>
/// <param name="options"></param>
/// <param name="sourceType"></param>
/// <param name="collectionType"></param>
/// <param name="elementType"></param>
/// <param name="saver"></param>
/// <param name="bundle"></param>
/// <param name="names"></param>
public class MemberCollectionConverter(IMapperOptions options,Type sourceType, Type collectionType, Type elementType, IEmitElementSaver saver, IDictionary<string, IEmitMemberReader> bundle, ICollection<string> names)
    : EmitCollectionBase(collectionType, elementType)
    , IEmitConverter
{
    #region 配置
    private readonly IMapperOptions _options = options;
    private readonly PairTypeKey _key = new(sourceType, collectionType);
    private readonly IEmitElementSaver _saver = saver;
    private readonly IDictionary<string, IEmitMemberReader> _bundle = bundle;
    private readonly ICollection<string> _names = names;
    /// <summary>
    /// 配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    /// <inheritdoc />
    public PairTypeKey Key
        => _key;
    /// <summary>
    /// 集合元素保存器
    /// </summary>
    public IEmitElementSaver Saver
        => _saver;
    /// <summary>
    /// 成员集合
    /// </summary>
    public IDictionary<string, IEmitMemberReader> Bundle
        => _bundle;
    /// <summary>
    /// 成员名
    /// </summary>
    public ICollection<string> Names
        => _names;
    #endregion
    /// <inheritdoc />
    public Expression Convert(Expression source)
        => Expression.ListInit(Expression.New(_collectionType), CheckElement(source));
    /// <summary>
    /// 检查子元素
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    private ElementInit[] CheckElement(Expression source)
    {
        var expressions = new List<ElementInit>(_names.Count);
        foreach (var name in _names)
        {
            var member = _bundle[name];
            if (member == null)
                continue;
            var memberConverter = _options.GetEmitConverter(member.ValueType, _elementType);
            if (memberConverter is null)
                continue;            
            expressions.Add(Expression.ElementInit(_saver.AddMethod, memberConverter.Convert(member.Read(source))));
        }
        return [.. expressions];
    }
}
