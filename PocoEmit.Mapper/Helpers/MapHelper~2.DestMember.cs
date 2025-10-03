using System;
using System.Linq.Expressions;
using System.Reflection;

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
		/// <param name="destHelper"></param>
		/// <param name="name"></param>
		internal DestMemberHelper(DestHelper destHelper, string name)
		{
            _member = destHelper.GetMember(name);
            _destHelper = destHelper;
			_name = name;
		}
		private readonly DestHelper _destHelper;
		private readonly string _name;
        private readonly MemberInfo _member;

		/// <summary>
		/// 映射目标
		/// </summary>
		/// <param name="sourceName"></param>
		/// <returns></returns>
		public DestHelper MapFrom(string sourceName)
			=> _destHelper.MapFrom(_name, sourceName);
        /// <summary>
        /// 忽略目标成员
        /// </summary>
        /// <returns></returns>
        public DestHelper Ignore()
			=> _destHelper.Ignore(_name);
        #region UseDefault
        /// <summary>
        /// 配置类型默认值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public DestHelper UseDefault<TValue>(TValue value)
            => _destHelper.UseDefault(_member, value);
        /// <summary>
        /// 配置类型默认值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="valueFunc"></param>
        /// <returns></returns>
        public DestHelper UseDefault<TValue>(Expression<Func<TValue>> valueFunc)
            => _destHelper.UseDefault(_member, valueFunc);
        #endregion
    }
}