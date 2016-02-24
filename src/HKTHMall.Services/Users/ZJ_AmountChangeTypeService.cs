using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Domain.Models;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Users
{
    public class ZJ_AmountChangeTypeService : BaseService, IZJ_AmountChangeTypeService
    {
        /// <summary>
        /// 获取账户异动类型
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
       public  ResultModel GetZJ_AmountChangeTypeList(SearchZJ_AmountChangeTypeModel model)
        {
            var tb = _database.Db.ZJ_AmountChangeType;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal); //

            if (model.ID>0)
            {
                //id
                where = new SimpleExpression(where, tb.ID == model.ID, SimpleExpressionType.And);
            }
            //if (!string.IsNullOrEmpty(model.TypeName))
            //{
            //    //类型名称
            //    where = new SimpleExpression(where, tb.TypeName.Like("%" + model.TypeName + "%"), SimpleExpressionType.And);
            //}

            var query = tb
                .Query()
                .Select(
                    tb.ID,
                    tb.TypeName,
                    tb.Remark
                )
                .Where(where)
                .OrderByID();

            var result = new ResultModel
            {
                Data = new SimpleDataPagedList<ZJ_AmountChangeTypeModel>(query,
                        model.PagedIndex, model.PagedSize)
            };

            return result;
        }

        /// <summary>
        /// 添加账户异动类型
        /// </summary>
        /// <param name="model">退换用户余额信息表</param>
        /// <returns>是否成功</returns>
        public ResultModel AddZJ_AmountChangeType(ZJ_AmountChangeTypeModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.ZJ_AmountChangeType.Insert(model)
            };
            return result;
        }

        /// <summary>
        /// 更新账户异动类型
        /// </summary>
        /// <param name="model">用户余额信息表模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateZJ_AmountChangeType(ZJ_AmountChangeTypeModel model) {
            var result = new ResultModel()
            {
                Data = base._database.Db.ZJ_UserBalance.UpdateByID(ID: model.ID, Remark: model.Remark, TypeName: model.TypeName)
            };

            result.IsValid = result.Data > 0 ? true : false;

            return result;
        }
    }
}
