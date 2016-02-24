using BrCms.Framework.Collections;
using HKTHMall.Core;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Users
{
    public class MessageService : BaseService, IMessageService
    {
        /// <summary>
        /// 留言信息分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-27</remarks>
        public ResultModel Select(SearchMessageModel model)
        {
            var message = _database.Db.Message;
            #region 查询参数条件
            //查询参数条件
            var whereParam = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            //留言人
            if (!string.IsNullOrEmpty(model.MsgPerson))
            {
                whereParam = new SimpleExpression(whereParam, message.MsgPerson.Like("%" + model.MsgPerson + "%"), SimpleExpressionType.And);
            }
            //留言主题
            if (!string.IsNullOrEmpty(model.subject))
            {
                whereParam = new SimpleExpression(whereParam, message.subject.Like("%" + model.subject + "%"), SimpleExpressionType.And);
            }
            #endregion

            var query = message.All().Where(whereParam).OrderByCreateDT();


            var result = new ResultModel()
            {
                Data = new SimpleDataPagedList<MessageModel>(query,
                            model.PagedIndex, model.PagedSize)
            };
            return result;
        }

        /// <summary>
        /// 添加用户留言
        /// zhoub 20150825
        /// </summary>
        public ResultModel AddMessage(MessageModel model)
        {
            ResultModel result = new ResultModel();
            result.Data = _database.Db.Message.Insert(model);
            if (result.Data == null)
            {
                result.IsValid = false;
                result.Messages = new List<string>() { "Message failed" };//留言失败
            }
            else {
                result.Messages = new List<string>() { "Message success" };//留言成功
            }
            return result;
        }
    }
}
