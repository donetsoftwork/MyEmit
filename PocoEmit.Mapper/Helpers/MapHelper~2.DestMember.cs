namespace PocoEmit.Helpers;

/// <summary>
/// 映射辅助
/// </summary>
public partial class MapHelper<TSource, TDest>
{
	/// <summary>
	/// 映射源成员
	/// </summary>
	public class DestMemberHelper
	{
		/// <summary>
		/// 映射目标成员
		/// </summary>
		/// <param name="sourceHelper"></param>
		/// <param name="name"></param>
		internal DestMemberHelper(DestHelper sourceHelper, string name)
		{
			_sourceHelper = sourceHelper;
			_name = name;
		}
		private readonly DestHelper _sourceHelper;
		private readonly string _name;

		/// <summary>
		/// 映射目标
		/// </summary>
		/// <param name="sourceName"></param>
		/// <returns></returns>
		public DestHelper MapFrom(string sourceName)
			=> _sourceHelper.MapFrom(_name, sourceName);
        /// <summary>
        /// 忽略目标成员
        /// </summary>
        /// <returns></returns>
        public DestHelper Ignore()
			=> _sourceHelper.Ignore(_name);
	}
}