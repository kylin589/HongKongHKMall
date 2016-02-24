using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.New;
using HKTHMall.Domain.Models;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.New
{
    public class NewInfoService : BaseService, INewInfoService
    {
        /// <summary>
        /// 获取新闻信息表 wuyf
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>新闻信息表</returns>
        public ResultModel GetNewInfoList(SearchNewInfoModel model)
        {
            var tb = _database.Db.NewInfo;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal); //

            if ( model.NewInfoId > 0)
            {
                //ID
                where = new SimpleExpression(where, tb.NewInfoId == model.NewInfoId, SimpleExpressionType.And);
            }
            if ( model.IsRecommend != 10)
            {
                //IsRecommend 是否推荐
                where = new SimpleExpression(where, tb.IsRecommend == model.IsRecommend, SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.NewTitle))
            {
                //新闻标题model.NewTitle.Trim() 这里不用Trim是为了英文
                where = new SimpleExpression(where, tb.NewTitle.Like("%" + model.NewTitle + "%"), SimpleExpressionType.And);
            }
            if (model.NewType > 0)
            {
                //新闻类型
                where = new SimpleExpression(where, tb.NewType == model.NewType, SimpleExpressionType.And);
            }


            //dynamic pc;

            var query = tb
                .Query()

                //.LeftJoin(_database.Db.YH_User, out pc)
                //.On(_database.Db.YH_User.UserID == tb.UserID)
                .Select(
                    tb.NewInfoId,
                    tb.NewType,
                    tb.NewTitle,
                    tb.NewContent,
                    tb.IsRecommend,
                    tb.NewImage,
                    tb.CreateBy,
                    tb.CreateDT,
                    tb.UpdateBy,
                    tb.UpdateDT

                    //pc.Phone,
                //pc.RealName,
                //pc.Account
                )
                .Where(where)
                .OrderByCreateDTDescending();

            var result = new ResultModel
            {
                Data = new SimpleDataPagedList<NewInfoModel>(query,
                        model.PagedIndex, model.PagedSize)
            };

            return result;
        }
        /// <summary>
        /// 獲取首頁新聞公告
        /// 黃主霞 2016-01-14
        /// </summary>
        /// <param name="TopCount"></param>
        /// <param name="PageNo">从1开始</param>
        /// <param name="NewsType">null表示获取所有,0表示公告1表示特惠</param>
        /// <param name="IsRecommend">是否推薦(null表示所有)</param>
        /// <returns></returns>
        public ResultModel GetIndexNews(int TopCount,int PageNo,int? NewsType, bool? IsRecommend)
        {
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            PageNo = PageNo <= 0 ? 0 : PageNo - 1;
            if(IsRecommend.HasValue)
            {
                where = new SimpleExpression(where, _database.Db.NewInfo.IsRecommend == (IsRecommend.Value?1:0), SimpleExpressionType.And);
            }
            if (NewsType.HasValue)
            {
                where = new SimpleExpression(where, _database.Db.NewInfo.NewType == NewsType.Value, SimpleExpressionType.And);
            } 
            var list =_database.Db.NewInfo.FindAll(where).OrderByDescending(_database.Db.NewInfo.CreateDT);
            var result = new  ResultModel
            {
                Data = new SimpleDataPagedList<dynamic>(list,PageNo, TopCount)
            };

            return result;
        }
        /// <summary>
        /// 获取新闻公告总条数
        /// 黄主霞 2016-01-15
        /// </summary>
        /// <param name="NewsType">null表示获取所有,0表示公告1表示特惠</param>
        /// <param name="IsRecommend">是否推薦(null表示所有)</param>
        /// <returns></returns>
        public int GetNewsCount(int? NewsType, bool? IsRecommend)
        {
            var tb=base._database.Db.NewInfo;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            if (IsRecommend.HasValue)
            {
                where = new SimpleExpression(where, _database.Db.NewInfo.IsRecommend == (IsRecommend.Value ? 1 : 0), SimpleExpressionType.And);
            }
            if (NewsType.HasValue)
            {
                where = new SimpleExpression(where, _database.Db.NewInfo.NewType == NewsType.Value, SimpleExpressionType.And);
            }
            return tb.GetCount(where);
        }

        /// <summary>
        /// 添加新闻信息表 wuyf
        /// </summary>
        /// <param name="model">新闻信息表</param>
        /// <returns>是否成功</returns>
        public ResultModel AddNewInfo(NewInfoModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.NewInfo.Insert(model)
            };
            return result;
        }

        /// <summary>
        /// 更新新闻信息表(更新推荐) wuyf
        /// </summary>
        /// <param name="model">新闻信息表模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateNewInfo(NewInfoModel model)
        {
            var result = new ResultModel();
            using (var tx = _database.Db.BeginTransaction())
            {
                try
                {
                    //tx.NewInfo.UpdateAll(IsRecommend: 0, Condition: tx.NewInfo.IsRecommend > 0);//先把所以修改成为未推荐
                    tx.NewInfo.UpdateByNewInfoId(NewInfoId: model.NewInfoId, IsRecommend: model.IsRecommend);//在把单个改成推荐
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();

                    result.IsValid = false;
                    result.Messages.Add(ex.Message);
                }
            }
            return result;
        }

        /// <summary>
        /// 更新新闻信息表 wuyf
        /// </summary>
        /// <param name="model">新闻信息表模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateNewInfos(NewInfoModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.NewInfo.UpdateByNewInfoId(NewInfoId: model.NewInfoId, NewContent: model.NewContent, NewImage: model.NewImage, NewTitle: model.NewTitle, NewType: model.NewType, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT)
            };
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model">新闻信息表模型</param>
        /// <returns>是否删除成功</returns>
        public ResultModel DeleteNewInfo(NewInfoModel model)
        {

            var result = new ResultModel()
            {
                Data = base._database.Db.NewInfo.Delete(NewInfoId: model.NewInfoId)
            };

            result.IsValid = result.Data > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// 根据ID获取新闻详细内容
        /// 黄主霞：2016-01-15
        /// </summary>
        /// <param name="id">新闻ID</param>
        /// <returns></returns>
        public ResultModel GetNewsById(long id)
        {
            var result = new ResultModel()
            {
                Data = base._database.Db.NewInfo.Find(base._database.Db.NewInfo.NewInfoId==id)
            };
            return result;
        }

    }
}
