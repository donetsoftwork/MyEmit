namespace TestApi2.Contract.Models;

/// <summary>
/// 实体接口
/// </summary>
/// <typeparam name="TEntityId"></typeparam>
public interface IEntity<out TEntityId>
{
    /// <summary>
    /// 实体标识
    /// </summary>
    TEntityId Id { get; }
}
