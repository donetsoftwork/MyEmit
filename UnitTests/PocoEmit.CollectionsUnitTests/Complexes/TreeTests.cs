using PocoEmit.CollectionsUnitTests.Supports;
using PocoEmit.Configuration;
using PocoEmit.Resolves;
using System.Linq.Expressions;

namespace PocoEmit.CollectionsUnitTests.Complexes;

public class TreeTests : CollectionTestBase
{
    [Fact]
    public void ConvertFuncTree()
    {
        var tree = CreateTree();
        var trunk = tree.Trunk;
        var branch1 = trunk.Branches[0];
        var func = _mapper.GetConvertFunc<Tree, TreeDTO>();
        TreeDTO dto = func(tree);
        Assert.NotNull(dto);
        var branches = dto.Trunk.Branches;
        Assert.NotNull(branches);
        Assert.Equal(trunk.Branches.Length, branches.Length);
        var dtoBranch1 = branches[0];
        Assert.NotNull(dtoBranch1);
        var dtoLeaves = dtoBranch1.Leaves;
        Assert.NotNull(dtoLeaves);
        Assert.Equal(branch1.Leaves.Length, dtoLeaves.Length);
    }
    [Fact]
    public void ConvertFuncTreeBranch()
    {
        var trunk = CreateTreeBranch();
        var branch1 = trunk.Branches[0];
        var func = _mapper.GetConvertFunc<TreeBranch, TreeBranchDTO>();
        TreeBranchDTO dto = func(trunk);
        Assert.NotNull(dto);
        var branches = dto.Branches;
        Assert.NotNull(branches);
        Assert.Equal(trunk.Branches.Length, branches.Length);
        var dtoBranch1 = dto.Branches[0];
        Assert.NotNull(dtoBranch1);
        var dtoLeaves = dtoBranch1.Leaves;
        Assert.NotNull(dtoLeaves);
        Assert.Equal(branch1.Leaves.Length, dtoLeaves.Length);
    }
    [Fact]
    public void ConvertFuncTreeBranchByInvoke()
    {
        var trunk = CreateTreeBranch();
        var branch1 = trunk.Branches[0];

        var mapper = Mapper.Create(new MapperOptions { LambdaInvoke = true });
        mapper.UseCollection();
        var func = mapper.GetConvertFunc<TreeBranch, TreeBranchDTO2>();
        TreeBranchDTO2 dto = func(trunk);
        Assert.NotNull(dto);
        var branches = dto.Branches;
        Assert.NotNull(branches);
        Assert.Equal(trunk.Branches.Length, branches.Length);
        var dtoBranch1 = dto.Branches[0];
        Assert.NotNull(dtoBranch1);
        var dtoLeaves = dtoBranch1.Leaves;
        Assert.NotNull(dtoLeaves);
        Assert.Equal(branch1.Leaves.Length, dtoLeaves.Length);
    }
    [Fact]
    public void ConvertFastTreeBranch()
    {
        var trunk = CreateTreeBranch();
        var branch1 = trunk.Branches[0];
        var expression = _mapper.BuildConverter<TreeBranch, TreeBranchDTO>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var func = FastExpressionCompiler.ExpressionCompiler.CompileFast(expression);
        var dto = func(trunk);
        Assert.NotNull(dto);
        //var dto2 = ToDTO(trunk);
        var branches = dto.Branches;
        Assert.NotNull(branches);
        Assert.Equal(trunk.Branches.Length, branches.Length);
        var dtoBranch1 = dto.Branches[0];
        Assert.NotNull(dtoBranch1);
        var dtoLeaves = dtoBranch1.Leaves;
        Assert.NotNull(dtoLeaves);
        Assert.Equal(branch1.Leaves.Length, dtoLeaves.Length);

    }
    [Fact]
    public void GetContextConvertFunc()
    {
        var converter = _mapper.GetEmitContextConverter<TreeBranch, TreeBranchDTO>();
        Assert.NotNull(converter);
        var expression = converter.Build();
        Assert.NotNull(expression);
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var convertFunc = _mapper.GetContextConvertFunc<TreeBranch, TreeBranchDTO>();
        Assert.NotNull(convertFunc);
        var trunk = CreateTreeBranch();
        var branch1 = trunk.Branches[0];
        using var content = SingleContext<TreeBranch, TreeBranchDTO>.Pool.Get();
        var dto = convertFunc(content, trunk);
        Assert.NotNull(dto);
        //var dto2 = ToDTO(trunk);
        var branches = dto.Branches;
        Assert.NotNull(branches);
        Assert.Equal(trunk.Branches.Length, branches.Length);
        var dtoBranch1 = dto.Branches[0];
        Assert.NotNull(dtoBranch1);
        var dtoLeaves = dtoBranch1.Leaves;
        Assert.NotNull(dtoLeaves);
        Assert.Equal(branch1.Leaves.Length, dtoLeaves.Length);
    }
    [Fact]
    public void GetContextConvertFunc2()
    {
        var mapper = Mapper.Create(new MapperOptions { Cached = ComplexCached.Always, LambdaInvoke = true })
            .UseCollection();
        var converter = mapper.GetEmitContextConverter<TreeBranch, TreeBranchDTO>();
        Assert.NotNull(converter);
        var expression = converter.Build();
        Assert.NotNull(expression);
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var convertFunc = mapper.GetContextConvertFunc<TreeBranch, TreeBranchDTO>();
        Assert.NotNull(convertFunc);
        var trunk = CreateTreeBranch();
        var branch1 = trunk.Branches[0];
        using var content = SingleContext<TreeBranch, TreeBranchDTO>.Pool.Get();
        var dto = convertFunc(content, trunk);
        Assert.NotNull(dto);
        //var dto2 = ToDTO(trunk);
        var branches = dto.Branches;
        Assert.NotNull(branches);
        Assert.Equal(trunk.Branches.Length, branches.Length);
        var dtoBranch1 = dto.Branches[0];
        Assert.NotNull(dtoBranch1);
        var dtoLeaves = dtoBranch1.Leaves;
        Assert.NotNull(dtoLeaves);
        Assert.Equal(branch1.Leaves.Length, dtoLeaves.Length);
    }

    public static TreeBranch CreateTreeBranch()
    {
        var trunk = new TreeBranch { Id = 1 };
        var branch1 = new TreeBranch { Id = 2 };
        var branch2 = new TreeBranch { Id = 3 };
        trunk.Branches = [branch1, branch2];
        var leaf11 = new TreeLeaf { Id = 11 };
        var leaf12 = new TreeLeaf { Id = 12 };
        branch1.Leaves = [leaf11, leaf12];
        var leaf21 = new TreeLeaf { Id = 21 };
        var leaf22 = new TreeLeaf { Id = 22 };
        branch2.Leaves = [leaf21, leaf22];
        return trunk;
    }
    public static Tree CreateTree()
    {
        var trunk = CreateTreeBranch();
        var tree = new Tree { Id = 1, Trunk = trunk };
        return tree;
    }
}
