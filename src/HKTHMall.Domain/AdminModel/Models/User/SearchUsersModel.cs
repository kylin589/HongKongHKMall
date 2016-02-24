using BrCms.Framework.Collections;

namespace HKTHMall.Domain.Models.User
{
    /// <summary>
    /// 用户列表搜索类
    /// zhoub 20150707
    /// </summary>
    public class SearchUsersModel:Paged
    {
        /// <summary>
        /// 用户帐号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public int UserMode { get; set; }
    }
}
