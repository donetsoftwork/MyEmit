using PocoEmit.Configuration;
using PocoEmit.Converters;
using System.Collections.Generic;
using System.Linq;

namespace PocoEmit.Complexes;

/// <summary>
/// 复杂类型成员信息
/// </summary>
/// <param name="context"></param>
/// <param name="key"></param>
/// <param name="converter"></param>
/// <param name="depth"></param>
/// <param name="isCollection"></param>
public class ComplexBundle(BuildContext context, in PairTypeKey key, IEmitConverter converter, int depth, bool isCollection)
    : IComplexBundle
{
    #region 配置
    /// <summary>
    /// 复杂类型转化上下文
    /// </summary>
    private readonly BuildContext _context = context;
    private readonly PairTypeKey _key = key;
    private readonly IEmitConverter _converter = converter;
    private int _depth = depth;
    private bool _isCircle = false;
    private bool _hasCircle = false;
    private bool _isCache = false;
    /// <summary>
    /// 被缓存包含
    /// </summary>
    private bool _cacheIncluded = false;
    private bool _hasCache = false;
    private readonly bool _isCollection = isCollection;
    //private ComplexUsed _used = ComplexUsed.None;
    /// <summary>
    /// 包含类型
    /// </summary>
    private readonly HashSet<ComplexBundle> _includes = [];
    /// <summary>
    /// 被调用
    /// </summary>
    private readonly Dictionary<ComplexBundle, int> _uses = [];
    /// <summary>
    /// 复杂类型转化上下文
    /// </summary>
    public BuildContext Context 
        => _context;
    /// <summary>
    /// 转化类型
    /// </summary>
    public PairTypeKey Key 
        => _key;
    /// <summary>
    /// Emit类型转化
    /// </summary>
    public IEmitConverter Converter
        => _converter;
    /// <summary>
    /// 是否集合类型
    /// </summary>
    public bool IsCollection
    => _isCollection;
    /// <summary>
    /// 是否循环引用
    /// </summary>
    public bool IsCircle
        => _isCircle;
    /// <summary>
    /// 是否包含循环引用
    /// </summary>
    public bool HasCircle
        => _hasCircle || _isCircle;
    /// <summary>
    /// 是否缓存
    /// </summary>
    public bool IsCache
        => _isCache || _cacheIncluded;
    /// <summary>
    /// 是否包含缓存
    /// </summary>
    public bool HasCache
        => _hasCache || _isCache || _cacheIncluded;
    ///// <summary>
    ///// 是否包含上下文
    ///// </summary>
    //public bool HasContext
    //    => _isCircle || _hasCircle || _isCache || _hasCache;
    /// <summary>
    /// 包含类型
    /// </summary>
    public HashSet<ComplexBundle> Includes
        => _includes;
    /// <summary>
    /// 被调用类型
    /// </summary>
    public Dictionary<ComplexBundle, int> Uses
        => _uses;
    /// <summary>
    /// 引用深度
    /// </summary>
    public int Depth 
        => _depth;
    ///// <summary>
    ///// 被使用状态
    ///// </summary>
    //public ComplexUsed Used
    //    => _used;
    #endregion
    /// <summary>
    /// 修正深度
    /// </summary>
    /// <param name="depth"></param>
    public void CheckDepth(int depth)
    {
        if (depth < _depth)
            _depth = depth;
    }
    ///// <summary>
    ///// 修正使用状态
    ///// </summary>
    //public void CheckUsed()
    //{
    //    switch (_uses.Count)
    //    {
    //        case 0:
    //            break;
    //        case 1:
    //            if (_uses.First().Value == 1)
    //                _used = ComplexUsed.One;
    //            else
    //                _used = ComplexUsed.Many;
    //            break;
    //        default:
    //            _used = ComplexUsed.Many;
    //            break;
    //    }
    //}
    /// <summary>
    /// 获取转化器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public IEmitConverter GetConverter(in PairTypeKey key)
        => _context.GetConverter(key);
    /// <summary>
    /// 包含
    /// </summary>
    /// <param name="bundle"></param>
    private bool Include(ComplexBundle bundle)
    {
        if(bundle is null)
            return false;
        if (_includes.Contains(bundle))
            return false;
        _includes.Add(bundle);
        Use(bundle);
        return true;
    }
    /// <summary>
    /// 调用
    /// </summary>
    /// <param name="bundle"></param>
    private void Use(ComplexBundle bundle)
    {
        var uses = bundle.Uses;
        if (_isCollection)
        {
            uses[this] = 2;
        }
        else
        {
            uses.TryGetValue(this, out var times);
            uses[this] = times + 1;
        }
    }
    #region Circle
    /// <summary>
    /// 检测循环引用
    /// </summary>
    /// <returns></returns>
    public bool CheckIsCircle()
    {
        if (_isCircle)
            return true;
        return CheckIsCircle(this, [], _includes);
    }
    /// <summary>
    /// 递归查找循环引用
    /// </summary>
    /// <param name="bundle"></param>
    /// <param name="skip"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    public static bool CheckIsCircle(ComplexBundle bundle, HashSet<ComplexBundle> skip, IEnumerable<ComplexBundle> includes)
    {
        foreach (var item in includes)
        {
            if (skip.Contains(item))
                continue;
            if (bundle == item)
                return bundle._isCircle = true;
            skip.Add(item);
            if (CheckIsCircle(bundle, skip, item.Includes))
                return bundle._hasCircle = true;
        }
        return false;
    }
    /// <summary>
    /// 检测循环引用包含
    /// </summary>
    /// <returns></returns>
    public bool CheckHasCircle()
    {
        if (_hasCircle || _isCircle)
            return true;
        return CheckHasCircle(this, [this], _includes);
    }
    /// <summary>
    /// 递归查找循环引用包含
    /// </summary>
    /// <param name="bundle"></param>
    /// <param name="skip"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    public static bool CheckHasCircle(ComplexBundle bundle, HashSet<ComplexBundle> skip, IEnumerable<ComplexBundle> includes)
    {
        foreach (var item in includes)
        {
            if (skip.Contains(item))
                continue;
            if (item._hasCircle || item._isCircle)
                return bundle._hasCircle = true;
            skip.Add(item);
            if (CheckHasCircle(item, skip, item.Includes))
                return bundle._hasCircle = true;
        }
        return false;
    }
    #endregion
    #region Cache
    /// <summary>
    /// 检测是否缓存
    /// </summary>
    /// <returns></returns>
    public bool CheckIsCache()
    {
        if (_isCircle)
            return _isCache = true;
        switch (_uses.Count)
        {
            case 0:
                return _isCache = false;
            case 1:
                if (_uses.First().Value == 1)
                    return _isCache = false;
                break;
            default:
                break;
        }
        return _isCache = true;
    }
    /// <summary>
    /// 判断是否缓存包含
    /// </summary>
    /// <returns></returns>
    public bool CheckCacheInclude()
    {
        if(IsCache)
            return true;
        return CheckCacheInclude(this, [this], _includes);
    }
    /// <summary>
    /// 判断是否缓存包含
    /// </summary>
    /// <param name="bundle"></param>
    /// <param name="skip"></param>
    /// <param name="uses"></param>
    /// <returns></returns>
    public static bool CheckCacheInclude(ComplexBundle bundle, HashSet<ComplexBundle> skip, IEnumerable<ComplexBundle> uses)
    {
        foreach (var item in uses)
        {
            if (skip.Contains(item))
                continue;
            if (item.IsCache)
                return bundle._cacheIncluded = true;
            skip.Add(item);
            if (CheckCacheInclude(item, skip, item.Includes))
                return bundle._cacheIncluded = true;
        }
        return false;
    }
    /// <summary>
    /// 按照循环引用设置缓存
    /// </summary>
    public bool CheckCacheByCircle()
        => _isCache = _isCircle;
    /// <summary>
    /// 判断是否包含缓存
    /// </summary>
    /// <returns></returns>
    public bool CheckHasCache()
    {
        if (HasCircle || HasCache)
            return true;
        return CheckHasCache(this, [this], _includes);
    }
    /// <summary>
    /// 判断是否包含缓存
    /// </summary>
    /// <param name="bundle"></param>
    /// <param name="skip"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    public static bool CheckHasCache(ComplexBundle bundle, HashSet<ComplexBundle> skip, HashSet<ComplexBundle> includes)
    {
        foreach (var item in includes)
        {
            if (skip.Contains(item))
                continue;
            if (item.HasCircle || item.IsCache)
                return bundle._hasCache = true;
            skip.Add(item);
            if (CheckHasCache(item, skip, item.Includes))
                return bundle._hasCache = true;
        }
        return false;
    }
    #endregion
    /// <inheritdoc />
    public ComplexBundle Accept(in PairTypeKey item, IEmitConverter converter, bool isCollection)
    {
        //var bundle = _context.GetBundleOrCreate(item, converter, _depth + 2, isCollection);
        var bundle = _context.GetBundle(item);
        if (bundle is null)
        {
            bundle = _context.CreateBundle(item, converter, _depth, isCollection);
            if (Include(bundle))
                return bundle;
        }
        else
        {
            Include(bundle);
        }
        return null;
    }
    /// <summary>
    /// 移除包含类型
    /// </summary>
    /// <param name="bundle"></param>
    public void Remove(ComplexBundle bundle)
        => _includes.Remove(bundle);
}
