using System;

namespace PocoEmit.Configuration;

/// <summary>
/// 集合容器配置
/// </summary>
public class CollectionContainerOptions
{
    /// <summary>
    /// 并发
    /// </summary>
    public int ConcurrencyLevel = Environment.ProcessorCount;
    /// <summary>
    /// 集合数量
    /// </summary>
    public int CounterCapacity = 31;
    /// <summary>
    /// 集合访问者数量
    /// </summary>
    public int VisitorCapacity = 31;
    /// <summary>
    /// 索引读取器数量
    /// </summary>
    public int ReadIndexCapacity = 31;
    /// <summary>
    /// 索引访问者数量
    /// </summary>
    public int IndexVisitorCapacity = 31;
    /// <summary>
    /// 元素保存器数量
    /// </summary>
    public int SaverCapacity = 31;
    /// <summary>
    /// 迭代类数量
    /// </summary>
    public int EnumerableCapacity = 31;
    /// <summary>
    /// 集合类数量
    /// </summary>
    public int Collectionapacity = 31;
    /// <summary>
    /// 列表类数量
    /// </summary>
    public int ListCapacity = 31;
    /// <summary>
    /// 字典类数量
    /// </summary>
    public int DictionaryCapacity = 11;
}
