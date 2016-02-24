using BrCms.Core.Infrastructure;
using BrCms.Framework.Collections;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.YHUser;
using HKTHMall.Domain.WebModel.Models.Users;
using HKTHMall.Domain.WebModel.Models.YH;

namespace HKTHMall.Services.YHUser
{
    /// <summary>
    /// 商城用户
    /// zhoub 20150714
    /// </summary>
    public interface IYH_UserVisitingCardService : IDependency
    {      

        /// <summary>
        /// 获取用户二维码信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        ResultModel findByUserid(long userid);
        /// <summary>
        /// 新增用户二维码信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        ResultModel Add(dynamic model);//YH_UserVisitingCard  刘文宁 修改 2015/9/2
        /// <summary>
        /// 修改用户二维码信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        ResultModel Update(dynamic model);//YH_UserVisitingCard  刘文宁 修改 2015/9/2
        /// <summary>
        /// 删除用户二维码信息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        ResultModel Delete(long userid);
        /// <summary>
        /// 更新用户二维码生成状态
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="status">二维码生成状态</param>
        /// <returns></returns>
        ResultModel UpdateStatus(long id,byte status);

     
    }
}
