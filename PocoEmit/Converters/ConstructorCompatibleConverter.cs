using Hand.Reflection;
using PocoEmit.Configuration;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Converters;

/// <summary>
/// 构造函数兼容转化器
/// </summary>
/// <param name="key"></param>
/// <param name="constructor"></param>
/// <param name="parameterType"></param>
public class ConstructorCompatibleConverter(in PairTypeKey key, ConstructorInfo constructor, Type parameterType)
    : ConstructorConverter(key, constructor)
{
    #region 配置
    private readonly Type _parameterType = parameterType;
    /// <summary>
    /// 参数类型
    /// </summary>
    public Type ParameterType 
        => _parameterType;
    #endregion
    /// <inheritdoc />
    public override Expression Convert(Expression source)
        => base.Convert(Expression.Convert(source, _parameterType));
}
