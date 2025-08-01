using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Maping;
using PocoEmit.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit.Activators;

/// <summary>
/// 参数激活构造函数
/// </summary>
/// <param name="options"></param>
/// <param name="returnType"></param>
/// <param name="constructor"></param>
/// <param name="match"></param>
public class ParameterActivatorBak(IMapperOptions options, Type returnType, ConstructorInfo constructor, IMemberMatch match)
{
    #region 配置
    private readonly IMapperOptions _options = options;
    private readonly Type _returnType = returnType;
    private readonly ConstructorInfo _constructor = constructor;
    private readonly MemberInfo[] _parameters = SelectMembers(constructor.GetParameters());
    private readonly IMemberMatch _match = match;
    /// <summary>
    /// 映射配置
    /// </summary>
    public IMapperOptions Options
        => _options;
    /// <inheritdoc />
    public Type ReturnType
        => _returnType;
    /// <summary>
    /// 构造函数
    /// </summary>
    public ConstructorInfo Constructor
        => _constructor;
    /// <summary>
    /// 构造函数参数
    /// </summary>
    public MemberInfo[] Parameters
        => _parameters;
    /// <summary>
    /// 成员匹配
    /// </summary>
    public IMemberMatch Match 
        => _match;
    #endregion
    /// <summary>
    /// 激活
    /// </summary>
    /// <param name="source"></param>
    /// <param name="sourceMembers"></param>
    /// <returns></returns>
    public Expression New(Expression source, IEnumerable<MemberInfo> sourceMembers)
    {
        var count = _parameters.Length;
        if (count == 0)
            return Expression.New(_constructor);
        var arguments = new Expression[count];
        var readerCacher = MemberContainer.Instance.MemberReaderCacher;
        var i = 0;
        var parameters = _constructor.GetParameters();
        foreach (var param in _parameters)
        {
            var reader = GetReader(readerCacher, sourceMembers, param);
            if(reader is null)
            {
                var parameterType = parameters[i].ParameterType;
                arguments[i++] = Expression.Default(parameterType);
                //Expression.Constant
            }
            else
            {
                arguments[i++] = reader.Read(source);
            }
        }
        return Expression.New(_constructor, arguments);
    }
    /// <summary>
    /// 过滤目标成员
    /// </summary>
    /// <param name="destMembers"></param>
    /// <returns></returns>
    public IEnumerable<MemberInfo> CheckDestMembers(IEnumerable<MemberInfo> destMembers)
    {
        foreach (var destMember in destMembers)
        {
            if(_parameters.Any(p=> _match.Match(p, destMember)))
                continue;
            yield return destMember;
        }
    }
    /// <summary>
    /// 获取读取器
    /// </summary>
    /// <param name="readerCacher"></param>
    /// <param name="sourceMembers"></param>
    /// <param name="destMember"></param>
    /// <returns></returns>
    private IEmitMemberReader GetReader(MemberReaderCacher readerCacher, IEnumerable<MemberInfo> sourceMembers, MemberInfo destMember)
    {
        var member = sourceMembers.FirstOrDefault(m => _match.Match(m, destMember));
        if(member is null)
            return null;
        return readerCacher.Get(member);
    }
    /// <summary>
    /// 转化为MemberInfo
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    private static MemberInfo[] SelectMembers(ParameterInfo[] parameters)
    {
        MemberInfo[] members = new MemberInfo[parameters.Length];
        var i = 0;
        foreach (var param in parameters)
            members[i++] = param.Member;
        return members;
    }
}
