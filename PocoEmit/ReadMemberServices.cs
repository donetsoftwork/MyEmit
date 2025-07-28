using PocoEmit.Collections;
using PocoEmit.Configuration;
using PocoEmit.Members;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit;

/// <summary>
/// 成员读取扩展方法
/// </summary>
public static partial class PocoEmitServices
{
    #region GetReadFunc
    /// <summary>
    /// 读成员委托
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="options"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static Func<TInstance, TValue> GetReadFunc<TInstance, TValue>(this IPocoOptions options, string memberName)
    {
        var member = options.GetReadMember<TInstance>(memberName);
        if (member is null)
            return null;
        return GetReadFunc<TInstance, TValue>(options, member);
    }
    /// <summary>
    /// 读成员委托
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="options"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public static Func<TInstance, TValue> GetReadFunc<TInstance, TValue>(this IPocoOptions options, MemberInfo member)
    {
        var emitReader = MemberContainer.Instance.MemberReaderCacher.Get(member);
        if (emitReader is null)
            return null;
        bool noConvert = true;
        var instanceType = typeof(TInstance);
        if (CheckCompatible(options, ref emitReader, instanceType))
        {
            if (emitReader is null)
                return null;
            noConvert = false;
        }
        var valueType = typeof(TValue);
        if (CheckConvert(options, ref emitReader, valueType))
        {
            if (emitReader is null)
                return null;
            noConvert = false;
        }
        var instance = Expression.Parameter(instanceType, "instance");
        if (noConvert)
        {
            if (emitReader.Compiled && emitReader is ICompiledReader<TInstance, TValue> compiledReader)
                return compiledReader.ReadFunc;
            var readFunc = Compile<TInstance, TValue>(emitReader, instance);
            MemberContainer.Instance.MemberReaderCacher.Set(member, new CompiledReader<TInstance, TValue>(emitReader, readFunc));
            return readFunc;
        }
        return Compile<TInstance, TValue>(emitReader, instance);
    }
    #endregion
    #region GetMemberReader
    /// <summary>
    /// 获取成员读取器
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="options"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static IMemberReader<TInstance, TValue> GetMemberReader<TInstance, TValue>(this IPocoOptions options, string memberName)
    {
        var member = options.GetReadMember<TInstance>(memberName);
        if (member is null)
            return null;
        return GetMemberReader<TInstance, TValue>(options, member);
    }
    /// <summary>
    /// 获取成员读取器
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="options"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public static IMemberReader<TInstance, TValue> GetMemberReader<TInstance, TValue>(this IPocoOptions options, MemberInfo member)
    {
        var emitReader = MemberContainer.Instance.MemberReaderCacher.Get(member);
        if (emitReader is null)
            return null;
        bool noConvert = true;
        var instanceType = typeof(TInstance);
        if (CheckCompatible(options, ref emitReader, instanceType))
        {
            if (emitReader is null)
                return null;
            noConvert = false;
        }
        var valueType = typeof(TValue);
        if (CheckConvert(options, ref emitReader, valueType))
        {
            if (emitReader is null)
                return null;
            noConvert = false;
        }
        if (noConvert)
        {
            if (emitReader.Compiled && emitReader is ICompiledReader<TInstance, TValue> compiledReader)
                return compiledReader;
            compiledReader = new CompiledReader<TInstance, TValue>(emitReader, Compile<TInstance, TValue>(emitReader));
            MemberContainer.Instance.MemberReaderCacher.Set(member, compiledReader);
            return compiledReader;
        }
        var instance = Expression.Parameter(instanceType, "instance");
        return new CompiledReader<TInstance, TValue>(emitReader, Compile<TInstance, TValue>(emitReader, instance));
    }
    #endregion
    #region Check
    /// <summary>
    /// 检查实例类型是否需要兼容
    /// </summary>
    /// <param name="options"></param>
    /// <param name="emitReader"></param>
    /// <param name="instanceType"></param>
    /// <returns></returns>
    public static bool CheckCompatible(this IPocoOptions options, ref IEmitMemberReader emitReader, Type instanceType)
    {
        var instanceType0 = emitReader.InstanceType;
        if (ReflectionHelper.CheckValueType(instanceType, instanceType0))
            return false;
        var emitConverter = options.ConverterFactory.Get(instanceType, instanceType0);
        if (emitConverter is null)
            emitReader = null;
        else
            emitReader = new ConvertInstanceReader(emitConverter, emitReader);
        return true;
    }
    /// <summary>
    /// 检查值类型是否需要转化
    /// </summary>
    /// <param name="options"></param>
    /// <param name="emitReader"></param>
    /// <param name="valueType"></param>
    /// <returns></returns>
    public static bool CheckConvert(this IPocoOptions options, ref IEmitMemberReader emitReader, Type valueType)
    {
        var valueType0 = emitReader.ValueType;
        if (ReflectionHelper.CheckValueType(valueType0, valueType))
            return false;
        var emitConverter = options.ConverterFactory.Get(valueType0, valueType);
        if (emitConverter is null)
            emitReader = null;
        else
            emitReader = new ConvertValueReader(emitReader, emitConverter);
        return true;
    }
    #endregion
    #region Compile
    /// <summary>
    /// 编译转换委托
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="emit"></param>
    /// <param name="instance"></param>
    /// <returns></returns>
    public static Func<TInstance, TValue> Compile<TInstance, TValue>(this IEmitMemberReader emit, ParameterExpression instance)
    {
        var body = emit.Read(instance);
        var lambda = Expression.Lambda<Func<TInstance, TValue>>(body, instance);
        return lambda.Compile();
    }
    /// <summary>
    /// 编译转换委托
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="emit"></param>
    /// <returns></returns>
    public static Func<TInstance, TValue> Compile<TInstance, TValue>(this IEmitMemberReader emit)
        => Compile<TInstance, TValue>(emit, Expression.Parameter(typeof(TInstance), "instance"));
    #endregion
}
