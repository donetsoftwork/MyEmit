using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Maping;
using PocoEmit.Members;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Activators;

/// <summary>
/// 含参构造函数激活
/// </summary>
/// <param name="options">映射配置</param>
/// <param name="returnType">返回类型</param>
/// <param name="constructor">构造函数</param>
/// <param name="readers">参数</param>
public class ParameterConstructorActivator(IMapperOptions options, Type returnType, ConstructorInfo constructor, IEmitReader[] readers)
    : ConstructorActivator(returnType, constructor)
    , IComplexPreview
{
    #region 配置
    /// <summary>
    /// Emit配置
    /// </summary>
    private readonly IMapperOptions _options = options;
    private readonly IEmitReader[] _readers = readers;
    /// <summary>
    /// Emit配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    /// <summary>
    /// 参数读取器
    /// </summary>
    public IEmitReader[] Readers
        => _readers;
    #endregion
    /// <inheritdoc />
    public void Preview(IComplexBundle parent)
    {
        foreach (var reader in _readers)
            parent.Visit(reader as ConvertValueReader);
    }
    /// <inheritdoc />
    public override Expression New(IBuildContext context, Expression argument)
        => Expression.New(_constructor, CreateParameters(context, argument));
    private Expression[] CreateParameters(IBuildContext context, Expression source)
    {
        var arguments = new Expression[_readers.Length];
        var i = 0;
        foreach (var reader in _readers)
            arguments[i++] = context.Read(reader, source);
        return arguments;
    }
    /// <summary>
    /// 构建含参构造函数激活器
    /// </summary>
    /// <param name="options"></param>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <param name="constructor"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static ParameterConstructorActivator Create(IMapperOptions options, Type sourceType, Type destType, ConstructorInfo constructor, ParameterInfo[] parameters)
    {
        IEmitReader[] readers = new IEmitReader[parameters.Length];
        var sourceReaders = options.MemberCacher.Get(sourceType)?.EmitReaders.Values ?? [];
        var match = options.GetMemberMatch(sourceType, destType);
        var i = 0;
        foreach (var parameter in parameters)
        {
            var parameterMember = new ConstructorParameterMember(constructor, parameter);
            var reader = GetReader(options, parameterMember, match, sourceReaders);
            if (reader is null)
            {
                var builder = options.DefaultValueBuilder.Build(parameterMember);
                // 没有适配到读取器和默认值构建失败
                if (builder is null)
                    return null;
                readers[i++] = new BuilderReaderAdapter(builder);
            }
            else
            {
                readers[i++] = reader;
            }
        }
        return new ParameterConstructorActivator(options, destType, constructor, readers);
    }
    /// <summary>
    /// 获取读取器
    /// </summary>
    /// <param name="options"></param>
    /// <param name="parameter"></param>
    /// <param name="match"></param>
    /// <param name="readers"></param>
    /// <returns></returns>
    private static IEmitMemberReader GetReader(IMapperOptions options, ConstructorParameterMember parameter, IMemberMatch match, IEnumerable<IEmitMemberReader> readers)
    {
        var valueType = parameter.ValueType;
        IEmitMemberReader emitMemberReader = null;
        foreach (var reader in match.Select(options.Recognizer, readers, parameter))
        {
            emitMemberReader = reader;
            options.CheckValueType(ref emitMemberReader, valueType);
            if (emitMemberReader is not null)
                break;
        }
        return emitMemberReader;
    }
}
