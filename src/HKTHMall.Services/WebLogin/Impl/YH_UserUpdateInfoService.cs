using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.WebLogin.Impl
{
    public class YH_UserUpdateInfoService : BaseService, IYH_UserUpdateInfoService
    {
        /// <summary>
        /// 用户账号系统更新信息表插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel Add(YH_UserUpdateInfoModel model)
        {
            var tb = base._database.Db.YH_UserUpdateInfo;
            var result = new ResultModel()
            {
                Data = tb.Insert(model)
            };
            return result;
        }
    }
}
