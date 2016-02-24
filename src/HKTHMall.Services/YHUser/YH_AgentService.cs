using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models;
using HKTHMall.Domain.AdminModel.Models.YHUser;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.YHUser
{
    public class YH_AgentService : BaseService, IYH_AgentService
    {
        /// <summary>
        /// 根据用户ID查询代理商信息
        /// zhoub 20150924
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultModel GetYH_AgentByUserId(long userId)
        {
            var ag = _database.Db.YH_Agent;
            var us = _database.Db.YH_User;
            ResultModel result = new ResultModel();
            result.Data = us.All()
                .LeftJoin(ag).On(ag.UserID == us.UserID)
                .Where(us.UserID == userId)
                .Select(ag.AgentID, ag.UserID, ag.AgentType, ag.InitialFee, ag.CreateBy, ag.CreateDT, ag.UpdateBy, ag.UpdateDT, us.RealName, us.Phone, us.Account)
                .ToList<YH_AgentModel>();
            return result;
        }

        /// <summary>
        /// 代理商添加
        /// zhoub 20150924
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel AddYH_Agent(YH_AgentModel model)
        {
            ResultModel result = new ResultModel();
            var yh_AgentModel = base._database.Db.YH_Agent.Find(base._database.Db.YH_Agent.UserID == model.UserID);
            if (yh_AgentModel == null)
            {
                result.Data = _database.Db.YH_Agent.Insert(model);
                result.Messages.Add("Agent add success.");
            }
            else
            {
                result.IsValid = false;
                result.Messages.Add("Agent already exist.");
            }
            return result;
        }

        /// <summary>
        /// 代理商更新
        /// zhoub 20150924
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel EditYH_Agent(YH_AgentModel model)
        {
            ResultModel result = new ResultModel();
            result.Data = _database.Db.YH_Agent.UpdateByAgentID(AgentID: model.AgentID, AgentType: model.AgentType, UpdateBy: model.UpdateBy, UpdateDT:model.UpdateDT);
            if (result.Data > 0)
            {
                result.Messages.Add("Agent edit success.");
            }
            else
            {
                result.IsValid = false;
                result.Messages.Add("Agent edit Failure.");
            }
            return result;
        }

        public ResultModel GetPagingYH_Agent(SearchYH_AgentModel model)
        {
            var tb = _database.Db.YH_Agent;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal); //

           
            if (model.UserID > 0)
            {
                //用户ID
                where = new SimpleExpression(where, tb.UserID == model.UserID, SimpleExpressionType.And);
            }
            if (model.AgentType > 0)
            {
                //用户ID
                where = new SimpleExpression(where, tb.AgentType == model.AgentType, SimpleExpressionType.And);
            }
            if (model.Phone != null)
            {
                where = new SimpleExpression(where, _database.Db.YH_User.Phone.Like("%" + model.Phone.Trim() + "%"), SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.Email) )
            {
                where = new SimpleExpression(where, _database.Db.YH_User.Email.Like("%" + model.Email.Trim() + "%"), SimpleExpressionType.And);
            }

            if (model.IsLock != -1)
            {
                where = new SimpleExpression(where, _database.Db.YH_User.IsLock == model.IsLock, SimpleExpressionType.And);
            }
            if (model.RegisterDateBegin != null)
            {
                where = new SimpleExpression(where, tb.CreateDT >= model.RegisterDateBegin, SimpleExpressionType.And);
            }
            if (model.RegisterDateEnd != null)
            {
                where = new SimpleExpression(where, tb.CreateDT < Convert.ToDateTime(model.RegisterDateEnd).AddDays(1), SimpleExpressionType.And);
            }

            dynamic pc;
            

            var query = tb
                .Query()
                
                .LeftJoin(_database.Db.YH_User, out pc)
                .On(_database.Db.YH_User.UserID == tb.UserID)
                .Select(
                    tb.AgentID,
                    tb.UserID,
                    tb.AgentType,
                    tb.InitialFee,
                    tb.CreateBy,
                    tb.CreateDT,
                    tb.UpdateBy,
                    tb.UpdateDT,



                    pc.Phone,
                    pc.Email,
                    pc.IsLock,
                    pc.Account
                )
                .Where(where)
                .OrderByUserIDDescending();

            var result = new ResultModel
            {
                Data = new SimpleDataPagedList<YH_AgentModel>(query,
                    model.PagedIndex, model.PagedSize)
            };

            return result;
        }

        /// <summary>
        /// 代理商删除
        /// wuyf 20150924 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel DeleterYH_Agent(YH_AgentModel model)
        {
            var result = new ResultModel();
            result.Data = _database.Db.YH_Agent.DeleteByUserID(UserID: model.UserID);
            return result;
        }
    }
}
