using System;
using System.Reflection;

namespace PocoEmit.Converters;

/// <summary>
/// 委托类型转化
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
/// <param name="convertFunc"></param>
/// <param name="method"></param>
public sealed class DelegateConverter<TSource, TDest>(Func<TSource, TDest> convertFunc, MethodInfo method)
    : MethodConverter(convertFunc.Target, method, typeof(TSource)), ICompiledConverter<TSource, TDest>
{
    /// <summary>
    /// 委托类型转化
    /// </summary>
    /// <param name="converter"></param>
    public DelegateConverter(Func<TSource, TDest> converter)
        : this(converter, converter.GetMethodInfo())
    {
    }
    #region 配置
    private readonly Func<TSource, TDest> _convertFunc = convertFunc;
    /// <inheritdoc />
    public Func<TSource, TDest> ConvertFunc
        => _convertFunc;
    /// <inheritdoc />
    public override bool Compiled
        => true;
    #endregion
    /// <inheritdoc />
    TDest IPocoConverter<TSource, TDest>.Convert(TSource source)
        => _convertFunc(source);
    /// <inheritdoc />
    object IObjectConverter.ConvertObject(object source)
        => _convertFunc((TSource)source);
}
