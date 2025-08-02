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
        internal DestHelper(MapHelper<TSource, TDest> helper)
        {
            _helper = helper;
        }
        private readonly MapHelper<TSource, TDest> _helper;

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
        /// 忽略源目标
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DestHelper Ignore(string name)
        {
            _helper.IgnoreDest(name);
            return this;
        }
        #region Helper
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