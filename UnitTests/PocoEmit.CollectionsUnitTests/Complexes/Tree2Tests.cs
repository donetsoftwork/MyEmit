using PocoEmit.CollectionsUnitTests.Supports;
using PocoEmit.Configuration;
using PocoEmit.Resolves;

namespace PocoEmit.CollectionsUnitTests.Complexes;

public class Tree2Tests
{
    [Fact]
    public void ConvertTreeBranch2()
    {
        var trunk = CreateTreeBranch2(new Tree2());
        var branch1 = trunk.Branches[0];
        var mapper = Mapper.Create(new MapperOptions { Cached = ComplexCached.Circle });
        mapper.UseCollection();
        TreeBranchDTO2 dto = mapper.Convert<TreeBranch2, TreeBranchDTO2>(trunk);
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
    public void ConvertFuncTree2()
    {
        var tree = CreateTree2();
        var trunk = tree.Trunk;
        var branch1 = trunk.Branches[0];
        var mapper = Mapper.Create(new MapperOptions { Cached = ComplexCached.Circle });
        mapper.UseCollection();
        var func = mapper.GetConvertFunc<Tree2, TreeDTO2>();
        TreeDTO2 dto = func(tree);
        Assert.NotNull(dto);
        var trunkDTO = dto.Trunk;
        Assert.NotNull(trunkDTO);
        var branches = trunkDTO.Branches;
        Assert.NotNull(branches);
        Assert.Equal(trunk.Branches.Length, branches.Length);
        var dtoBranch1 = trunkDTO.Branches[0];
        Assert.NotNull(dtoBranch1);
        var dtoLeaves = dtoBranch1.Leaves;
        Assert.NotNull(dtoLeaves);
        Assert.Equal(branch1.Leaves.Length, dtoLeaves.Length);
    }
    [Fact]
    public void ConvertFuncTreeBranch2()
    {
        var trunk = CreateTreeBranch2(new Tree2());
        var branch1 = trunk.Branches[0];
        var mapper = Mapper.Create(new MapperOptions { Cached = ComplexCached.Circle });
        mapper.UseCollection();
        var func = mapper.GetConvertFunc<TreeBranch2, TreeBranchDTO2>();
        TreeBranchDTO2 dto = func(trunk);
        Assert.NotNull(dto);
        Assert.NotNull(dto.Tree);
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
    public void ConvertFuncTreeBranchArray2()
    {
        var trunk = CreateTreeBranch2(new Tree2());
        var branches = trunk.Branches;
        var mapper = Mapper.Create(new MapperOptions { Cached = ComplexCached.Circle });
        mapper.UseCollection();
        var func = mapper.GetConvertFunc<TreeBranch2[], TreeBranchDTO2[]>();
        TreeBranchDTO2[] dto = func(branches);
        Assert.NotNull(dto);
        Assert.Equal(branches.Length, dto.Length);
        var dtoBranch1 = dto[0];
        Assert.NotNull(dtoBranch1);
        var dtoLeaves = dtoBranch1.Leaves;
        Assert.NotNull(dtoLeaves);
        Assert.Equal(branches[0].Leaves.Length, dtoLeaves.Length);
    }

    [Fact]
    public void ConvertFastTreeBranch2()
    {
        var trunk = CreateTreeBranch2(new Tree2());
        var branch1 = trunk.Branches[0];
        var mapper = Mapper.Create(new MapperOptions { Cached = ComplexCached.Circle })
            .UseCollection();
        var expression = mapper.BuildConverter<TreeBranch2, TreeBranchDTO2>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var func = FastExpressionCompiler.ExpressionCompiler.CompileFast(expression);
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
    public void ConvertFuncTreeBranchByInvoke()
    {
        var trunk = CreateTreeBranch2(new Tree2());
        var branch1 = trunk.Branches[0];
        var mapper = Mapper.Create(new MapperOptions { Cached = ComplexCached.Circle })
            .UseCollection();
        var converter = mapper.GetEmitContextConverter<TreeBranch2, TreeBranchDTO2>();
        Assert.NotNull(converter);
        var expression = converter.Create();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var func = mapper.GetContextConvertFunc<TreeBranch2, TreeBranchDTO2>();
        Assert.NotNull(func);
        using var content = ConvertContext.Pool.Get();
        TreeBranchDTO2 dto = func(content, trunk);
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
    public static TreeBranch2 CreateTreeBranch2(Tree2 tree)
    {
        var trunk = new TreeBranch2 { Id = 1, Tree = tree };
        var branch1 = new TreeBranch2 { Id = 2, Parent = trunk, Tree = tree };
        var branch2 = new TreeBranch2 { Id = 3, Parent = trunk, Tree = tree };
        trunk.Branches = [branch1, branch2];
        var leaf11 = new TreeLeaf2 { Id = 11, Branch = branch1 };
        var leaf12 = new TreeLeaf2 { Id = 12, Branch = branch1 };
        branch1.Leaves = [leaf11, leaf12];
        var leaf21 = new TreeLeaf2 { Id = 21, Branch = branch2 };
        var leaf22 = new TreeLeaf2 { Id = 22, Branch = branch2 };
        branch2.Leaves = [leaf21, leaf22];
        return trunk;
    }

    public static Tree2 CreateTree2()
    {
        var tree = new Tree2 { Id = 1 };
        var trunk = CreateTreeBranch2(tree);
        tree.Trunk = trunk;
        return tree;
    }
}
