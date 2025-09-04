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
    /// 获取源成员
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public Expression GetSourceMember(Expression source)
        => _sourceReader.Read(source);
    /// <summary>
    /// 转化成员
    /// </summary>
    /// <param name="sourceMember"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    public Expression ConvertMember(Expression sourceMember, Expression dest)
    {
        var memberType = sourceMember.Type;
        var defaultValue = _options.CreateDefault(memberType);
        if (defaultValue is null)
        {
            // 基础类型直接赋值(忽略null判断)
            if (_options.CheckPrimitive(memberType))
                return _destWriter.Write(dest, sourceMember);
            return Expression.IfThen(Expression.NotEqual(sourceMember, Expression.Constant(null)), _destWriter.Write(dest, sourceMember));
        }
        else
        {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
            var isValueType = memberType.GetTypeInfo().IsValueType;
#else
            var isValueType = memberType.IsValueType;
#endif
            // 值类型直接赋值(忽略null判断和默认值)
            if (isValueType)
                return _destWriter.Write(dest, sourceMember);
            return Expression.IfThenElse(Expression.Equal(sourceMember, Expression.Constant(null)), _destWriter.Write(dest, defaultValue), _destWriter.Write(dest, sourceMember));
        }
    }
}
