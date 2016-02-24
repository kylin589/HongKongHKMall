using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.YHUser;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.YHUser
{
    public interface IYH_MerchantInfoService : IDependency
    {
        /// <summary>
        /// 商家添加
        /// zhoub 20150918
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel Add(YH_MerchantInfoModel model);

        /// <summary>
        /// 商家更新
        /// zhoub 20150918
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel Edit(YH_MerchantInfoModel model);

        /// <summary>
        /// 根据ID获取商家信息
        /// zhoub 20150918
        /// </summary>
        /// <param name="merchantID"></param>
        /// <returns></returns>
        ResultModel GetYH_MerchantInfoById(long merchantID);

        /// <summary>
        /// 商家审核
        /// zhoub 20150918
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel AuditYH_MerchantInfo(YH_MerchantInfoModel model);
    }
}
