using PocoEmit.Configuration;
using PocoEmit.Members;
using System;
using System.Linq;
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
            var valueType = parameter.ValueType;
            var reader = readers.FirstOrDefault(m => match.Match(m, parameter));
            if (reader is not null)
                _options.CheckValueType(ref reader, valueType);
            if (reader is null)
                arguments[i++] = Expression.Default(valueType);
            else
                arguments[i++] = reader.Read(source);
        }
        return arguments;
    }
    ///// <summary>
    ///// 构造参数表达式
    ///// </summary>
    ///// <param name="source"></param>
    ///// <param name="members"></param>
    ///// <returns></returns>
    //public static Expression[] CreateParameters(Expression source, IEmitReader[] members)
    //{
    //    var parameters = new Expression[members.Length];
    //    var i = 0;
    //    foreach (var member in members)
    //        parameters[i++] = member.Read(source);
    //    return parameters;
    //}
}
