using Hand.Creational;
using PocoEmit.Builders;
using PocoEmit.Configuration;
using System.Linq.Expressions;

namespace PocoEmit.Members;

/// <summary>
/// 表达式适配成员读取器
/// </summary>
public class BuilderReaderAdapter(ICreator<Expression> builder)
    : IEmitReader
{
    #region 配置
    private readonly ICreator<Expression> _builder = builder;
    /// <summary>
    /// 表达式构建器
    /// </summary>
    public ICreator<Expression> Builder
        => _builder;
    /// <inheritdoc />
    bool ICompileInfo.Compiled
        => false;
    #endregion
    /// <inheritdoc />
    Expression IEmitReader.Read(Expression instance)
        => _builder.Create();
}
