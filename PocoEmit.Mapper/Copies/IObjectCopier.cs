namespace PocoEmit.Copies;

/// <summary>
/// 弱类型复制器
/// </summary>
public interface IObjectCopier
{
    /// <summary>
    /// 复制
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    void CopyObject(object from, object to);
}
