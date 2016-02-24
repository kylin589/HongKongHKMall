using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.WebLogin.Impl
{
    public class ZJ_UserBalanceService : BaseService, IZJ_UserBalanceService
    {
        /// <summary>
        /// 资金表插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel AddBalance(ZJ_UserBalanceModel model)
        {
            var tb = base._database.Db.ZJ_UserBalance;
            var result = new ResultModel()
            {
                Data = tb.Insert(model)
            };
            return result;
        }
    }
}
