using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.banner;
using HKTHMall.Domain.Models;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Banner
{
    public class FloorConfigService : BaseService, IFloorConfigService
    {
        /// <summary>
        /// 获取楼层配置表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>楼层配置表</returns>
        public ResultModel GetFloorConfigList(SearchFloorConfigModel model)
        {
            var tb = _database.Db.FloorConfig;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal); //
            if (model.FloorConfigId > 0)
            {
                where = new SimpleExpression(where, tb.FloorConfigId == model.FloorConfigId, SimpleExpressionType.And);
            }
            dynamic cy;


            var query = tb
                .Query()
                //.LeftJoin(_database.Db.Category, out cy)
                //.On(_database.Db.Category.CategoryId == tb.ProductId)
                .Select(
                    tb.FloorConfigId,
                    tb.FloorConfigName,
                    tb.CateIdStr
                )
                .Where(where)
                .OrderByFloorConfigId();

            var result = new ResultModel
            {
                Data = new SimpleDataPagedList<FloorConfigModel>(query,
                        model.PagedIndex, model.PagedSize)
            };

            return result;
        }

        /// <summary>
        /// 获取单个实体数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultModel GetModelById(int id)
        {
            var result = new ResultModel
            {
                Data = base._database.Db.FloorConfig.FindByFloorConfigId(id)
            };
            return result;
        }

        /// <summary>
        /// 添加楼层配置表
        /// </summary>
        /// <param name="model">楼层配置表model</param>
        /// <returns>是否成功</returns>
        public ResultModel AddFloorConfig(FloorConfigModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.FloorConfig.Insert(model)
            };
            return result;
        }

        /// <summary>
        /// 修改楼层配置表
        /// </summary>
        /// <param name="model">楼层配置表model</param>
        /// <returns>是否成功</returns>
        public ResultModel UpdateFloorConfig(FloorConfigModel model)
        {
            var result = new ResultModel()
            {
                Data = base._database.Db.FloorConfig.UpdateByFloorConfigId(FloorConfigId: model.FloorConfigId, CateIdStr: model.CateIdStr)
            };

            result.IsValid = result.Data > 0 ? true : false;
            return result;
        }
    }
}
