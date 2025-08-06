using PocoEmit.Members;
using System.Collections.Generic;

namespace PocoEmit.Maping;

/// <summary>
/// 识别器
/// </summary>
public interface IRecognizer
{
    /// <summary>
    /// 匹配器
    /// </summary>
    INameMatch Matcher { get; }
    /// <summary>
    /// 前缀
    /// </summary>
    IEnumerable<string> Prefixes { get; }
    /// <summary>
    /// 后缀
    /// </summary>
    IEnumerable<string> Suffixes { get; }
    /// <summary>
    /// 添加前缀
    /// </summary>
    /// <param name="prefix"></param>
    void AddPrefix(string prefix);
    /// <summary>
    /// 添加后缀
    /// </summary>
    /// <param name="suffix"></param>
    void AddSuffix(string suffix);
    /// <summary>
    /// 识别成员
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    IEnumerable<IMember> Recognize(IMember member);
    /// <summary>
    /// 识别读取器
    /// </summary>
    /// <param name="readers"></param>
    /// <returns></returns>
    IEnumerable<IEmitMemberReader> Recognize(IEnumerable<IEmitMemberReader> readers);
}
