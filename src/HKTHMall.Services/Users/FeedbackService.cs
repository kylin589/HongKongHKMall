﻿using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Users;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Users
{
    public class FeedbackService : BaseService, IFeedbackService
    {
        /// <summary>
        /// 用户反馈分页查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-8-12</remarks>
        public ResultModel Select(SearchFeedbackModel model)
        {
            var feedback = _database.Db.Feedback;
            var feedbackType_lang = _database.Db.FeedbackType_lang;
            var user = _database.Db.YH_User;

            //查询参数条件
            var whereParam = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            //用户名
            if (!string.IsNullOrEmpty(model.Account))
            {
                whereParam = new SimpleExpression(whereParam, user.Account.Like("%" + model.Account + "%"), SimpleExpressionType.And);
            }
            //反馈名称
            if (!string.IsNullOrEmpty(model.FeedbackName))
            {
                whereParam = new SimpleExpression(whereParam, feedbackType_lang.FeedbackName.Like("%" + model.FeedbackName + "%"), SimpleExpressionType.And);
            }
            //多语言
            if (model.LanguageID != 0)
            {
                whereParam = new SimpleExpression(whereParam, feedbackType_lang.LanguageID == model.LanguageID, SimpleExpressionType.And);
            }
            //来源
            if (model.Source != null)
            {
                whereParam = new SimpleExpression(whereParam, feedback.Source == model.Source.Value, SimpleExpressionType.And);
            }
            dynamic u;
            dynamic fl;
            var query = feedback.All().
                LeftJoin(feedbackType_lang, out fl).On(feedback.FeedbackType == fl.FeedbackTypeId).
                LeftJoin(user, out u).On(feedback.UserID == u.UserID).
                Select(
                feedback.FeedbackId,
                feedback.FeedbackType,
                feedback.MsgContent,
                feedback.FeedbackDate,
                feedback.Source,
                fl.FeedbackName,
                fl.LanguageID,
                u.Account
                ).
                Where(whereParam).OrderByFeedbackDateDescending();


            var result = new ResultModel()
            {
                Data = new SimpleDataPagedList<FeedbackModel>(query,
                            model.PagedIndex, model.PagedSize)
            };
            return result;
        }

        public Domain.Models.ResultModel AddFeedback(FeedbackView model)
        {
            var result = new ResultModel() { Data = base._database.Db.Feedback.Insert(model) };
            return result;
        }

        public ResultModel SelectFeedbackType(int langId)
        {
            var userBrank = _database.Db.FeedbackType_lang;
            var result = new ResultModel();
            result.Data = userBrank.FindAll(userBrank.LanguageID == langId ).ToList<FeedbackTypeLangView>();
            return result;
        }
    }
}

