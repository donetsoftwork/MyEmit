using PocoEmit.CollectionsUnitTests.Supports;
using PocoEmit.Complexes;
using PocoEmit.Configuration;
using PocoEmit.Converters;
using PocoEmit.Resolves;

namespace PocoEmit.CollectionsUnitTests.Complexes;

public class PreviewTests : CollectionTestBase
{
    [Fact]
    public void UserPreview()
    {
        var converter = _mapper.GetEmitConverter<User, UserDTO>();
        var context = new BuildContext((IMapperOptions)_mapper);
        _ = context.Visit(converter).ToArray();
        Assert.Single(context.Collections);
    }
    [Fact]
    public void UserPreview2()
    {
        var converter = _mapper.GetEmitConverter<User?, UserDTO>();
        var context = new BuildContext((IMapperOptions)_mapper);
        _ = context.Visit(converter)
            .ToArray();
        Assert.Single(context.Collections);
    }
    [Fact]
    public void VisitUser()
    {
        var context = new BuildContext((IMapperOptions)_mapper);
        _ = context.Visit<User, UserDTO>()
            .ToArray();
        Assert.Single(context.Collections);
    }
    [Fact]
    public void Visitint()
    {
        var context = new BuildContext((IMapperOptions)_mapper);
        _ = context.Visit<int, string>()
            .ToArray();
        Assert.Empty(context.Collections);
    }
    [Fact]
    public void VisitNode()
    {
        var context = new BuildContext((IMapperOptions)_mapper);
        _ = context.Visit<Node, NodeDTO>()
            .ToArray();
        context.CheckCircle();
        Assert.Equal(3, context.Collections.Count());
        var node = context.GetBundle<Node, NodeDTO>();
        Assert.NotNull(node);
        Assert.True(node.IsCircle);
        var leaf = context.GetBundle<Leaf, LeafDTO>();
        Assert.NotNull(leaf);
        Assert.True(leaf.IsCircle);
    }
    [Fact]
    public void VisitTree()
    {
        var mapper = Mapper.Create();
        var context = new BuildContext((IMapperOptions)mapper);
        _ = context.Visit<Tree, TreeDTO>()
            .ToArray();
        context.CheckCircle();
        Assert.Equal(5, context.Collections.Count());
        var branch = context.GetBundle<TreeBranch, TreeBranchDTO>();
        Assert.NotNull(branch);
        Assert.True(branch.IsCircle);
        var leaf = context.GetBundle<TreeLeaf, TreeLeafDTO>();
        Assert.NotNull(leaf);
        Assert.False(leaf.IsCircle);
    }
    [Fact]
    public void VisitTree2()
    {
        var mapper = Mapper.Create();
        var context = new BuildContext((IMapperOptions)mapper);
        _ = context.Visit<Tree2, TreeDTO2>()
            .ToArray();
        context.CheckCircle();
        Assert.Equal(5, context.Collections.Count());
        var branch = context.GetBundle<TreeBranch2, TreeBranchDTO2>();
        Assert.NotNull(branch);
        Assert.True(branch.IsCircle);
        var leaf = context.GetBundle<TreeLeaf2, TreeLeafDTO2>();
        Assert.NotNull(leaf);
        Assert.True(leaf.IsCircle);
    }

    [Fact]
    public void VisitTreeBranch2()
    {
        var mapper = Mapper.Create();
        var context = new BuildContext((IMapperOptions)mapper);
        _ = context.Visit<TreeBranch2, TreeBranchDTO2>()
            .ToArray();
        context.CheckCircle();
        Assert.Equal(5, context.Collections.Count());
        var branch = context.GetBundle<TreeBranch2, TreeBranchDTO2>();
        Assert.NotNull(branch);
        Assert.True(branch.IsCircle);
        var leaf = context.GetBundle<TreeLeaf2, TreeLeafDTO2>();
        Assert.NotNull(leaf);
        Assert.True(leaf.IsCircle);
    }
    [Fact] 
    public void ConvertTreeBranch2()
    {
        var trunk = CreateTreeBranch2(new Tree2());
        var branch1 = trunk.Branches[0];
        TreeBranchDTO2 dto = _mapper.Convert<TreeBranch2, TreeBranchDTO2>(trunk);
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
    public void ConvertFuncTreeBranch3()
    {
        var trunk = CreateTreeBranch();
        var branch1 = trunk.Branches[0];

        var mapper = Mapper.Create(new MapperOptions { LambdaInvoke = false });
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
    public void ConvertFuncTree2()
    {
        var tree = CreateTree2();
        var trunk = tree.Trunk;
        var branch1 = trunk.Branches[0];
        var mapper = Mapper.Create();
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
        var mapper = Mapper.Create();
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
        var mapper = Mapper.Create();
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
    public void ConvertFastTreeBranch()
    {
        var trunk = CreateTreeBranch();
        var branch1 = trunk.Branches[0];
        var expression = _mapper.BuildConverter<TreeBranch, TreeBranchDTO>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var func = FastExpressionCompiler.ExpressionCompiler.CompileFast(expression);
        TreeBranchDTO dto = func(trunk);
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
    public void ConvertFastTreeBranch2()
    {
        var trunk = CreateTreeBranch2(new Tree2());
        var branch1 = trunk.Branches[0];
        var expression = _mapper.BuildConverter<TreeBranch2, TreeBranchDTO2>();
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
