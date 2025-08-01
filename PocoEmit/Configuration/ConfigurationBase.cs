using PocoEmit.Builders;
using PocoEmit.Collections;
using PocoEmit.Converters;
using PocoEmit.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PocoEmit.Configuration;

/// <summary>
/// Emit配置
/// </summary>
public abstract partial class ConfigurationBase 
    : IPocoOptions
{
    /// <summary>
    /// Emit配置
    /// </summary>
    public ConfigurationBase(IReflectionMember reflectionMember)
    {
        // 初始化配置
        _reflectionMember = reflectionMember;
        _convertBuilder = DefaultConvertBuilder.Default;
        _converterFactory = new(this);
        _memberCacher = new TypeMemberCacher(this, reflectionMember);
        _readerFuncCacher = new ReadFuncCacher(this);
        _writerActionCacher = new WriteActionCacher(this);
    }
    #region 配置
    /// <summary>
    /// 转换器工厂
    /// </summary>
    private readonly ConverterFactory _converterFactory;
    /// <summary>
    /// 构建转换器
    /// </summary>
    private IConvertBuilder _convertBuilder;
    private IReflectionMember _reflectionMember;
    /// <summary>
    /// 成员缓存器
    /// </summary>
    private readonly TypeMemberCacher _memberCacher;
    /// <summary>
    /// 读成员缓存
    /// </summary>
    private readonly ReadFuncCacher _readerFuncCacher;
    /// <summary>
    /// 写成员缓存
    /// </summary>
    private readonly WriteActionCacher _writerActionCacher;
    /// <inheritdoc />
    public IEnumerable<IEmitConverter> Converters
        => _converters.Values;
    /// <summary>
    /// 反射获取成员
    /// </summary>
    public IReflectionMember ReflectionMember
    {
        get => _reflectionMember;
        internal set => _reflectionMember = value;
    }
    /// <inheritdoc />
    public IConvertBuilder ConvertBuilder
    {
        get => _convertBuilder;
        protected set => _convertBuilder = value;
    }
    /// <summary>
    /// 成员缓存器
    /// </summary>
    public TypeMemberCacher MemberCacher
        => _memberCacher;
    /// <summary>
    /// 读成员委托
    /// </summary>
    public IEnumerable<Func<object, object>> ReadFuncs
        => _readFuncs.Values;
    /// <summary>
    /// 写成员委托
    /// </summary>
    public IDictionary<MemberInfo, Action<object, object>> WriteActions
        => _writeActions;
    #endregion
    #region 功能
    /// <inheritdoc />
    public virtual Func<object, object> GetReadFunc(MemberInfo member)
        => _readerFuncCacher.Get(member);
    /// <inheritdoc />
    public virtual Action<object, object> GetWriteAction(MemberInfo member)
        => _writerActionCacher.Get(member);
    /// <inheritdoc />
    public virtual IEmitConverter GetEmitConverter(MapTypeKey key)
        => _converterFactory.Get(key);
    /// <inheritdoc />
    public void SetConvertSetting(MapTypeKey key, IEmitConverter converter)
        => _convertSetting[key] = converter;
    #endregion
}
