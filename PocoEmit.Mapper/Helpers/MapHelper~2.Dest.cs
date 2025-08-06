using PocoEmit.Maping;

namespace PocoEmit.Helpers;

/// <summary>
/// 映射辅助
/// </summary>
public partial class MapHelper<TSource, TDest>
{
    /// <summary>
    /// 映射目标
    /// </summary>
    public class DestHelper
    {
        /// <summary>
        /// 映射目标
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="recognizer"></param>
        internal DestHelper(MapHelper<TSource, TDest> helper, IgnoreRecognizer recognizer)
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
        /// <param name="name"></param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public DestHelper MapFrom(string name, string sourceName)
        {
            _helper.SetMemberMap(sourceName, name);
            return this;
        }
        /// <summary>
        /// 忽略目标成员
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DestHelper Ignore(string name)
        {
            _recognizer.Ignore(name);
            return this;
        }
        /// <summary>
        /// 添加前缀
        /// </summary>
        /// <param name="prefix"></param>
        public DestHelper AddPrefix(string prefix)
        {
            _recognizer.AddPrefix(prefix);
            return this;
        }
        /// <summary>
        /// 清空前缀(主要用于清理默认前缀)
        /// </summary>
        public DestHelper ClearPrefix()
        {
            _recognizer.ClearPrefix();
            return this;
        }
        /// <summary>
        /// 添加后缀
        /// </summary>
        /// <param name="suffix"></param>
        public DestHelper AddSuffix(string suffix)
        {
            _recognizer.AddSuffix(suffix);
            return this;
        }
        #region Member
        /// <summary>
        /// 指定成员名
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public DestMemberHelper ForMember(string memberName)
            => new(this, memberName);
        #endregion
    }
}