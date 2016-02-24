using BrCms.Core.Infrastructure;
using BrCms.Framework.Collections;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.User;

namespace HKTHMall.Services.Users
{
    /// <summary>
    /// 用户接口
    /// zhoub 20150707
    /// </summary>
    public interface IAC_UserService : IDependency
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="model">用户模型</param>
        ResultModel AddAC_User(AC_UserModel model);

        /// <summary>
        /// 根据用户id获取用户
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns>用户模型</returns>
        ResultModel GetAC_UserById(long id);

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="model">用户模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateAC_User(AC_UserModel model);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns>是否删除成功</returns>
        ResultModel DeleteAC_UserById(long id);

        /// <summary>
        /// 分页获取用户列表
        /// </summary>
        /// <param name="model">用户搜索模型</param>
        /// <returns>用户列表数据</returns>
        ResultModel GetPagingAC_Users(SearchUsersModel model);

        /// <summary>
        /// 根据用户名查询用户信息
        /// zhoub 20150707
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        ResultModel GetAC_UserByUserName(string userName);

        /// <summary>
        /// 根据用户名查询用户信息和部门信息(吴育富)
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>用户列表数据</returns>
        ResultModel GetAC_UserDepartmentByUserName(SearchUsersModel model);

        /// <summary>
        /// 用户密码重置
        /// zhoub 20150707
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResultModel ReSetAC_UserPassword(long userId, string password);

        //Result GetUserById(int id);
        //Result Add(AC_UserModel model);
        //Result SearchUsers(SearchUsersModel model);

        //Result Delete(int id);
    }
}
