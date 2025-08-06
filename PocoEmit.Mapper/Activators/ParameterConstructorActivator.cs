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
/// <param name="parameters">参数</param>
public class ParameterConstructorActivator(IMapperOptions options, Type returnType, ConstructorInfo constructor, ConstructorParameterMember[] parameters)
    : ConstructorActivator(returnType, constructor)
{
    /// <summary>
    /// 含参构造函数激活
    /// </summary>
    /// <param name="options"></param>
    /// <param name="returnType"></param>
    /// <param name="constructor"></param>
    /// <param name="parameters"></param>
    public ParameterConstructorActivator(IMapperOptions options, Type returnType, ConstructorInfo constructor, ParameterInfo[] parameters)
        : this(options, returnType, constructor, ConstructorParameterMember.Convert(constructor, parameters))
    {
    }
    #region 配置
    private readonly IMapperOptions _options = options;
    private readonly ConstructorParameterMember[] _parameters = parameters;
    /// <summary>
    /// 映射配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    /// <summary>
    /// 参数列表
    /// </summary>
    public ConstructorParameterMember[] Parameters
        => _parameters;
    #endregion
    /// <inheritdoc />
    public override Expression New(Expression argument)
        => Expression.New(_constructor, CreateParameters(argument));
    private Expression[] CreateParameters(Expression source)
    {
        var sourceType = source.Type;
        var readers = _options.MemberCacher.Get(sourceType)?.EmitReaders.Values ?? [];
        var match = _options.GetMemberMatch(sourceType, _returnType);
        var arguments = new Expression[_parameters.Length];
        var i = 0;
        foreach (var parameter in _parameters)
        {
            IEmitMemberReader emitMemberReader = GetReader(parameter, match, readers);
            if (emitMemberReader is null)
                arguments[i++] = _options.CreateDefault(parameter.ValueType);
            else
                arguments[i++] = emitMemberReader.Read(source);
        }
        return arguments;
    }
    /// <summary>
    /// 获取读取器
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="match"></param>
    /// <param name="readers"></param>
    /// <returns></returns>
    private IEmitMemberReader GetReader(ConstructorParameterMember parameter, IMemberMatch match, IEnumerable<IEmitMemberReader> readers)
    {
        var valueType = parameter.ValueType;
        IEmitMemberReader emitMemberReader = null;
        foreach (var reader in match.Select(_options.Recognizer, readers, parameter))
        {
            emitMemberReader = reader;
            _options.CheckValueType(ref emitMemberReader, valueType);
            if (emitMemberReader is not null)
                break;
        }
        return emitMemberReader;
    }
}
