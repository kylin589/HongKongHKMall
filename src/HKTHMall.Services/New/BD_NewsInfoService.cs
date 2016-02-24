using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.New;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//wuyf 2015-8-21
using System.Data;
using Simple.Data.RawSql;
using Simple.Data;
using HKTHMall.Domain.WebModel.Models.Login;
using HKTHMall.Core.Extensions;
using HKTHMall.Core;


namespace HKTHMall.Services.New
{
    public class BD_NewsInfoService : BaseService, IBD_NewsInfoService
    {
        /// <summary>
        /// 通过Id查询新闻信息
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-8-10</remarks>
        public ResultModel GetBD_NewsInfoById(int id)
        {
            var result = new ResultModel() { Data = base._database.Db.BD_NewsInfo.FindByID(id) };
            return result;
        }

        /// <summary>
        /// 新闻信息分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-8-10</remarks>
        public ResultModel Select(SearchBD_NewsInfoModel model)
        {
            var newsInfo = base._database.Db.BD_NewsInfo;
            //查询参数条件
            var whereParam = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            whereParam = new SimpleExpression(whereParam, newsInfo.IsDelete == 0, SimpleExpressionType.And);
            //1.惠卡动态,2惠粉公告,3惠粉消息
            if (model.TypeID != null)
            {
                whereParam = new SimpleExpression(whereParam, newsInfo.TypeID == model.TypeID.Value, SimpleExpressionType.And);
            }
            //标题
            if (!string.IsNullOrEmpty(model.Title))
            {
                whereParam = new SimpleExpression(whereParam, newsInfo.Title.Like("%" + model.Title + "%"), SimpleExpressionType.And);
            }
            //发布人
            if (!string.IsNullOrEmpty(model.Releaser))
            {
                whereParam = new SimpleExpression(whereParam, newsInfo.Releaser.Like("%" + model.Releaser + "%"), SimpleExpressionType.And);
            }
            //是否导读
            if (model.IsHasNaviContent != null)
            {
                whereParam = new SimpleExpression(whereParam, newsInfo.IsHasNaviContent == model.IsHasNaviContent.Value, SimpleExpressionType.And);
            }
            //是否已经审核(1:审核通过,2:待审核,3.拒审)
            if (model.IsCheck != null)
            {
                whereParam = new SimpleExpression(whereParam, newsInfo.IsCheck == model.IsCheck.Value, SimpleExpressionType.And);
            }
            //是否图片新闻(0:否,1:是)
            if (model.IsPic != null)
            {
                whereParam = new SimpleExpression(whereParam, newsInfo.IsPic == model.IsPic.Value, SimpleExpressionType.And);
            }
            //发布时间
            if (model.BeginDate != null)
            {
                whereParam = new SimpleExpression(whereParam, newsInfo.ReleaseDT >= model.BeginDate, SimpleExpressionType.And);
            }
            if (model.EndDate != null)
            {
                whereParam = new SimpleExpression(whereParam, newsInfo.CreateDT < Convert.ToDateTime(model.EndDate).AddDays(1), SimpleExpressionType.And);
            }
            var query = newsInfo.All().Where(whereParam).OrderByCreateDTDescending();

            var result = new ResultModel()
            {
                Data = new SimpleDataPagedList<BD_NewsInfoModel>(query,
                            model.PagedIndex, model.PagedSize)
            };
            return result;
        }

        /// <summary>
        /// 添加新闻信息
        /// </summary>
        /// <param name="model">新闻信息</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-8-10</remarks>
        public ResultModel Add(BD_NewsInfoModel model)
        {
            var result = new ResultModel { Data = _database.Db.BD_NewsInfo.Insert(model) };
            return result;
        }

        /// <summary>
        /// 通过Id删除新闻信息
        /// </summary>
        /// <param name="id">新闻信息id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-8-10</remarks>
        public ResultModel Delete(int id)
        {
            var result = new ResultModel() { Data = base._database.Db.BD_NewsInfo.UpdateByID(ID: id, IsDelete: 1) };
            return result;
        }


        /// <summary>
        /// 更新新闻信息
        /// </summary>
        /// <param name="model">新闻信息对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-8-10</remarks>
        public ResultModel Update(BD_NewsInfoModel model)
        {
            dynamic record = new SimpleRecord();
            record.ID = model.ID;
            record.Title = model.Title;
            record.TypeID = model.TypeID;
            record.NewsContent = model.NewsContent;
            record.IsPic = model.IsPic;
            record.IsHasNaviContent = model.IsHasNaviContent;
            record.PicPath = model.PicPath;
            record.NaviContent = model.NaviContent;
            record.UpdateBy = model.UpdateBy;
            record.UpdateDT = DateTime.Now;
            var result = new ResultModel() { Data = this._database.Db.BD_NewsInfo.UpdateByID(record) };
            return result;
        }

        /// <summary>
        /// 审核信息
        /// </summary>
        /// <param name="model">新闻信息对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-8-11</remarks>
        public ResultModel UpdateState(BD_NewsInfoModel model)
        {
            dynamic record = new SimpleRecord();
            record.ID = model.ID;
            record.IsCheck = model.IsCheck;
            record.UpdateBy = model.UpdateBy;
            record.UpdateDT = DateTime.Now;
            var result = new ResultModel() { Data = this._database.Db.BD_NewsInfo.UpdateByID(record) };
            return result;
        }



        /// <summary>
        /// 通过语言Id查询新闻类型
        /// </summary>
        /// <param name="languageID">语言ID</param>
        /// <returns></returns>
        /// <remarks>added by jimmmy,2015-8-27</remarks>
        public ResultModel GetBD_NewsTypelang(int languageID)
        {
            var result = new ResultModel() { Data = base._database.Db.BD_NewsTypelang.FindAll( base._database.Db.BD_NewsTypelang.LanguageID==languageID) };
            MemCacheFactory.GetCurrentMemCache().AddCache("GetBD_NewsTypelang"+languageID,result.Data,60);
            return result;
        }
    }
}
