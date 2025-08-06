using PocoEmit.Activators;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Maping;
using System;
using System.Collections.Generic;

namespace PocoEmit.Helpers;

/// <summary>
/// 映射辅助
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
public partial class MapHelper<TSource, TDest>
{
    /// <summary>
    /// 映射辅助
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="memberSettings"></param>
    /// <param name="sourceRecognizer"></param>
    /// <param name="destRecognizer"></param>
    internal MapHelper(IMapper mapper, IDictionary<string, string> memberSettings, IgnoreRecognizer sourceRecognizer, IgnoreRecognizer destRecognizer)
    {
        _mapper = mapper;
        var sourceType = typeof(TSource);
        var destType = typeof(TDest);
        sourceRecognizer.AddPrefix(destType.Name);
        destRecognizer.AddPrefix(sourceType.Name);
        _key = new(sourceType, destType);
        _memberSettings = memberSettings;
        _sourceRecognizer = sourceRecognizer;
        _destRecognizer = destRecognizer;
        var matcher0 = mapper.GetMemberMatch(_key);
        _matcher = new CustomMatcher(matcher0.NameMatch, memberSettings, sourceRecognizer, destRecognizer);
        _mapper.Configure(_key, _matcher);
    }
    /// <summary>
    /// 映射辅助
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="comparison"></param>
    public MapHelper(IMapper mapper, StringComparison comparison)
        : this(mapper, new Dictionary<string, string>(StringCompareConverter.ToComparer(comparison)), new IgnoreRecognizer(mapper.Recognizer), new IgnoreRecognizer(mapper.Recognizer))
    {
    } 
    private readonly IMapper _mapper;
    private readonly IDictionary<string, string> _memberSettings;
    private readonly IgnoreRecognizer _sourceRecognizer;
    private readonly IgnoreRecognizer _destRecognizer;
    private readonly IMemberMatch _matcher;
    private readonly MapTypeKey _key;
    #region 功能
    #region Member
    /// <summary>
    /// 映射
    /// </summary>
    /// <param name="sourceMemberName"></param>
    /// <param name="destMemberName"></param>
    /// <returns></returns>
    public MapHelper<TSource, TDest> SetMemberMap(string sourceMemberName, string destMemberName)
    {
        _memberSettings[destMemberName] = sourceMemberName;
        return this;
    }
    #endregion
    #region Source
    /// <summary>
    /// 忽略源成员
    /// </summary>
    /// <param name="sourceMemberName"></param>
    /// <returns></returns>
    public MapHelper<TSource, TDest> IgnoreSource(string sourceMemberName)
    {
        _sourceRecognizer.Ignore(sourceMemberName);
        return this;
    }
    /// <summary>
    /// 添加源前缀
    /// </summary>
    /// <param name="prefix"></param>
    /// <returns></returns>
    public MapHelper<TSource, TDest> AddSourcePrefix(string prefix)
    {
        _sourceRecognizer.AddPrefix(prefix);
        return this;
    }
    /// <summary>
    /// 添加源后缀
    /// </summary>
    /// <param name="suffix"></param>
    public MapHelper<TSource, TDest> AddSourceSuffix(string suffix)
    {
        _sourceRecognizer.AddSuffix(suffix);
        return this;
    }
    #endregion
    #region Dest
    /// <summary>
    /// 忽略目标成员
    /// </summary>
    /// <param name="destMemberName"></param>
    /// <returns></returns>
    public MapHelper<TSource, TDest> IgnoreDest(string destMemberName)
    {
        _destRecognizer.Ignore(destMemberName);
        return this;
    }
    /// <summary>
    /// 添加目标前缀
    /// </summary>
    /// <param name="prefix"></param>
    /// <returns></returns>
    public MapHelper<TSource, TDest> AddDestPrefix(string prefix)
    {
        _destRecognizer.AddPrefix(prefix);
        return this;
    }
    /// <summary>
    /// 添加目标后缀
    /// </summary>
    /// <param name="suffix"></param>
    public MapHelper<TSource, TDest> AddDestSuffix(string suffix)
    {
        _destRecognizer.AddSuffix(suffix);
        return this;
    }
    #endregion
    #region Activator
    /// <summary>
    /// 设置带参委托来激活
    /// </summary>
    /// <param name="activatorFunc"></param>
    /// <returns></returns>
    public MapHelper<TSource, TDest> UseActivator(Func<TSource, TDest> activatorFunc)
    {
        _mapper.Configure(_key, new ArgumentDelegateActivator<TSource, TDest>(activatorFunc));
        return this;
    }
    /// <summary>
    /// 设置委托来激活
    /// </summary>
    /// <param name="activatorFunc"></param>
    /// <returns></returns>
    public MapHelper<TSource, TDest> UseActivator(Func<TDest> activatorFunc)
    {
        _mapper.Configure(_key, new DelegateActivator<TDest>(activatorFunc));
        return this;
    }
    #endregion
    #region Copier

    #endregion
    #region Converter
    /// <summary>
    /// 使用委托转化
    /// </summary>
    /// <param name="convertFunc"></param>
    /// <returns></returns>
    public IMapper UseConvertFunc(Func<TSource, TDest> convertFunc)
    {
        _mapper.Configure(_key, new DelegateConverter<TSource, TDest>(convertFunc));
        return _mapper;
    }
    #endregion
    #endregion
    #region Helper
    /// <summary>
    /// 映射源
    /// </summary>
    public SourceHelper Source
        => new(this, _sourceRecognizer);
    /// <summary>
    /// 映射目标
    /// </summary>
    public DestHelper Dest
        => new(this, _destRecognizer);
    #endregion
}
