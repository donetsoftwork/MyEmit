//using PocoEmit.Collections;
//using PocoEmit.Configuration;
//using PocoEmit.Converters;
//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;

//namespace PocoEmit.Members;

///// <summary>
///// 成员转数组
///// </summary>
///// <param name="options"></param>
///// <param name="arrayType"></param>
///// <param name="elementType"></param>
///// <param name="bundle"></param>
///// <param name="names"></param>
//public class MemberArrayConverter(IMapperOptions options, Type arrayType, Type elementType, IDictionary<string, IEmitMemberReader> bundle, ICollection<string> names)
//    : EmitCollectionBase(arrayType, elementType)
//    , IEmitConverter
//{
//    #region 配置
//    private readonly IMapperOptions _options = options;
//    private readonly PairTypeKey _key = new(sourceType, arrayType);
//    private readonly IDictionary<string, IEmitMemberReader> _bundle = bundle;
//    private readonly ICollection<string> _names = names;
//    /// <summary>
//    /// 配置
//    /// </summary>
//    public IMapperOptions Options
//        => _options;
//    /// <inheritdoc />
//    public PairTypeKey Key
//        => _key;
//    /// <summary>
//    /// 成员集合
//    /// </summary>
//    public IDictionary<string, IEmitMemberReader> Bundle 
//        => _bundle;
//    /// <summary>
//    /// 成员名
//    /// </summary>
//    public ICollection<string> Names
//        => _names;
//    #endregion

//    /// <inheritdoc />
//    public Expression Convert(Expression source)
//    {
//        var expressions = new List<Expression>(_names.Count);
//        foreach (var name in _names)
//        {
//            var member = _bundle[name];
//            if (member == null)
//                continue;
//            var memberConverter = _options.GetEmitConverter(member.ValueType, _elementType);
//            if (memberConverter is null)
//                continue;
//            expressions.Add(memberConverter.Convert(member.Read(source)));
//        }
//        return Expression.NewArrayInit(_elementType, [.. expressions]);
//    }    
//}
