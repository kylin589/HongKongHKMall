using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.AdminModel.Models.Products;
using BrCms.Framework.Collections;
using Simple.Data;

namespace HKTHMall.Services.Products
{
    public class SalesRuleService : BaseService, ISalesRuleService
    {
        /// <summary>
        /// 通过Id查询促销规则对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        public ResultModel GetSalesRuleById(int id)
        {
            var result = new ResultModel { Data = _database.Db.SalesRule.FindBySalesRuleId(id) };
            return result;
        }

        /// <summary>
        ///促销规则分页查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        public ResultModel Select(SearchSalesRuleModel model)
        {
            var result = new ResultModel
            {
                Data =
                    new SimpleDataPagedList<SalesRuleModel>(
                        _database.Db.SalesRule.FindAll(base._database.Db.SalesRule.RuleName.Like("%" + model.RuleName + "%")),
                        model.PagedIndex, model.PagedSize)
            };
            return result;
        }

        /// <summary>
        /// 添加促销规则
        /// </summary>
        /// <param name="model">促销规则对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        public ResultModel Add(SalesRuleModel model)
        { 
            var result = new ResultModel();
                result.Data = _database.Db.SalesRule.Insert(model);
            return result;
        }

        /// <summary>
        /// 通过Id删除促销规则
        /// </summary>
        /// <param name="id">系统参数id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        public ResultModel Delete(int id)
        {
            var result = new ResultModel { Data = _database.Db.SalesRule.DeleteBySalesRuleId(id) };
            return result;
        }

        /// <summary>
        /// 更新促销规则
        /// </summary>
        /// <param name="model">促销规则对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        public ResultModel Update(SalesRuleModel model)
        {
            var sp = _database.Db.SalesRule;
            var result = new ResultModel
            {
                Data = sp.UpdateBySalesRuleId(model)
            };
            return result;
        }

        /// <summary>
        /// 查询所有的促销规则信息
        /// </summary>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        public ResultModel GetSalesRuleList()
        {
            var result = new ResultModel
            {
                Data = _database.Db.SalesRule.FindAll(_database.Db.SalesRule.SalesRuleId > 0)
            };

            return result;
        }
    }
}
