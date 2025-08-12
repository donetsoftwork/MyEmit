using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Members;
using System.Linq.Expressions;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
using System.Reflection;
#endif

namespace PocoEmit.Copies;

/// <summary>
/// 成员转化
/// </summary>
/// <param name="options"></param>
/// <param name="sourceReader"></param>
/// <param name="destWriter"></param>
public sealed class MemberConverter(IMapperOptions options, IEmitReader sourceReader, IEmitWriter destWriter)
    : IMemberConverter
{
    #region 配置
    private readonly IMapperOptions _options = options;
    private readonly IEmitReader _sourceReader = sourceReader;
    private readonly IEmitWriter _destWriter = destWriter;
    /// <summary>
    /// Emit配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    /// <summary>
    /// 源成员读取器
    /// </summary>
    public IEmitReader SourceReader 
        => _sourceReader;
    /// <summary>
    /// 目标成员写入器
    /// </summary>
    public IEmitWriter DestWriter 
        => _destWriter;
    #endregion
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    public Expression Convert(Expression source, Expression dest)
    {
        var getter = _sourceReader.Read(source);
        var getterType = getter.Type;
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
        var isValueType = getterType.GetTypeInfo().IsValueType;
#else
        var isValueType = getterType.IsValueType;
#endif
        if (isValueType)
            return _destWriter.Write(dest, getter);
        var test = Expression.Equal(getter, Expression.Constant(null));
        var defaultValue = _options.CreateDefault(getterType);
        return Expression.IfThenElse(test, defaultValue, _destWriter.Write(dest, getter));
    }
}
