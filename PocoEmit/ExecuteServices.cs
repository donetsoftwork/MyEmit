using Hand.Creational;
using PocoEmit.Builders;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Members;
using System;
using System.Linq.Expressions;

namespace PocoEmit;

/// <summary>
/// 执行扩展方法
/// </summary>
public static partial class PocoEmitServices
{
    /// <summary>
    /// 执行ICreator
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="creator"></param>
    /// <returns></returns>
    public static Expression Execute(this IEmitBuilder builder, ICreator<Expression> creator)
    {
        if(creator is IEmitExecuter executer)
            return executer.Execute(builder);
        return creator.Create();
    }
    /// <summary>
    /// 执行IEmitConverter
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="converter"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static Expression Execute(this IEmitBuilder builder, IEmitConverter converter, Expression source)
    {
        if (converter is IArgumentExecuter executer)
            return executer.Execute(builder, source);
        return converter.Convert(source);
    }
    /// <summary>
    /// 执行IEmitReader
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="reader"></param>
    /// <param name="instance"></param>
    /// <returns></returns>
    public static Expression Execute(this IEmitBuilder builder, IEmitReader reader, Expression instance)
    {
        if (reader is IArgumentExecuter executer)
            return executer.Execute(builder, instance);
        return reader.Read(instance);
    }
}
