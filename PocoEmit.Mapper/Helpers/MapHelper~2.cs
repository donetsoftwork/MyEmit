using PocoEmit.Configuration;
using PocoEmit.Maping;
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
    /// <param name="sourceIgnores"></param>
    /// <param name="destIgnores"></param>
    /// <param name="comparer"></param>
    internal MapHelper(IMapperOptions mapper, IDictionary<string, string> memberSettings, HashSet<string> sourceIgnores, HashSet<string> destIgnores, IEqualityComparer<string> comparer)
    {
        _mapper = mapper;
        _memberSettings = memberSettings;
        _sourceIgnores = sourceIgnores;
        _destIgnores = destIgnores;
        _matcher = new CustomMatcher(memberSettings, sourceIgnores, destIgnores, comparer);
    }
    /// <summary>
    /// 映射辅助
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="comparer"></param>
    public MapHelper(IMapperOptions mapper, IEqualityComparer<string> comparer)
        : this(mapper, new Dictionary<string, string>(comparer),new HashSet<string>(comparer), new HashSet<string>(comparer), comparer)
    {
    }
    private readonly IMapperOptions _mapper;
    private readonly IDictionary<string, string> _memberSettings;
    private readonly HashSet<string> _sourceIgnores;
    private readonly HashSet<string> _destIgnores;
    private readonly IMemberMatch _matcher;
    private readonly MapTypeKey _key = new(typeof(TSource), typeof(TDest));
    #region 功能
    /// <summary>
    /// 映射
    /// </summary>
    /// <param name="sourceMemberName"></param>
    /// <param name="destMemberName"></param>
    /// <returns></returns>
    public MapHelper<TSource, TDest> SetMemberMap(string sourceMemberName, string destMemberName)
    {
        _memberSettings[destMemberName] = sourceMemberName;
        _mapper.Set(_key, _matcher);
        return this;
    }
    /// <summary>
    /// 忽略源成员
    /// </summary>
    /// <param name="sourceMemberName"></param>
    /// <returns></returns>
    public MapHelper<TSource, TDest> IgnoreSource(string sourceMemberName)
    {
        _sourceIgnores.Add(sourceMemberName);
        _mapper.Set(_key, _matcher);
        return this;
    }
    /// <summary>
    /// 忽略源成员
    /// </summary>
    /// <param name="destMemberName"></param>
    /// <returns></returns>
    public MapHelper<TSource, TDest> IgnoreDest(string destMemberName)
    {
        _destIgnores.Add(destMemberName);
        _mapper.Set(_key, _matcher);
        return this;
    }
    #endregion
    #region Helper
    /// <summary>
    /// 映射源
    /// </summary>
    public SourceHelper Source
        => new(this);
    /// <summary>
    /// 映射目标
    /// </summary>
    public DestHelper Dest
        => new(this);
    #endregion
}
