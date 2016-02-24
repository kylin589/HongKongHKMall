using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.AdminModel.Models.YHUser;
using BrCms.Framework.Collections;
using Simple.Data;


namespace HKTHMall.Services.YHUser
{
    public class ReportService : BaseService, IReportService
    {
        /// <summary>
        /// 通过Id查询惠粉举报
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-8-18</remarks>
        public ResultModel GetReportById(long id)
        {
            var result = new ResultModel { Data = _database.Db.Report.FindByReportId(id) };
            return result;
        }

        /// <summary>
        /// 惠粉举报分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-8-18</remarks>
        public ResultModel Select(SearchReportModel model)
        {
            var report = _database.Db.Report;
            var user = _database.Db.YH_User;
            var reportTyplang = _database.Db.ReportType_lang;

            //查询参数条件
            var whereParam = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            //举报人
            if (!string.IsNullOrEmpty(model.Account))
            {
                whereParam = new SimpleExpression(whereParam, user.As("user").Account.Like("%" + model.Account + "%"), SimpleExpressionType.And);
            }
            //举报类型Id
            if (!string.IsNullOrEmpty(model.ReportTypeName))
            {
                whereParam = new SimpleExpression(whereParam, reportTyplang.ReportTypeName.Like("%" + model.ReportTypeName + "%"), SimpleExpressionType.And);
            }
            
            //多语言
            if (model.LanguageID != 0)
            {
                whereParam = new SimpleExpression(whereParam, reportTyplang.LanguageID == model.LanguageID, SimpleExpressionType.And);
            }
            dynamic u;
            dynamic reportedUser;
            dynamic rl;
            var query = report.All().
                LeftJoin(reportTyplang, out rl).On(rl.ReportTypeId == report.ReportTypeId).
                LeftJoin(user.As("user"), out u).On(u.UserID == report.ReportUserId).
                LeftJoin(user.As("reportedUser"), out reportedUser).On(reportedUser.UserID == report.BeReporteId).
                Select(
                report.ReportId,
                report.ReportUserId,
                report.BeReporteId,
                report.ReportContent,
                report.CreateDT,
                report.Status,
                report.Result,
                report.DealUser,
                report.DealDate,
                u.Account.As("Account"),
                reportedUser.Account.As("ReportedName"),
                rl.LanguageID,
                rl.ReportTypeName
                ).Where(whereParam).OrderByCreateDTDescending();
          
            var result = new ResultModel
            {
                Data =
                    new SimpleDataPagedList<ReportModel>(
                       query,
                        model.PagedIndex, model.PagedSize)
            };
            return result;
        }

        /// <summary>
        /// 通过Id删除惠粉举报
        /// </summary>
        /// <param name="id">惠粉举报id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-8-18</remarks>
        public ResultModel Delete(long id)
        {
            var result = new ResultModel { Data = _database.Db.Report.DeleteByReportId(id) };
            return result;
        }

        /// <summary>
        /// 更新惠粉举报
        /// </summary>
        /// <param name="model">惠粉举报</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-8-18</remarks>
        public ResultModel Update(ReportModel model)
        {
           
            var result = new ResultModel
            {
                Data = _database.Db.Report.UpdateByReportId(ReportId: model.ReportId,
                    Status: model.Status, Result: model.Result, DealUser: model.DealUser, DealDate: model.DealDate)
            };
            return result;
        }
    }
}
