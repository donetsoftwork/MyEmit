using PocoEmit.Configuration;
using PocoEmit.Converters;
using System.Collections.Generic;

namespace PocoEmit.Complexes;

/// <summary>
/// 复杂类型成员信息
/// </summary>
/// <param name="context"></param>
/// <param name="key"></param>
/// <param name="converter"></param>
public class ComplexBundle(BuildContext context, PairTypeKey key, IEmitConverter converter)
    : IComplexBundle
{
    #region 配置
    /// <summary>
    /// 复杂类型转化上下文
    /// </summary>
    private readonly BuildContext _context = context;
    private readonly PairTypeKey _key = key;
    private readonly IEmitConverter _converter = converter;
    private bool _isCircle = false;
    private bool _hasCircle = false;
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
    /// 是否循环引用
    /// </summary>
    public bool IsCircle
        => _isCircle;
    /// <summary>
    /// 是否包含循环引用
    /// </summary>
    public bool HasCircle
        => _isCircle;
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
    #endregion
    /// <summary>
    /// 获取转化器
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public IEmitConverter GetConverter(PairTypeKey key)
        => _context.GetConverter(key);
    /// <summary>
    /// 包含
    /// </summary>
    /// <param name="bundle"></param>
    private bool Include(ComplexBundle bundle)
    {
        _uses.TryGetValue(bundle, out var times);
        _uses[bundle] = times + 1;
        if (_includes.Contains(bundle))
            return false;
        _includes.Add(bundle);
        return true;
    }
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
    /// <summary>
    /// 增加包含类型
    /// </summary>
    /// <param name="item"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    public ComplexBundle Accept(PairTypeKey item, IEmitConverter converter)
    {
        var bundle = _context.GetBundleOrCreate(item, converter);
        if (bundle is null)
            return null;
        if (Include(bundle))
            return bundle;
        return null;
    }
    /// <summary>
    /// 移除包含类型
    /// </summary>
    /// <param name="bundle"></param>
    public void Remove(ComplexBundle bundle)
        => _includes.Remove(bundle);
}
