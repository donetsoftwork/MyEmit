using Hand.Reflection;
using PocoEmit.Builders;
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
    /// <param name="poco"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static Func<TInstance, TValue> GetReadFunc<TInstance, TValue>(this IPoco poco, string memberName)
    {
        var reader = poco.GetEmitReader<TInstance>(memberName);
        if (reader is null)
            return null;
        return GetReadFunc<TInstance, TValue>(poco, reader);
    }
    /// <summary>
    /// 读成员委托
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="poco"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public static Func<TInstance, TValue> GetReadFunc<TInstance, TValue>(this IPoco poco, MemberInfo member)
    {
        var emitReader = MemberContainer.Instance.MemberReaderCacher.Get(member);
        if (emitReader is null)
            return null;
        return GetReadFunc<TInstance, TValue>(poco, emitReader);
    }
    /// <summary>
    /// 读成员委托
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="poco"></param>
    /// <param name="emitReader"></param>
    /// <returns></returns>
    public static Func<TInstance, TValue> GetReadFunc<TInstance, TValue>(this IPoco poco, IEmitMemberReader emitReader)
    {
        var instanceType = typeof(TInstance);
        var valueType = typeof(TValue);
        var noConvert = CheckType(poco, ref emitReader, instanceType, valueType);
        var instance = Expression.Parameter(instanceType, "instance");
        if (noConvert)
        {
            if (emitReader.Compiled && emitReader is ICompiledReader<TInstance, TValue> compiledReader)
                return compiledReader.ReadFunc;
            var readFunc = Compiler.CompileFunc<TInstance, TValue>(emitReader);
            MemberContainer.Instance.MemberReaderCacher.Save(emitReader.Info, new CompiledReader<TInstance, TValue>(emitReader, readFunc));
            return readFunc;
        }
        else if (emitReader is null)
        {
            return null;
        }
        return Compiler.CompileFunc<TInstance, TValue>(emitReader);
    }
    #endregion
    #region GetMemberReader
    /// <summary>
    /// 获取成员读取器
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="poco"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static IMemberReader<TInstance, TValue> GetMemberReader<TInstance, TValue>(this IPoco poco, string memberName)
    {
        var reader = poco.GetEmitReader<TInstance>(memberName);
        if (reader is null)
            return null;
        return GetMemberReader<TInstance, TValue>(poco, reader);
    }
    /// <summary>
    /// 获取成员读取器
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="poco"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public static IMemberReader<TInstance, TValue> GetMemberReader<TInstance, TValue>(this IPoco poco, MemberInfo member)
    {
        var emitReader = MemberContainer.Instance.MemberReaderCacher.Get(member);
        if (emitReader is null)
            return null;
        return GetMemberReader<TInstance, TValue>(poco, emitReader);
    }
    /// <summary>
    /// 获取成员读取器
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="poco"></param>
    /// <param name="emitReader"></param>
    /// <returns></returns>
    public static IMemberReader<TInstance, TValue> GetMemberReader<TInstance, TValue>(this IPoco poco, IEmitMemberReader emitReader)
    {
        var instanceType = typeof(TInstance);
        var valueType = typeof(TValue);
        var noConvert = CheckType(poco, ref emitReader, instanceType, valueType);
        if (noConvert)
        {
            if (emitReader.Compiled && emitReader is ICompiledReader<TInstance, TValue> compiledReader)
                return compiledReader;
            compiledReader = new CompiledReader<TInstance, TValue>(emitReader, Compiler.CompileFunc<TInstance, TValue>(emitReader));
            MemberContainer.Instance.MemberReaderCacher.Save(emitReader.Info, compiledReader);
            return compiledReader;
        }
        else if (emitReader is null)
        {
            return null;
        }
        var instance = Expression.Parameter(instanceType, "instance");
        return new CompiledReader<TInstance, TValue>(emitReader, Compiler.CompileFunc<TInstance, TValue>(emitReader));
    }
    #endregion
    #region CheckType
    /// <summary>
    /// 检查类型
    /// </summary>
    /// <param name="poco"></param>
    /// <param name="emitReader"></param>
    /// <param name="instanceType"></param>
    /// <param name="valueType"></param>
    /// <returns></returns>
    internal static bool CheckType(this IPoco poco, ref IEmitMemberReader emitReader, Type instanceType, Type valueType)
    {
        bool noConvert = true;
        if (CheckInstanceType(poco, ref emitReader, instanceType))
        {
            if (emitReader is null)
                return false;
            noConvert = false;
        }
        if (CheckValueType(poco, ref emitReader, valueType))
            return false;
        return noConvert;
    }
    /// <summary>
    /// 检查实例类型是否需要兼容
    /// </summary>
    /// <param name="poco"></param>
    /// <param name="emitReader"></param>
    /// <param name="instanceType"></param>
    /// <returns></returns>
    internal static bool CheckInstanceType(this IPoco poco, ref IEmitMemberReader emitReader, Type instanceType)
    {
        var instanceType0 = emitReader.InstanceType;
        if (PairTypeKey.CheckValueType(instanceType, instanceType0))
            return false;
        var emitConverter = poco.GetEmitConverter(instanceType, instanceType0);
        if (emitConverter is null)
            emitReader = null;
        else
            emitReader = new ConvertInstanceReader(emitConverter, emitReader);
        return true;
    }
    /// <summary>
    /// 检查值类型是否需要转化
    /// </summary>
    /// <param name="poco"></param>
    /// <param name="emitReader"></param>
    /// <param name="valueType"></param>
    /// <returns></returns>
    internal static bool CheckValueType(this IPoco poco, ref IEmitMemberReader emitReader, Type valueType)
    {
        var valueType0 = emitReader.ValueType;
        if (PairTypeKey.CheckValueType(valueType0, valueType))
            return false;
        var emitConverter = poco.GetEmitConverter(valueType0, valueType);
        if (emitConverter is null)
            emitReader = null;
        else
            emitReader = new ConvertValueReader(emitReader, emitConverter, valueType);
        return true;
    }
    #endregion
    #region Build
    /// <summary>
    /// 转换委托
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="emit"></param>
    /// <returns></returns>
    public static Expression<Func<TInstance, TValue>> Build<TInstance, TValue>(this IEmitMemberReader emit)
    {
        var instance = Expression.Parameter(typeof(TInstance), "instance");
        return Expression.Lambda<Func<TInstance, TValue>>(emit.Read(instance), instance);
    }
    #endregion
}
