using TestApi2.Contract.Models;

namespace TestApi2.Contract.Infrastructure;

/// <summary>
/// 仓储接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IRepository<TEntity>
    where TEntity : IEntity<long>
{
    /// <summary>
    /// 按id获取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TEntity?> GetByIdAsync(long id);
    /// <summary>
    /// 插入
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task InsertAsync(TEntity entity);
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="fieldNames"></param>
    /// <returns></returns>
    Task UpdateAsync(TEntity entity, IEnumerable<string> fieldNames);
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task DeleteAsync(TEntity entity);
}
