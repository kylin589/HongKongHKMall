using BrCms.Core.Infrastructure;
using BrCms.Framework.Collections;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.User;
using HKTHMall.Domain.WebModel.Models.Login;

namespace HKTHMall.Services.Users
{
    public interface IUserAddressService : IDependency
    {

        /// <summary>
        /// 根据ID获取收货地址信息
        /// zhoub 20150717
        /// </summary>
        /// <param name="userAddressId"></param>
        /// <returns></returns>
        ResultModel GetUserAddressById(long userAddressId);

        /// <summary>
        /// 添加收货地址
        /// zhoub 20150716
        /// </summary>
        /// <param name="model">模型</param>
        ResultModel AddUserAddress(UserAddress model);

        /// <summary>
        /// 更新收货地址
        /// zhoub 20150716
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel UpdateUserAddress(UserAddress model);

        /// <summary>
        /// 常用收货地址修改
        /// zhoub 20150720
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel UpdateUserAddressFlag(UserAddress model);

        /// <summary>
        /// 删除收货地址
        /// zhoub 20150716
        /// </summary>
        /// <param name="userAddressId"></param>
        /// <returns></returns>
        ResultModel DelUserAddress(long userAddressId);

        /// <summary>
        /// 分页获取收获地址列表
        /// zhoub 20150716
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel GetPagingUserAddress(SearchUserAddressModel model, int languageID);


        /// <summary>
        /// 获取用户所有收货地址
        /// </summary>
        /// <param name="model">查询模型</param>
        /// <param name="languageID">语言代码</param>
        /// <returns>地址列表</returns>
        ResultModel GetUserAllAddress(SearchUserAddressModel model, int languageID);

        /// <summary>
        /// 根据区域ID获取区域详细信息
        /// zhoub 20150805
        /// </summary>
        /// <param name="thAreaID">区域ID</param>
        /// <param name="languageID">语言ID</param>
        /// <returns></returns>
        ResultModel GetTHAreaAreaName(long thAreaID, int languageID);
    }
}
