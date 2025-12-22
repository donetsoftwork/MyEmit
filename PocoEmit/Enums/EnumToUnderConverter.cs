using Hand.Reflection;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using System;
using System.Linq.Expressions;

namespace PocoEmit.Enums;

/// <summary>
/// 枚举转化为基础类型
/// </summary>
/// <param name="bundle"></param>
public class EnumToUnderConverter(IEnumBundle bundle)
     : IEmitConverter
{
    #region 配置
    private readonly PairTypeKey _key = new(bundle.EnumType, bundle.UnderType);
    //private readonly IEnumBundle _bundle = bundle;
    private readonly Type _underType = bundle.UnderType;
    //private readonly IEnumField[] _fields = [.. bundle.Fields];

    /// <inheritdoc />
    public PairTypeKey Key
        => _key;
    ///// <summary>
    ///// 枚举配置
    ///// </summary>
    //public IEnumBundle Bundle
    //=> _bundle;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    public virtual Expression Convert(Expression source)
        => Expression.Convert(source, _underType);    
}
