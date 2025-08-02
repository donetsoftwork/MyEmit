namespace PocoEmit.Helpers;

/// <summary>
/// 映射辅助
/// </summary>
public partial class MapHelper<TSource, TDest>
{
    /// <summary>
    /// 映射源成员
    /// </summary>
    public class SourceMemberHelper
    {
        /// <summary>
        /// 映射源成员
        /// </summary>
        /// <param name="sourceHelper"></param>
        /// <param name="name"></param>
        internal SourceMemberHelper(SourceHelper sourceHelper, string name)
        {
            _sourceHelper = sourceHelper;
            _name = name;
        }
        private readonly SourceHelper _sourceHelper;
        private readonly string _name;

        /// <summary>
        /// 映射目标
        /// </summary>
        /// <param name="destName"></param>
        /// <returns></returns>
        public SourceHelper MapTo(string destName)
            => _sourceHelper.MapTo(_name, destName);
        /// <summary>
        /// 忽略源成员
        /// </summary>
        /// <returns></returns>
        public SourceHelper Ignore()
            => _sourceHelper.Ignore(_name);
    }
}
