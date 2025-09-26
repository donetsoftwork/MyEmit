using PocoEmit.CollectionsUnitTests.Supports;
using PocoEmit.Configuration;

namespace PocoEmit.CollectionsUnitTests.Complexes;

public class DepartmentTests : CollectionTestBase
{
    [Fact]
    public void ConvertDepartment()
    {
        var dept = CreateDepartment();
        var dto = _mapper.Convert<Department, DepartmentDTO>(dept);
        AssertDepartment(dept, dto);
        var dtoChildren = dto.Children;
        Assert.NotNull(dtoChildren);
        var count = dtoChildren.Length;
        Assert.Equal(dept.Children.Length, count);
        for (int i = 0; i < count; i++)
            AssertDepartment(dept.Children[i], dtoChildren[i]);
    }
    [Fact]
    public void ConvertNoCache()
    {
        var dept = CreateDepartment();
        var mapper = Mapper.Create(new MapperOptions { Cached = ComplexCached.Never });
        mapper.UseCollection();
        var expression = mapper.BuildConverter<Department, DepartmentDTO>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var fastFunc = FastExpressionCompiler.ExpressionCompiler.CompileFast(expression);
        var dto = fastFunc(dept);
        AssertDepartment(dept, dto);
        var dtoChildren = dto.Children;
        Assert.NotNull(dtoChildren);
        var count = dtoChildren.Length;
        Assert.Equal(dept.Children.Length, count);
        for (int i = 0; i < count; i++)
            AssertDepartment(dept.Children[i], dtoChildren[i]);
        var convertFunc = mapper.GetConvertFunc<Department, DepartmentDTO>();
        var dto2 = convertFunc(dept);
        AssertDepartment(dept, dto2);
    }
    [Fact]
    public void ConvertNoneCollection()
    {
        var dept = CreateDepartment();
        var mapper = Mapper.Create(new MapperOptions { Cached = ComplexCached.Never });
        var expression = mapper.BuildConverter<Department, DepartmentDTO>();
        var code = FastExpressionCompiler.ToCSharpPrinter.ToCSharpString(expression);
        Assert.NotNull(code);
        var fastFunc = FastExpressionCompiler.ExpressionCompiler.CompileFast(expression);
        var dto = fastFunc(dept);
        AssertDepartment(dept, dto);
        Assert.Null(dto.Children);
        var convertFunc = mapper.GetConvertFunc<Department, DepartmentDTO>();
        var dto2 = convertFunc(dept);
        AssertDepartment(dept, dto2);
    }
    /// <summary>
    /// 断言
    /// </summary>
    /// <param name="dept"></param>
    /// <param name="dto"></param>
    private void AssertDepartment(Department dept, DepartmentDTO dto)
    {
        Assert.NotNull(dto);
        Assert.Equal(dept.Name, dto.Name);
    }

    private static Department CreateDepartment()
    {
        var dept1 = new Department() { Name = "技术部" };
        var dept2 = new Department() { Name = "市场部" };
        var dept3 = new Department() { Name = "人事部" };

        return new Department
        {
            Name = "总部",
            Children = [dept1, dept2, dept3],
        };
    }
}
