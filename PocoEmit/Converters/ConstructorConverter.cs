using PocoEmit.Configuration;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Converters;

/// <summary>
/// 构造函数转化器
/// </summary>
/// <param name="key"></param>
/// <param name="constructor"></param>
public class ConstructorConverter(PairTypeKey key, ConstructorInfo constructor)
    : IEmitConverter
{
    #region 配置
    private readonly PairTypeKey _key = key;
    private readonly ConstructorInfo _constructor = constructor;
    /// <inheritdoc />
    public PairTypeKey Key
        => _key;
    /// <summary>
    /// 构造函数
    /// </summary>
    public ConstructorInfo Constructor
        => _constructor;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    #region IEmitConverter
    /// <inheritdoc />
    public virtual Expression Convert(Expression source)
      => Expression.New(_constructor, source);
    #endregion
}
