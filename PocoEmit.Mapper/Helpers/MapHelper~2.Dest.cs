using PocoEmit.Builders;
using PocoEmit.Maping;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

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
            _members = helper._mapper.MemberCacher.Get(typeof(TDest)).WriteMembers;
        }
        /// <summary>
        /// 映射辅助
        /// </summary>
        private readonly MapHelper<TSource, TDest> _helper;
        /// <summary>
        /// 识别器
        /// </summary>
        private readonly IgnoreRecognizer _recognizer;
        private readonly IDictionary<string, MemberInfo> _members;
        /// <summary>
        /// 验证属性是否合法
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        internal void GuardMember(string name)
        {
            if(!_members.ContainsKey(name)) 
                throw new KeyNotFoundException(name);
        }
        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        internal MemberInfo GetMember(string name)
            => _members[name] ?? throw new KeyNotFoundException(name);
        /// <summary>
        /// 映射
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public DestHelper MapFrom(string name, string sourceName)
        {
            GuardMember(name);
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
            GuardMember(name);
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
        #region UseDefault
        /// <summary>
        /// 配置类型默认值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="member"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal DestHelper UseDefault<TValue>(MemberInfo member, TValue value)
        {
            _helper._mapper.Configure(member, ConstantBuilder.Use(value, typeof(TValue)));
            return this;
        }
        /// <summary>
        /// 配置类型默认值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="member"></param>
        /// <param name="valueFunc"></param>
        /// <returns></returns>
        internal DestHelper UseDefault<TValue>(MemberInfo member, Expression<Func<TValue>> valueFunc)
        {
            _helper._mapper.Configure(member, new FuncBuilder(valueFunc));
            return this;
        }
        /// <summary>
        /// 配置类型默认值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="member"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DestHelper UseDefault<TValue>(string member, TValue value)
            => UseDefault(GetMember(member), value);
        /// <summary>
        /// 配置类型默认值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="member"></param>
        /// <param name="valueFunc"></param>
        /// <returns></returns>
        public DestHelper UseDefault<TValue>(string member, Expression<Func<TValue>> valueFunc)
            => UseDefault(GetMember(member), valueFunc);
        #endregion
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