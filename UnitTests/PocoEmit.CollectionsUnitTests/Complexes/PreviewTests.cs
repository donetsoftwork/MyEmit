using PocoEmit.CollectionsUnitTests.Supports;
using PocoEmit.Complexes;
using PocoEmit.Configuration;

namespace PocoEmit.CollectionsUnitTests.Complexes;

public class PreviewTests : CollectionTestBase
{
    [Fact]
    public void UserPreview()
    {
        var converter = _mapper.GetEmitConverter<User, UserDTO>();
        var context = new BuildContext((IMapperOptions)_mapper);
        context.Visit(converter);
        PreviewTests.CheckSingle(context);
    }
    private static void CheckSingle(BuildContext context)
    {
        context.CheckConvertContext();
        var collections = context.Collections;
        Assert.Single(collections);
        var bundle = collections.First();
        Assert.False(bundle.IsCircle);
        Assert.False(bundle.HasCircle);
        Assert.False(bundle.IsCache);
        Assert.False(bundle.HasCircle);
    }
    [Fact]
    public void UserPreview2()
    {
        var converter = _mapper.GetEmitConverter<User?, UserDTO>();
        var context = new BuildContext((IMapperOptions)_mapper);
        context.Visit(converter);
        CheckSingle(context);
    }
    [Fact]
    public void VisitUser()
    {
        var context = new BuildContext((IMapperOptions)_mapper);
        context.Visit<User, UserDTO>();
        CheckSingle(context);
    }
    [Fact]
    public void Visitint()
    {
        var context = new BuildContext((IMapperOptions)_mapper);
        context.Visit<int, string>();
        Assert.Empty(context.Collections);
    }
    [Fact]
    public void VisitNode()
    {
        var context = new BuildContext((IMapperOptions)_mapper);
        context.Visit<Node, NodeDTO>();
        context.CheckConvertContext();
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
        var context = new BuildContext((IMapperOptions)_mapper);
        context.Visit<Tree, TreeDTO>();
        context.CheckConvertContext();
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
        var context = new BuildContext((IMapperOptions)_mapper);
        context.Visit<Tree2, TreeDTO2>();
        context.CheckConvertContext();
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
        var context = new BuildContext((IMapperOptions)_mapper);
        context.Visit<TreeBranch2, TreeBranchDTO2>();
        context.CheckConvertContext();
        Assert.Equal(5, context.Collections.Count());
        var branch = context.GetBundle<TreeBranch2, TreeBranchDTO2>();
        Assert.NotNull(branch);
        Assert.True(branch.IsCircle);
        var leaf = context.GetBundle<TreeLeaf2, TreeLeafDTO2>();
        Assert.NotNull(leaf);
        Assert.True(leaf.IsCircle);
    }
}
