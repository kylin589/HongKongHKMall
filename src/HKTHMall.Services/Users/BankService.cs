using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Bank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Users
{
    public class BankService:BaseService,IBankService
    {
        /// <summary>
        /// 获取银行卡列表
        /// </summary>
        /// <returns></returns>
        public ResultModel GetBankList()
        {
            var result = new ResultModel();
            result.Data = this._database.RunQuery(db =>
            {
                return db.BD_Bank.All().ToList<BankModel>();
            });
            return result;
        }
    }
}
