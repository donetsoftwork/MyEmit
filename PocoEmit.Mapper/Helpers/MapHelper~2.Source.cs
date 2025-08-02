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
        /// 映射源
        /// </summary>
        /// <param name="helper"></param>
        internal SourceHelper(MapHelper<TSource, TDest> helper)
        {
            _helper = helper;
        }
        private readonly MapHelper<TSource, TDest> _helper;

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
            _helper.IgnoreSource(name);
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
