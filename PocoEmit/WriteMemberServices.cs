using PocoEmit.Collections;
using PocoEmit.Members;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoEmit;

/// <summary>
/// 成员写入扩展方法
/// </summary>
public static partial class PocoEmitServices
{
    #region GetWriteAction
    /// <summary>
    /// 写成员委托
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="poco"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static Action<TInstance, TValue> GetWriteAction<TInstance, TValue>(this IPoco poco, string memberName)
    {
        var writer = poco.GetEmitWriter<TInstance>(memberName);
        if (writer is null)
            return null;
        return GetWriteAction<TInstance, TValue>(poco, writer);
    }
    /// <summary>
    /// 写成员委托
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="poco"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public static Action<TInstance, TValue> GetWriteAction<TInstance, TValue>(this IPoco poco, MemberInfo member)
    {
        var emitWriter = MemberContainer.Instance.MemberWriterCacher.Get(member);
        if (emitWriter is null)
            return null;
        return GetWriteAction<TInstance, TValue>(poco, emitWriter);
    }
    /// <summary>
    /// 写成员委托
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="poco"></param>
    /// <param name="emitWriter"></param>
    /// <returns></returns>
    public static Action<TInstance, TValue> GetWriteAction<TInstance, TValue>(this IPoco poco, IEmitMemberWriter emitWriter)
    {
        var instanceType = typeof(TInstance);
        var valueType = typeof(TValue);
        var noConvert = CheckType(poco, ref emitWriter, instanceType, valueType);
        var instance = Expression.Parameter(instanceType, "instance");
        var value = Expression.Parameter(valueType, "value");
        if (noConvert)
        {
            if (emitWriter.Compiled && emitWriter is ICompiledWriter<TInstance, TValue> typeWriter)
                return typeWriter.WriteAction;
            var writeAction = Compile<TInstance, TValue>(emitWriter, instance, value);
            MemberContainer.Instance.MemberWriterCacher.Set(emitWriter.Info, new CompiledWriter<TInstance, TValue>(emitWriter, writeAction));
            return writeAction;
        }
        else if (emitWriter == null)
        {
            return null;
        }
        return Compile<TInstance, TValue>(emitWriter, instance, value);
    }
    #endregion
    #region GetMemberWriter
    /// <summary>
    /// 获取成员写入器
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="poco"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static IMemberWriter<TInstance, TValue> GetMemberWriter<TInstance, TValue>(this IPoco poco, string memberName)
    {
        var writer = poco.GetEmitWriter<TInstance>(memberName);
        if (writer is null)
            return null;
        return GetMemberWriter<TInstance, TValue>(poco, writer);
    }
    /// <summary>
    /// 获取成员写入器
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="poco"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public static IMemberWriter<TInstance, TValue> GetMemberWriter<TInstance, TValue>(this IPoco poco, MemberInfo member)
    {
        var emitWriter = MemberContainer.Instance.MemberWriterCacher.Get(member);
        if (emitWriter is null)
            return null;
        return GetMemberWriter<TInstance, TValue>(poco, emitWriter);
    }
    /// <summary>
    /// 获取成员写入器
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="poco"></param>
    /// <param name="emitWriter"></param>
    /// <returns></returns>
    public static IMemberWriter<TInstance, TValue> GetMemberWriter<TInstance, TValue>(this IPoco poco, IEmitMemberWriter emitWriter)
    {
        var instanceType = typeof(TInstance);
        var valueType = typeof(TValue);
        var noConvert = CheckType(poco, ref emitWriter, instanceType, valueType);
        var instance = Expression.Parameter(instanceType, "instance");
        var value = Expression.Parameter(valueType, "value");
        if (noConvert)
        {
            if (emitWriter.Compiled && emitWriter is ICompiledWriter<TInstance, TValue> compiledWriter)
                return compiledWriter;
            compiledWriter = new CompiledWriter<TInstance, TValue>(emitWriter, Compile<TInstance, TValue>(emitWriter, instance, value));
            MemberContainer.Instance.MemberWriterCacher.Set(emitWriter.Info, compiledWriter);
            return compiledWriter;
        }
        else if (emitWriter == null)
        {
            return null;
        }
        return new CompiledWriter<TInstance, TValue>(emitWriter, Compile<TInstance, TValue>(emitWriter, instance, value));
    }
    #endregion
    #region Check
    /// <summary>
    /// 检查类型
    /// </summary>
    /// <param name="poco"></param>
    /// <param name="emitWriter"></param>
    /// <param name="instanceType"></param>
    /// <param name="valueType"></param>
    /// <returns></returns>
    public static bool CheckType(this IPoco poco, ref IEmitMemberWriter emitWriter, Type instanceType, Type valueType)
    {
        bool noConvert = true;
        if (CheckInstanceType(poco, ref emitWriter, instanceType))
        {
            if (emitWriter is null)
                return false;
            noConvert = false;
        }
        if (CheckValueType(poco, ref emitWriter, valueType))
            return false;
        return noConvert;
    }
    /// <summary>
    /// 检查实例类型是否需要兼容
    /// </summary>
    /// <param name="poco"></param>
    /// <param name="emitWriter"></param>
    /// <param name="instanceType"></param>
    /// <returns></returns>
    public static bool CheckInstanceType(this IPoco poco, ref IEmitMemberWriter emitWriter, Type instanceType)
    {
        var instanceType0 = emitWriter.InstanceType;
        if (ReflectionHelper.CheckValueType(instanceType, instanceType0))
            return false;
        var emitConverter = poco.GetEmitConverter(instanceType, instanceType0);
        if (emitConverter is null)
            emitWriter = null;
        else
            emitWriter = new ConvertInstanceWriter(emitConverter, emitWriter);
        return true;
    }
    /// <summary>
    /// 检查值类型是否需要转化
    /// </summary>
    /// <param name="poco"></param>
    /// <param name="emitWriter"></param>
    /// <param name="valueType"></param>
    /// <returns></returns>
    public static bool CheckValueType(this IPoco poco, ref IEmitMemberWriter emitWriter, Type valueType)
    {
        var valueType0 = emitWriter.ValueType;
        if (ReflectionHelper.CheckValueType(valueType, valueType0))
            return false;
        var emitConverter = poco.GetEmitConverter(valueType, valueType0);
        if (emitConverter is null)
            emitWriter = null;
        else
            emitWriter = new ConvertValueWriter(emitConverter, emitWriter);
        return true;
    }
    #endregion
    #region Compile
    /// <summary>
    /// 编译写入委托
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="emit"></param>
    /// <param name="instance"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Action<TInstance, TValue> Compile<TInstance, TValue>(this IEmitMemberWriter emit, ParameterExpression instance, ParameterExpression value)
    {
        var body = emit.Write(instance, value);
        var lambda = Expression.Lambda<Action<TInstance, TValue>>(body, instance, value);
        return lambda.Compile();
    }
    /// <summary>
    /// 编译写入委托
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="emit"></param>
    /// <returns></returns>
    public static Action<TInstance, TValue> Compile<TInstance, TValue>(this IEmitMemberWriter emit)
        => Compile<TInstance, TValue>(emit, Expression.Parameter(typeof(TInstance), "instance"), Expression.Parameter(typeof(TValue), "value"));
    #endregion
}
