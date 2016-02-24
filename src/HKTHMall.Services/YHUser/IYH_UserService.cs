using BrCms.Core.Infrastructure;
using BrCms.Framework.Collections;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.YHUser;
using HKTHMall.Domain.WebModel.Models.Users;

namespace HKTHMall.Services.YHUser
{
    /// <summary>
    /// 商城用户
    /// zhoub 20150714
    /// </summary>
    public interface IYH_UserService : IDependency
    {
        #region  商城用户

        /// <summary>
        /// 分页获取商城用户列表
        /// </summary>
        /// <param name="model">用户搜索模型</param>
        /// <returns>用户列表数据</returns>
        ResultModel GetPagingYH_User(SearchYHUserModel model);
        /// <summary>
        /// 获取商城用户ID
        /// </summary>       
        /// <returns>用户列表数据</returns>
        int[] GetAllUserId();
        /// <summary>
        /// 删除用户
        /// zhoub 20150714
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        ResultModel DeleteYH_UserByUserID(YH_UserModel model);

        /// <summary>
        /// 更新用户锁定状态
        /// zhoub 20150714
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        ResultModel UpdateYH_UserIsLock(YH_UserModel model);

        /// <summary>
        /// 重置用户登录密码
        /// zhoub 20150714
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        ResultModel UpdateYH_UserPassWord(YH_UserModel model);

        /// <summary>
        /// 重置用户交易密码
        /// zhoub 20150714
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel UpdateYH_UserPayPassWord(YH_UserModel model);

        /// <summary>
        /// 感恩惠粉人数获取
        /// zhoub 20150715
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResultModel GetYH_UserReferrerIDCount(long userId);

        /// <summary>
        /// 根据用户ID获取消费金额、收益金额
        /// zhoub 20150826
        /// </summary>
        /// <param name="type">1 消费金额 2 收益金额</param>
        /// <returns></returns>
        ResultModel GetYH_UserMoney(long userID, int type);

        #endregion

        #region 余额变动记录

        /// <summary>
        /// 分页余额变动记录
        /// </summary>
        /// <param name="model">搜索模型</param>
        /// <returns>列表数据</returns>
        ResultModel GetPagingZJ_UserBalanceChangeLog(SearchUserBalanceChangeModel model);

        #endregion

        

        /// <summary>
        /// 获取数据库表用户信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        ResultModel GetUserInfoByUserId(long userid);


        #region 变更用户信息
       

       
       /// <summary>
        /// 激活邮箱
       /// </summary>
       /// <returns></returns>
        ResultModel ActiveEmail(string email);

      
        #endregion


        #region 与支付相关

        /// <summary>
        /// 获取用户信息（用于支付,配合GetYH_UserForPayment方法使用）
        /// </summary>
        /// <param name="userInfoView">用户信息</param>
        /// <param name="compareUserInfoView">用于比较的用户信息</param>
        /// <returns>是否可以支付</returns>
        ResultModel GetYH_UserForPaymentMessage(UserInfoViewForPayment userInfoView, UserInfoViewForPayment compareUserInfoView);

        /// <summary>
        /// 获取用户信息（用于支付,配合GetYH_UserForPayment方法使用）
        /// </summary>
        /// <param name="userInfoView">用户信息</param>
        /// <param name="compareUserInfoView">用于比较的用户信息</param>
        /// <param name="trans">事务对象</param>
        /// <returns>是否可以支付</returns>
        ResultModel GetYH_UserForPaymentMessage(UserInfoViewForPayment userInfoView,
            UserInfoViewForPayment compareUserInfoView, dynamic trans);

        ///<summary>
        ///获取用户信息（用于支付）
        ///</summary>
        ///<param name="userId">用户Id</param>
        ///<returns>用户信息</returns>
        ResultModel GetYH_UserForPayment(long userId);


        ///<summary>
        ///获取用户信息（用于支付）
        ///</summary>
        ///<param name="userId">用户Id</param>
        /// <param name="trans">事务对象</param>
        ///<returns>用户信息</returns>
        ResultModel GetYH_UserForPayment(long userId, dynamic trans);

        #endregion

        /// <summary>
        /// 添加订阅
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel AddEmailSub(MailSubscriptionModel model);

        /// <summary>
        /// 是否已经订阅过
        /// </summary>
        /// <param name="Email">Email地址</param>
        /// <returns></returns>
        bool HasEmailSubd(string Email);
    }
}
