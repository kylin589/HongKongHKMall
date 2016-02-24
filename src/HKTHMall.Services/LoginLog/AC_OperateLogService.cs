using BrCms.Framework.Collections;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.LoginLog;
using Simple.Data;

namespace HKTHMall.Services.LoginLog
{
    public class AC_OperateLogService : BaseService, IAC_OperateLogService
    {
        /// <summary>
        ///     添加系统操作日志
        /// </summary>
        /// <param name="model">系统操作日志模型</param>
        /// <returns>是否成功</returns>
        public ResultModel AddAC_OperateLog(AC_OperateLogModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.AC_OperateLog.Insert(model)
            };
            return result;
        }

        /// <summary>
        ///     根据系统操作日志id获取系统操作日志
        /// </summary>
        /// <param name="id">类别id</param>
        /// <returns>系统操作日志模型</returns>
        public ResultModel GetAC_OperateLogById(int? OperateID)
        {
            var result = new ResultModel
            {
                Data = _database.Db.AC_OperateLog.Get(OperateID)
            };

            return result;
        }

        /// <summary>
        ///     获取系统操作日志列表
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <returns>系统操作日志列表</returns>
        public ResultModel GetAC_OperateLogList(SearchAC_OperateLogModel model)
        {
            var tb = _database.Db.AC_OperateLog;
            var where= new SimpleExpression(1, 1, SimpleExpressionType.Equal);

            if (! string.IsNullOrEmpty(model.OperateName))
            {
                where = tb.OperateName.Like("%" + model.OperateName.Trim() + "%"); //操作人
            }

            if (model.BeginOperateTime != null)
            {
                //查询开始时间
                where = new SimpleExpression(where, tb.OperateTime >= model.BeginOperateTime, SimpleExpressionType.And);
            }

            if (model.EndOperateTime != null)
            {
                //结束 时间
                where = new SimpleExpression(where, tb.OperateTime < model.EndOperateTime, SimpleExpressionType.And);
            }

            var result = new ResultModel
            {
                Data =
                    new SimpleDataPagedList<AC_OperateLogModel>(_database.Db.AC_OperateLog.FindAll(where).OrderByOperateTimeDescending(),
                        model.PagedIndex, model.PagedSize)
            };

            return result;
        }

        /// <summary>
        ///     更新系统操作日志
        /// </summary>
        /// <param name="model">系统操作日志模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateAC_OperateLog(AC_OperateLogModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.AC_OperateLog.Update(model)
            };
            result.IsValid = result.Data > 0 ? true : false;
            return result;
        }

        /// <summary>
        ///     删除系统操作日志
        /// </summary>
        /// <param name="model">系统操作日志模型</param>
        /// <returns>是否删除成功</returns>
        public ResultModel DeleteAC_OperateLog(AC_OperateLogModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.AC_OperateLog.Delete(OperateID: model.OperateID)
            };
            result.IsValid = result.Data > 0 ? true : false;
            return result;
        }
    }
}