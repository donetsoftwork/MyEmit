using PocoEmit.Maping;

namespace PocoEmit.Helpers;

/// <summary>
/// 映射辅助
/// </summary>
public partial class MapHelper<TSource, TDest>
{
    /// <summary>
    /// 映射源
    /// </summary>
    public class SourceHelper
    {
        /// <summary>
        /// 映射目标
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="recognizer"></param>
        internal SourceHelper(MapHelper<TSource, TDest> helper, IgnoreRecognizer recognizer)
        {
            _helper = helper;
            _recognizer = recognizer;
        }
        /// <summary>
        /// 映射辅助
        /// </summary>
        private readonly MapHelper<TSource, TDest> _helper;
        /// <summary>
        /// 识别器
        /// </summary>
        private readonly IgnoreRecognizer _recognizer;
        /// <summary>
        /// 映射
        /// </summary>
        /// <param name="sourceName"></param>
        /// <param name="destName"></param>
        /// <returns></returns>
        public SourceHelper MapTo(string sourceName,  string destName)
        {
            _helper.SetMemberMap(sourceName, destName);
            return this;
        }
        /// <summary>
        /// 忽略源成员
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SourceHelper Ignore(string name)
        {
            _recognizer.Ignore(name);
            return this;
        }
        /// <summary>
        /// 添加前缀
        /// </summary>
        /// <param name="prefix"></param>
        public SourceHelper AddPrefix(string prefix)
        {
            _recognizer.AddPrefix(prefix);
            return this;
        }
        /// <summary>
        /// 清空前缀(主要用于清理默认前缀)
        /// </summary>
        public SourceHelper ClearPrefix()
        {
            _recognizer.ClearPrefix();
            return this;
        }
        /// <summary>
        /// 添加后缀
        /// </summary>
        /// <param name="suffix"></param>
        public SourceHelper AddSuffix(string suffix)
        {
            _recognizer.AddSuffix(suffix);
            return this;
        }
        #region Helper
        /// <summary>
        /// 指定成员名
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public SourceMemberHelper ForMember(string memberName)
            => new(this, memberName);
        #endregion
    }
}
