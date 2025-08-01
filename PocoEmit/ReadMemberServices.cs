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
        var reader = options.GetEmitReader<TInstance>(memberName);
        if (reader is null)
            return null;
        return GetReadFunc<TInstance, TValue>(options, reader);
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
        return GetReadFunc<TInstance, TValue>(options, emitReader);
    }
    /// <summary>
    /// 读成员委托
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="options"></param>
    /// <param name="emitReader"></param>
    /// <returns></returns>
    public static Func<TInstance, TValue> GetReadFunc<TInstance, TValue>(this IPocoOptions options, IEmitMemberReader emitReader)
    {
        var instanceType = typeof(TInstance);
        var valueType = typeof(TValue);
        var noConvert = CheckType(options, ref emitReader, instanceType, valueType);
        var instance = Expression.Parameter(instanceType, "instance");
        if (noConvert)
        {
            if (emitReader.Compiled && emitReader is ICompiledReader<TInstance, TValue> compiledReader)
                return compiledReader.ReadFunc;
            var readFunc = Compile<TInstance, TValue>(emitReader, instance);
            MemberContainer.Instance.MemberReaderCacher.Set(emitReader.Info, new CompiledReader<TInstance, TValue>(emitReader, readFunc));
            return readFunc;
        }
        else if (emitReader is null)
        {
            return null;
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
        var reader = options.GetEmitReader<TInstance>(memberName);
        if (reader is null)
            return null;
        return GetMemberReader<TInstance, TValue>(options, reader);
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
        return GetMemberReader<TInstance, TValue>(options, emitReader);
    }
    /// <summary>
    /// 获取成员读取器
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="options"></param>
    /// <param name="emitReader"></param>
    /// <returns></returns>
    public static IMemberReader<TInstance, TValue> GetMemberReader<TInstance, TValue>(this IPocoOptions options, IEmitMemberReader emitReader)
    {
        var instanceType = typeof(TInstance);
        var valueType = typeof(TValue);
        var noConvert = CheckType(options, ref emitReader, instanceType, valueType);
        if (noConvert)
        {
            if (emitReader.Compiled && emitReader is ICompiledReader<TInstance, TValue> compiledReader)
                return compiledReader;
            compiledReader = new CompiledReader<TInstance, TValue>(emitReader, Compile<TInstance, TValue>(emitReader));
            MemberContainer.Instance.MemberReaderCacher.Set(emitReader.Info, compiledReader);
            return compiledReader;
        }
        else if (emitReader is null)
        {
            return null;
        }
        var instance = Expression.Parameter(instanceType, "instance");
        return new CompiledReader<TInstance, TValue>(emitReader, Compile<TInstance, TValue>(emitReader, instance));
    }
    #endregion
    #region CheckType
    /// <summary>
    /// 检查类型
    /// </summary>
    /// <param name="options"></param>
    /// <param name="emitReader"></param>
    /// <param name="instanceType"></param>
    /// <param name="valueType"></param>
    /// <returns></returns>
    public static bool CheckType(this IPocoOptions options, ref IEmitMemberReader emitReader, Type instanceType, Type valueType)
    {
        bool noConvert = true;
        if (CheckInstanceType(options, ref emitReader, instanceType))
        {
            if (emitReader is null)
                return false;
            noConvert = false;
        }
        if (CheckValueType(options, ref emitReader, valueType))
            return false;
        return noConvert;
    }
    /// <summary>
    /// 检查实例类型是否需要兼容
    /// </summary>
    /// <param name="options"></param>
    /// <param name="emitReader"></param>
    /// <param name="instanceType"></param>
    /// <returns></returns>
    public static bool CheckInstanceType(this IPocoOptions options, ref IEmitMemberReader emitReader, Type instanceType)
    {
        var instanceType0 = emitReader.InstanceType;
        if (ReflectionHelper.CheckValueType(instanceType, instanceType0))
            return false;
        var emitConverter = options.GetEmitConverter(instanceType, instanceType0);
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
    public static bool CheckValueType(this IPocoOptions options, ref IEmitMemberReader emitReader, Type valueType)
    {
        var valueType0 = emitReader.ValueType;
        if (ReflectionHelper.CheckValueType(valueType0, valueType))
            return false;
        var emitConverter = options.GetEmitConverter(valueType0, valueType);
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
