using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.Keywork;
using HKTHMall.Domain.Models;
using Simple.Data;

namespace HKTHMall.Services.Keywork
{
    public class FloorKeywordService : BaseService, IFloorKeywordService
    {
        /// <summary>
        ///     通过Id查询关键字对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        public ResultModel GetFloorKeywordById(long id)
        {
            var result = new ResultModel { Data = _database.Db.FloorKeyword.FindByFloorKeywordId(id) };
            return result;
        }

        /// <summary>
        ///     关键字分页查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        public ResultModel Select(SearchFloorKeywordModel model)
        {
            var whereParam = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            if (!string.IsNullOrEmpty(model.KeyWordName))
            {
                whereParam = new SimpleExpression(whereParam, _database.Db.FloorKeyword.KeyWordName.Like("%" + model.KeyWordName + "%"), SimpleExpressionType.And);
            }
            if (model.LanguageID != 0)
            {
                whereParam = new SimpleExpression(whereParam, _database.Db.FloorKeyword.LanguageID == model.LanguageID, SimpleExpressionType.And);
            }
            var result = new ResultModel
            {
                Data =
                    new SimpleDataPagedList<FloorKeywordModel>(
                        _database.Db.FloorKeyword.FindAll(whereParam).OrderBy(_database.Db.FloorKeyword.Sorts),
                        model.PagedIndex, model.PagedSize)
            };
            return result;
        }

        /// <summary>
        ///     添加关键字
        /// </summary>
        /// <param name="model">关键字对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        public ResultModel Add(FloorKeywordModel model)
        {
            var result = new ResultModel();
            var fk = _database.Db.FloorKeyword.FindBy(KeyWordName: model.KeyWordName.Trim(), LanguageID:model.LanguageID);
            if (fk != null && fk.FloorKeywordId > 0)
            {
                result.IsValid = false;
                result.Messages.Add("Has the same name as the key word, can not repeat！");
            }
            else
            {
                try
                {
                    result.Data = _database.Db.FloorKeyword.Insert(model);
                }
                catch (System.Exception ex)
                {
                    //todo错误日志
                    throw;
                }

            }
            return result;
        }

        /// <summary>
        ///     通过Id删除关键字
        /// </summary>
        /// <param name="id">系统参数id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        public ResultModel Delete(long id)
        {
            var result = new ResultModel { Data = _database.Db.FloorKeyword.DeleteByFloorKeywordId(id) };
            return result;
        }

        /// <summary>
        ///     更新关键字
        /// </summary>
        /// <param name="model">关键字对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        public ResultModel Update(FloorKeywordModel model)
        {
            var result = new ResultModel();

            try
            {
                var fk = _database.Db.FloorKeyword.Find(_database.Db.FloorKeyword.KeyWordName == model.KeyWordName.Trim() 
                    && _database.Db.FloorKeyword.LanguageID == model.LanguageID
                    && _database.Db.FloorKeyword.FloorKeywordId !=  model.FloorKeywordId);
                if (fk != null && fk.FloorKeywordId > 0)
                {
                    result.IsValid = false;
                    result.Messages.Add("Has the same name as the key word, can not repeat the change!");//已存在相同名称的关键字,不能重复修改！
                }
                else
                {
                    var fc = this._database.Db.FloorKeyword;
                    dynamic record = new SimpleRecord();
                    record.FloorKeywordId = model.FloorKeywordId;
                    record.PlaceCode = model.PlaceCode;
                    record.KeyWordName = model.KeyWordName;
                    record.UpdateBy = model.UpdateBy;
                    record.UpdateDT = model.UpdateDT;
                    result.Data = fc.UpdateByFloorKeywordId(record);
                }
               
            }
            catch (System.Exception ex)
            {
                //todo错误日志
                throw;
            }
            return result;
        }

        /// <summary>
        /// 更新排序位置
        /// </summary>
        /// <param name="floorKeywordId">关键字管理Id</param>
        /// <param name="place">位置</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-13</remarks>
        public ResultModel UpdatePlace(long floorKeywordId, int place)
        {
            var result = new ResultModel();
            var fc = this._database.Db.FloorKeyword;
            dynamic record = new SimpleRecord();
            record.FloorKeywordId = floorKeywordId;
            record.Sorts = place;
            result.Data = fc.UpdateByFloorKeywordId(record);
            return result;
        }


        /// <summary>
        /// 首页查询
        /// </summary>
        /// <param name="language"></param>
        /// <param name="counts"></param>
        /// <returns></returns>
        public ResultModel GetTopList(int language, int counts)
        {
            var result = new ResultModel();
            result.Data = base._database.Db.FloorKeyword.ALL()
                .Take(counts)
                .Where(base._database.Db.FloorKeyword.LanguageID == language)
                .OrderBy(base._database.Db.FloorKeyword.Sorts)
                .ToList<FloorKeywordModel>();
            return result;
        }
    }
}