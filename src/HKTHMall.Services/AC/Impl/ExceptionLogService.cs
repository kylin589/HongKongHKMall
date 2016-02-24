using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.AC;
using HKTHMall.Domain.Models;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple.Data.RawSql;
using HKTHMall.Core.Extensions;

namespace HKTHMall.Services.AC.Impl
{
    public class ExceptionLogService : BaseService,IExceptionLogService
    {
        /// <summary>
        /// 添加信息
        /// zhoub 20150902
        /// </summary>
        public ResultModel Add(ExceptionLogModel model)
        {
            ResultModel result = new ResultModel();
            var count=0;
            if(model.HandleId!="0")
            {
                count = _database.Db.ExceptionLog.GetCount(_database.Db.ExceptionLog.HandleId == model.HandleId && _database.Db.ExceptionLog.ServiceName == model.ServiceName);
            }
            if (count == 0)
            {
                model.CreateDT = DateTime.Now;
                result.Data = base._database.Db.ExceptionLog.Insert(model);
            }
            return result;
        }

        /// <summary>
        /// 更新信息
        /// zhoub 20150902
        /// </summary>
        /// <param name="model">角色模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel Update(ExceptionLogModel model)
        {
            var result = new ResultModel
            {
                Data = base._database.Db.ExceptionLog.UpdateByElId(ElId: model.ElId, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT, UpdateResult: model.UpdateResult, Status:model.Status)
            };
            if (result.Data > 0)
            {
                result.Messages.Add("Successful operation");//操作成功
            }
            else {
                result.Messages.Add("Operation failure");//操作失败
            }
            return result;
        }

        /// <summary>
        /// 分页查询
        /// zhoub 20150902
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel GetExceptionLogList(SearchExceptionLogModel model)
        {
            var tb = _database.Db.ExceptionLog;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);

            if (model.ElId > 0)
            {
                where = new SimpleExpression(where, tb.ElId == model.ElId, SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.ServiceName))
            {
                where = tb.ServiceName.Like("%" + model.ServiceName.Trim() + "%");
            }
            if (model.Status>0)
            {
                where = new SimpleExpression(where, tb.Status ==model.Status, SimpleExpressionType.And);
            }
            if (model.ResultType > 0)
            {
                where = new SimpleExpression(where, tb.ResultType == model.ResultType, SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.HandleId))
            {
                where = new SimpleExpression(where, tb.HandleId == model.HandleId, SimpleExpressionType.And);
            }
            if (model.BeginOperateTime != null)
            {
                //查询开始时间
                where = new SimpleExpression(where, tb.CreateDT >= model.BeginOperateTime, SimpleExpressionType.And);
            }

            if (model.EndOperateTime != null)
            {
                //结束 时间
                where = new SimpleExpression(where, tb.CreateDT < Convert.ToDateTime(model.EndOperateTime).AddDays(1), SimpleExpressionType.And);
            }

            var result = new ResultModel
            {
                Data =
                    new SimpleDataPagedList<ExceptionLogModel>(tb.FindAll(where).OrderByCreateDTDescending(),
                        model.PagedIndex, model.PagedSize)
            };

            return result;
        }

        /// <summary>
        /// 服务概况查询
        /// zhoub 20150902
        /// </summary>
        /// <returns></returns>
        public ResultModel GetExceptionLogSurvey()
        {
            ResultModel result = new ResultModel();
            string sql = "select * from ExceptionLog t1 where t1.elId in(select max(t2.elId) from ExceptionLog t2  where t2.handleId='0' group by t2.serviceName) order by t1.serviceName";
            List<dynamic> sourcesExceptionLog = _database.RunSqlQuery(x => x.ToResultSets(sql))[0];
            List<ExceptionLogModel> listExceptionLog = sourcesExceptionLog.ToEntity<ExceptionLogModel>();
            result.Data = listExceptionLog;
            return result;
        }
    }
}
