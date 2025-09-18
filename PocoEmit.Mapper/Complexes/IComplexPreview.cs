using System.Collections.Generic;

namespace PocoEmit.Complexes;

/// <summary>
/// 复杂类型预览
/// </summary>
public interface IComplexPreview
{
    /// <summary>
    /// 预览
    /// </summary>
    /// <param name="parent"></param>
    IEnumerable<ComplexBundle> Preview(IComplexBundle parent);
}
