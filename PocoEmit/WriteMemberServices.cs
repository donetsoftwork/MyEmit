using PocoEmit.Collections;
using PocoEmit.Configuration;
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
    /// <param name="options"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static Action<TInstance, TValue> GetWriteAction<TInstance, TValue>(this IPocoOptions options, string memberName)
    {
        var member = options.GetWriteMember<TInstance>(memberName);
        if (member is null)
            return null;
        return GetWriteAction<TInstance, TValue>(options, member);
    }
    /// <summary>
    /// 写成员委托
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="options"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public static Action<TInstance, TValue> GetWriteAction<TInstance, TValue>(this IPocoOptions options, MemberInfo member)
    {
        var emitWriter = MemberContainer.Instance.MemberWriterCacher.Get(member);
        if (emitWriter is null)
            return null;
        bool noConvert = true;
        var instanceType = typeof(TInstance);
        if(CheckCompatible(options, ref emitWriter, instanceType))
        {
            if (emitWriter is null)
                return null;
            noConvert = false;
        }
        var valueType = typeof(TValue);
        if(CheckConvert(options, ref emitWriter, valueType))
        {
            if (emitWriter is null)
                return null;
            noConvert = false;
        }        
        var instance = Expression.Parameter(instanceType, "instance");
        var value = Expression.Parameter(valueType, "value");
        if (noConvert)
        {
            if (emitWriter.Compiled && emitWriter is ICompiledWriter<TInstance, TValue> typeWriter)
                return typeWriter.WriteAction;
            var writeAction = Compile<TInstance, TValue>(emitWriter, instance, value);
            MemberContainer.Instance.MemberWriterCacher.Set(member, new CompiledWriter<TInstance, TValue>(emitWriter, writeAction));
            return writeAction;
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
    /// <param name="options"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static IMemberWriter<TInstance, TValue> GetMemberWriter<TInstance, TValue>(this IPocoOptions options, string memberName)
    {
        var member = options.GetWriteMember<TInstance>(memberName);
        if (member is null)
            return null;
        return GetMemberWriter<TInstance, TValue>(options, member);
    }
    /// <summary>
    /// 获取成员写入器
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="options"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public static IMemberWriter<TInstance, TValue> GetMemberWriter<TInstance, TValue>(this IPocoOptions options, MemberInfo member)
    {
        var emitWriter = MemberContainer.Instance.MemberWriterCacher.Get(member);
        if (emitWriter is null)
            return null;
        bool noConvert = true;
        var instanceType = typeof(TInstance);
        if (CheckCompatible(options, ref emitWriter, instanceType))
        {
            if (emitWriter is null)
                return null;
            noConvert = false;
        }
        var valueType = typeof(TValue);
        if (CheckConvert(options, ref emitWriter, valueType))
        {
            if (emitWriter is null)
                return null;
            noConvert = false;
        }
        var instance = Expression.Parameter(instanceType, "instance");
        var value = Expression.Parameter(valueType, "value");
        if (noConvert)
        {
            if (emitWriter.Compiled && emitWriter is ICompiledWriter<TInstance, TValue> compiledWriter)
                return compiledWriter;
            compiledWriter = new CompiledWriter<TInstance, TValue>(emitWriter, Compile<TInstance, TValue>(emitWriter, instance, value));
            MemberContainer.Instance.MemberWriterCacher.Set(member, compiledWriter);
            return compiledWriter;
        }
        return new CompiledWriter<TInstance, TValue>(emitWriter, Compile<TInstance, TValue>(emitWriter, instance, value));
    }
    #endregion
    #region Check
    /// <summary>
    /// 检查实例类型是否需要兼容
    /// </summary>
    /// <param name="options"></param>
    /// <param name="emitWriter"></param>
    /// <param name="instanceType"></param>
    /// <returns></returns>
    public static bool CheckCompatible(this IPocoOptions options, ref IEmitMemberWriter emitWriter, Type instanceType)
    {
        var instanceType0 = emitWriter.InstanceType;
        if (ReflectionHelper.CheckValueType(instanceType, instanceType0))
            return false;
        var emitConverter = options.ConverterFactory.Get(instanceType, instanceType0);
        if (emitConverter is null)
            emitWriter = null;
        else
            emitWriter = new ConvertInstanceWriter(emitConverter, emitWriter);
        return true;
    }
    /// <summary>
    /// 检查值类型是否需要转化
    /// </summary>
    /// <param name="options"></param>
    /// <param name="emitWriter"></param>
    /// <param name="valueType"></param>
    /// <returns></returns>
    public static bool CheckConvert(this IPocoOptions options, ref IEmitMemberWriter emitWriter, Type valueType)
    {
        var valueType0 = emitWriter.ValueType;
        if (ReflectionHelper.CheckValueType(valueType, valueType0))
            return false;
        var emitConverter = options.ConverterFactory.Get(valueType, valueType0);
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
        //try
        //{
        //    var body = emit.Write(instance, value);
        //    var lambda = Expression.Lambda<Action<TInstance, TValue>>(body, instance, value);
        //    // 尝试编译
        //    return lambda.Compile();
        //}
        //catch (Exception ex)
        //{
        //    if (emit is DelegateWriter<TInstance, TValue> delegateWriter)
        //    {
        //        var inner = delegateWriter.Inner;
        //        if (inner is FieldWriter fieldWriter)
        //        {
        //            throw new InvalidOperationException($"Failed to compile delegateWriter Inner FieldWriter for {fieldWriter.Name} on {fieldWriter.InstanceType.Name}.", ex);
        //        }
        //        else if (inner is PropertyWriter propertyWriter)
        //        {
        //            throw new InvalidOperationException($"Failed to compile delegateWriter Inner PropertyWriter for {propertyWriter.Name} on {propertyWriter.InstanceType.Name}.", ex);
        //        }
        //        throw new InvalidOperationException($"Failed to compile delegateWriter for {inner.Name} on {inner.InstanceType.Name}.", ex);
        //    }
        //    else if (emit is CompatibleMemberWriter compatibleMemberWriter)
        //    {
        //        throw new InvalidOperationException($"Failed to compile CompatibleMemberWriter for {compatibleMemberWriter.Name} on {compatibleMemberWriter.InstanceType.Name}.", ex);
        //    }
        //    else if (emit is ConvertMemberWriter convertMemberWriter)
        //    {
        //        var converter = convertMemberWriter.Converter;
        //        if(converter is DelegateConverter<int?, int> delegateConverter)
        //        {
        //            throw new InvalidOperationException($"Failed to compile ConvertMemberWriter for {convertMemberWriter.Name} with Converter with {delegateConverter.Method} on {convertMemberWriter.InstanceType.Name}.", ex);
        //        }
        //        throw new InvalidOperationException($"Failed to compile ConvertMemberWriter for {convertMemberWriter.Name} with Converter of {converter.GetType()} on {convertMemberWriter.InstanceType.Name}.", ex);
        //    }
        //    else if (emit is FieldWriter fieldWriter)
        //    {
        //        throw new InvalidOperationException($"Failed to compile FieldWriter for {fieldWriter.Name} on {fieldWriter.InstanceType.Name}.", ex);
        //    }
        //    else if (emit is PropertyWriter propertyWriter)
        //    {
        //        throw new InvalidOperationException($"Failed to compile PropertyWriter for {propertyWriter.Name} on {propertyWriter.InstanceType.Name}.", ex);
        //    }
        //    // 编译失败，抛出异常
        //    throw new InvalidOperationException($"Failed to compile member writer for {emit.Name} of {emit.GetType()} on {emit.InstanceType.Name}.", ex);
        //}
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
