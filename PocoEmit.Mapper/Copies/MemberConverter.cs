using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
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
/// <param name="converter"></param>
public sealed class MemberConverter(IMapperOptions options, IEmitReader sourceReader, IEmitMemberWriter destWriter, IEmitConverter converter)
    : IMemberConverter
{
    #region 配置
    private readonly IMapperOptions _options = options;
    private readonly IEmitReader _sourceReader = sourceReader;
    private readonly IEmitMemberWriter _destWriter = destWriter;
    private readonly IEmitConverter _converter = converter;
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
    public IEmitMemberWriter DestWriter 
        => _destWriter;
    /// <summary>
    /// 成员类型转化
    /// </summary>
    public IEmitConverter Converter 
        => _converter;
    #endregion
    /// <inheritdoc />
    void IComplexPreview.Preview(IComplexBundle parent)
        => parent.Visit(_converter);
    /// <summary>
    /// 获取源成员
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public Expression GetSourceMember(Expression source)
        => _sourceReader.Read(source);
    /// <inheritdoc />
    public Expression ConvertMember(IBuildContext context, Expression sourceMember, Expression dest)
    {
        var sourceType = sourceMember.Type;        
        // 基础类型直接赋值(忽略null判断)
        if (_options.CheckPrimitive(sourceType))
            return _destWriter.Write(dest, _converter.Convert(sourceMember));
        var defaultValue = _options.DefaultValueBuilder.Build(_destWriter)
            ?.Build();
        if (defaultValue is null)
        {    
            return Expression.IfThen(Expression.NotEqual(sourceMember, Expression.Constant(null)), _destWriter.Write(dest, context.Convert(_converter, sourceMember)));
        }
        else
        {
#if (NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6)
            var isValueType = sourceType.GetTypeInfo().IsValueType;
#else
            var isValueType = sourceType.IsValueType;
#endif
            // 值类型直接赋值(忽略null判断和默认值)
            if (isValueType)
                return _destWriter.Write(dest, sourceMember);
            return Expression.IfThenElse(Expression.Equal(sourceMember, Expression.Constant(null)), _destWriter.Write(dest, defaultValue), _destWriter.Write(dest, context.Convert(_converter, sourceMember)));
        }
    }
}
