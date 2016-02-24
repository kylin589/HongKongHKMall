using BrCms.Framework.Collections;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.Orders;
using Simple.Data;
using System;

namespace HKTHMall.Services.Orders
{
    public class ComplaintsService : BaseService, IComplaintsService
    {
        /// <summary>
        /// 订单服务实体
        /// </summary>
        private OrderService orderService;

        public ComplaintsService(OrderService orderService)
        {
            this.orderService = orderService;
        }

        /// <summary>
        ///     通过Id查询投诉对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        public ResultModel GetComplaintsById(string id)
        {
            var result = new ResultModel { Data = _database.Db.Complaints.FindByComplaintsID(id) };
            return result;
        }

        /// <summary>
        ///     投诉对象分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        public ResultModel Select(SearchComplaintsModel model)
        {
            var complaints = _database.Db.Complaints;
            var user = _database.Db.YH_User;
            var merc = _database.Db.YH_MerchantInfo;
            var whereExpr = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            //昵称
            if (!string.IsNullOrEmpty(model.NickName))
            {
                whereExpr = new SimpleExpression(whereExpr, user.NickName.Like("%" + model.NickName + "%"), SimpleExpressionType.And);
            }
            //手机号码
            if (!string.IsNullOrEmpty(model.Phone))
            {
                whereExpr = new SimpleExpression(whereExpr, user.Phone.Like("%" + model.Phone + "%"), SimpleExpressionType.And);
            }
            //Email
            if (!string.IsNullOrEmpty(model.Email))
            {
                whereExpr = new SimpleExpression(whereExpr, user.Email.Like("%" + model.Email + "%"), SimpleExpressionType.And);
            }
            //订单Id
            if (!string.IsNullOrEmpty(model.OrderID))
            {
                whereExpr = new SimpleExpression(whereExpr, complaints.OrderID.Like("%" + model.OrderID + "%"), SimpleExpressionType.And);
            }
            dynamic m;
            dynamic u;
            var query = complaints.All().LeftJoin(user, out u).On(u.UserID == complaints.UserID).
                LeftJoin(merc, out m).On(m.MerchantID == complaints.MerchantID).
                Select(
                complaints.ComplaintsID,
                complaints.OrderID,
                m.ShopName,
                u.NickName,
                u.Phone,
                u.Email,
                complaints.complainType,
                complaints.Content,
                complaints.ComplaintsDate,
                complaints.DealPeople,
                complaints.DealDate,
                complaints.Flag,
                complaints.Comments
                ).Where(whereExpr).OrderByComplaintsDateDescending();
            var result = new ResultModel
            {
                Data =
                    new SimpleDataPagedList<ComplaintsModel>(
                       query,
                        model.PagedIndex, model.PagedSize)
            };
            return result;
        }

        /// <summary>
        ///     通过Id删除投诉
        /// </summary>
        /// <param name="id">投诉id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        public ResultModel Delete(string id)
        {
            var result = new ResultModel { Data = _database.Db.Complaints.DeleteByComplaintsID(id) };
            return result;
        }

        /// <summary>
        ///     更新投诉
        /// </summary>
        /// <param name="model">投诉对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        public ResultModel Update(ComplaintsModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.Complaints.UpdateByComplaintsID(ComplaintsID: model.ComplaintsID,
                    DealPeople: model.DealPeople, DealDate: model.DealDate, Flag: model.Flag, Comments: model.Comments)
            };
            return result;
        }

        /// <summary>
        /// 新增投诉
        /// zhoub 20150716
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel AddComplaints(ComplaintsModel model)
        {
            var result = new ResultModel();
            using (var bt = this._database.Db.BeginTransaction())
            {
                try
                {
                    bt.Complaints.Insert(model);
                    orderService.UpdateOrderComplaintStatus(model.OrderID, 1);
                    bt.Commit();
                }
                catch (Exception ex)
                {
                    bt.Rollback();
                    result.IsValid = false;
                    result.Messages.Add(ex.Message);
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// 根据用户ID查询投诉数据
        /// zhoub 20150716
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultModel GetPagingComplaints(SearchComplaintsModel model)
        {
            var com = _database.Db.Complaints;
            var merc = _database.Db.YH_MerchantInfo;
            dynamic m;
            var whereExpr = com.UserID == model.UserID;

            var result = new ResultModel
            {
                Data =
                    new SimpleDataPagedList<ComplaintsModel>(com.All().
                        LeftJoin(merc, out m).On(m.MerchantID == com.MerchantID).
                        Select(com.ComplaintsID, com.OrderID, com.Content, com.Comments, com.ComplaintsDate, com.DealDate, merc.ShopName, com.Flag).Where(whereExpr),
                        model.PagedIndex, model.PagedSize)
            };

            return result;
        }
    }
}