using System.Runtime.InteropServices;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.AdminModel.Models.AC;
using HKTHMall.Core;

namespace HKTHMall.Services.AC.Impl
{
    /// <summary>
    /// 区域
    /// </summary>
    public class THAreaService : BaseService, ITHAreaService
    {
        /// <summary>
        /// 获取区域数据
        /// zhoub 20150709
        /// </summary>
        /// <returns></returns>
        public ResultModel GetTHAreaByLanguageIdToTree(int languageId)
        {
            dynamic cl;

            var data = _database.Db.THArea
                .Query()
                .LeftJoin(_database.Db.THArea_lang, out cl)
                .On(cl.THAreaID == _database.Db.THArea.THAreaID)
                .Select(
                    _database.Db.THArea.THAreaID.As("id")
                    , cl.AreaName.As("text")
                    , cl.languageId
                    , _database.Db.THArea.parentId
                ).OrderBy(_database.Db.THArea.THAreaID, cl.languageId)
                .Where(cl.languageId == languageId)
                .ToList();

            return new ResultModel { Data = CreateTree(data, 0) };
        }
        /// <summary>
        /// 获取区域数据
        /// zhoub 20150709
        /// </summary>
        /// <returns></returns>
        public ResultModel GetTHAreaByLanguageIdToTreeApi(int languageId)
        {
            var result = new ResultModel();
            dynamic cl;
            var key = "GetTHAreaByLanguageIdToTreeApi" + languageId;
            var dataCache = MemCacheFactory.GetCurrentMemCache().GetCache<dynamic>(key);
            if (dataCache != null && dataCache.Count > 0)
            {
                result.Data = dataCache;
            }
            else
            {
                var data = _database.Db.THArea
                .Query()
                .LeftJoin(_database.Db.THArea_lang, out cl)
                .On(cl.THAreaID == _database.Db.THArea.THAreaID)
                .Select(
                    _database.Db.THArea.THAreaID.As("areaId")
                    , cl.AreaName.As("text")
                    , cl.languageId
                    , _database.Db.THArea.parentId
                ).OrderBy(_database.Db.THArea.THAreaID, cl.languageId)
                .Where(cl.languageId == languageId)
                .ToList();
                result.Data = CreateTreeApi(data, 0);
                MemCacheFactory.GetCurrentMemCache().AddCache(key, result.Data, 60);
            }
            return result;
        }

        /// <summary>
        ///  递归创建树
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private dynamic CreateTree(List<dynamic> thArea, int parentId)
        {
            var list = thArea.FindAll(m => m.parentId == parentId);

            dynamic nodes = null;

            if (list.Any())
            {
                nodes = list.Select(m => new
                {
                    m.id,
                    m.text,
                    nodes = CreateTree(thArea, m.id)
                });
            }
            return nodes;
        }
        /// <summary>
        ///  递归创建树
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private dynamic CreateTreeApi(List<dynamic> thArea, int parentId)
        {
            var list = thArea.FindAll(m => m.parentId == parentId);

            dynamic nodes = null;

            if (list.Any())
            {
                nodes = list.Select(m => new
                {
                    m.areaId,
                    m.text,
                    nodes = CreateTreeApi(thArea, m.areaId) == null ? new List<dynamic>() : CreateTreeApi(thArea, m.areaId)
                });
            }
            return nodes;
        }
        /// <summary>
        ///  根据区域id获取所有语言信息
        ///  zhoub 20150709
        /// </summary>
        /// <param thAreaId="id">区域id</param>
        public ResultModel GetTHArea_langByTHAreaID(int thAreaId)
        {
            dynamic cl;
            var result = new ResultModel();
            result.Data = _database.Db.THArea_lang.Query()
                .Join(_database.Db.THArea, out cl).On(cl.THAreaID == _database.Db.THArea_lang.THAreaID)
                .Select(_database.Db.THArea_lang.THAreaID, _database.Db.THArea_lang.AreaName, cl.ShortName, cl.AreaType, _database.Db.THArea_lang.LanguageID)
                .Where(_database.Db.THArea_lang.THAreaID == thAreaId)
                .ToList<THArea_langModel>();
            return result;
        }

        /// <summary>
        /// 区域信息添加
        /// zhoub 20150709.update by liujc 增加hAreaName
        /// </summary>
        /// <returns></returns>
        public ResultModel AddTHArea(int parentId, string cAreaName, string eAreaName, string tAreaName, string hAreaName, string shortName, int areaType)
        {
            var result = new ResultModel();
            using (var tx = _database.Db.BeginTransaction())
            {
                try
                {
                    var thAreaModel = base._database.Db.THArea.Find(base._database.Db.THArea.ShortName == shortName);
                    if (thAreaModel == null)
                    {
                        //区域添加
                        THArea thArea = new THArea();
                        thArea.ParentID = parentId;
                        thArea.ShortName = shortName;
                        thArea.AreaType = areaType;
                        var th = _database.Db.THArea.Insert(thArea);
                        int tHAreaID = th.THAreaID;
                        //区域语言添加
                        THArea_lang cLang = new THArea_lang();
                        cLang.THAreaID = tHAreaID;
                        cLang.AreaName = cAreaName;
                        cLang.LanguageID = 1;
                        _database.Db.THArea_lang.Insert(cLang);

                        THArea_lang eLang = new THArea_lang();
                        eLang.THAreaID = tHAreaID;
                        eLang.AreaName = eAreaName;
                        eLang.LanguageID = 2;
                        _database.Db.THArea_lang.Insert(eLang);

                        //THArea_lang tLang = new THArea_lang();
                        //tLang.THAreaID = tHAreaID;
                        //tLang.AreaName = tAreaName;
                        //tLang.LanguageID = 3;
                        //_database.Db.THArea_lang.Insert(tLang);

                        //add by liujc
                        THArea_lang hLang = new THArea_lang();
                        hLang.THAreaID = tHAreaID;
                        hLang.AreaName = hAreaName;
                        hLang.LanguageID = 4;
                        _database.Db.THArea_lang.Insert(hLang);

                        tx.Commit();
                        result.Messages.Add("Add area information to success.");//添加区域信息成功.
                    }
                    else
                    {
                        result.Messages.Add("The area referred to as already exists.");//区域简称已经存在.
                    }
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    result.IsValid = false;
                    result.Messages.Add("Add area information to fail.");//添加区域信息失败.
                }
            }

            return result;
        }

        /// <summary>
        /// 区域信息修改
        /// zhoub 20150709.update by liujc 增加hAreaName
        /// </summary>
        /// <returns></returns>
        public ResultModel EditTHArea(int areaId, string cAreaName, string eAreaName, string tAreaName,string hAreaName, string shortName, int areaType)
        {
            var result = new ResultModel();
            using (var tx = _database.Db.BeginTransaction())
            {
                try
                {
                    var thAreaModel = base._database.Db.THArea.Find(base._database.Db.THArea.ShortName == shortName && base._database.Db.THArea.THAreaID != areaId);
                    if (thAreaModel == null)
                    {
                        //区域修改
                        _database.Db.THArea.UpdateByTHAreaID(THAreaID: areaId, ShortName: shortName, AreaType: areaType);
                        //区域语言添加
                        THArea_lang cLand = _database.Db.THArea_lang.Find(_database.Db.THArea_lang.THAreaID == areaId && _database.Db.THArea_lang.LanguageID == 1);
                        cLand.AreaName = cAreaName;
                        _database.Db.THArea_lang.UpdateById(cLand);

                        THArea_lang eLand = _database.Db.THArea_lang.Find(_database.Db.THArea_lang.THAreaID == areaId && _database.Db.THArea_lang.LanguageID == 2);
                        eLand.AreaName = eAreaName;
                        _database.Db.THArea_lang.UpdateById(eLand);

                        //THArea_lang tLand = _database.Db.THArea_lang.Find(_database.Db.THArea_lang.THAreaID == areaId && _database.Db.THArea_lang.LanguageID == 3);
                        //tLand.AreaName = tAreaName;
                        //_database.Db.THArea_lang.UpdateById(tLand);

                        THArea_lang hLand = _database.Db.THArea_lang.Find(_database.Db.THArea_lang.THAreaID == areaId && _database.Db.THArea_lang.LanguageID == 4);
                        hLand.AreaName = hAreaName;
                        _database.Db.THArea_lang.UpdateById(hLand);

                        tx.Commit();
                        result.Messages.Add("Modify regional information successfully.");//修改区域信息成功
                    }
                    else
                    {
                        result.IsValid = false;
                        result.Messages.Add("The area referred to as already exists.");//区域简称已经存在.
                    }
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    result.IsValid = false;
                    result.Messages.Add("Change the regional information failure.");//
                }
            }

            return result;
        }

        /// <summary>
        /// 删除区域信息
        /// zhoub 20150709
        /// </summary>
        /// <param name="thAreaId"></param>
        /// <returns></returns>
        public ResultModel DelTHArea(int thAreaId)
        {
            var result = new ResultModel();
            using (var tx = _database.Db.BeginTransaction())
            {
                try
                {
                    var thAreaModel = base._database.Db.THArea.Find(base._database.Db.THArea.ParentID == thAreaId);
                    if (thAreaModel == null)
                    {
                        _database.Db.THArea_lang.DeleteByTHAreaID(thAreaId);
                        _database.Db.THArea.DeleteByTHAreaID(thAreaId);
                        tx.Commit();
                        result.Messages.Add("Delete area information successfully.");//删除区域信息成功.
                    }
                    else
                    {
                        result.IsValid = false;
                        result.Messages.Add("Delete failed, please remove the sub area of the region.");
                    }
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    result.IsValid = false;
                    result.Messages.Add("Delete area information failure.");//删除区域信息失败
                }
            }
            return result;
        }

        /// <summary>
        /// 根据区域语言ID、父ID获取数据
        /// zhoub 20150717
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public ResultModel GetTHAreaByParentID(int languageId, int parentID)
        {
            dynamic cl;
            var result = new ResultModel();
            result.Data = _database.Db.THArea_lang.Query()
                .Join(_database.Db.THArea, out cl).On(cl.THAreaID == _database.Db.THArea_lang.THAreaID)
                .Select(_database.Db.THArea_lang.THAreaID, _database.Db.THArea_lang.AreaName)
                .Where(cl.ParentID == parentID && _database.Db.THArea_lang.LanguageID == languageId)
                .ToList<THArea_lang>();
            return result;
        }

        /// <summary>
        /// 根据区域语言ID、ID获取数据
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public ResultModel GetTHAreaByID(int languageId, int ID)
        {
            dynamic cl;
            var result = new ResultModel();
            result.Data = _database.Db.THArea.Query()
            .Join(_database.Db.THArea_lang, out cl).On(cl.THAreaID == _database.Db.THArea.THAreaID)
            .Select(_database.Db.THArea.THAreaID, cl.AreaName, _database.Db.THArea.ParentID, cl.LanguageID)
            .Where(_database.Db.THArea.THAreaID == ID && cl.LanguageID == languageId)
            .ToList<THAreaInfo>();
            return result;
        }

        /// <summary>
        /// 根据子区域Id,获取整个层级的区域名
        /// </summary>
        /// <param name="model">查询模型</param>
        /// <param name="languageID">语言Id</param>
        /// <returns></returns>
        public ResultModel GetSingleTierAreaNames(SearchUserAddressModel model, int languageID)
        {
            dynamic one, two,three, langOne, langTwo, langThree,langFour;
            var area = _database.Db.THArea;
            var result = new ResultModel();
            dynamic record = area.All()
                .LeftJoin(_database.Db.THArea.As("t1"), out one).On(one.THAreaID == area.THAreaID)
                .LeftJoin(_database.Db.THArea.As("t2"), out two).On(two.THAreaID == one.ParentID)
                 .LeftJoin(_database.Db.THArea.As("t3"), out three).On(three.THAreaID == two.ParentID)
                .LeftJoin(_database.Db.THArea_lang.As("l1"), out langOne)
                .On(langOne.THAreaID == one.ParentID && langOne.LanguageID == languageID)
                .LeftJoin(_database.Db.THArea_lang.As("l2"), out langTwo)
                .On(langTwo.THAreaID == two.ParentID && langTwo.LanguageID == languageID)
                .LeftJoin(_database.Db.THArea_lang.As("l3"), out langThree)
                .On(langThree.THAreaID == area.THAreaID && langThree.LanguageID == languageID)
                 .LeftJoin(_database.Db.THArea_lang.As("l4"), out langFour)
                .On(langFour.THAreaID == three.ParentID && langFour.LanguageID == languageID)
                .Where(one.THAreaID == model.THAreaID)
                .Select(langTwo.AreaName.As("ShengAreaName"), langOne.AreaName.As("ShiAreaName"),
                    langThree.AreaName.As("QuAreaName"), langFour.AreaName.As("CountryAreaName")).FirstOrDefault();

            Dictionary<string, string> areas = new Dictionary<string, string>();
            areas.Add("Country", record.CountryAreaName);
            areas.Add("Sheng", record.ShengAreaName);
            areas.Add("Shi", record.ShiAreaName);
            areas.Add("Qu", record.QuAreaName);
            result.Data = areas;
            result.IsValid = true;
            return result;
        }

    }
}
