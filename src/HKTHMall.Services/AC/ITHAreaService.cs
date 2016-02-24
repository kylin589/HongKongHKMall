using Autofac.Extras.DynamicProxy2;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.WebModel.Models.Login;

namespace HKTHMall.Services.AC
{
    public interface ITHAreaService : IDependency
    {
        /// <summary>
        /// 获取区域数据
        /// zhoub 20150709
        /// </summary>
        /// <returns></returns>
        ResultModel GetTHAreaByLanguageIdToTree(int languageId);
        /// <summary>
        /// 获取区域数据(Api)
        /// zhoub 20150709
        /// </summary>
        /// <returns></returns>
        ResultModel GetTHAreaByLanguageIdToTreeApi(int languageId);
        /// <summary>
        ///  根据区域id获取所有语言信息
        ///  zhoub 20150709
        /// </summary>
        /// <param thAreaId="id">区域id</param>
        ResultModel GetTHArea_langByTHAreaID(int thAreaId);

        /// <summary>
        /// 区域信息添加
        /// zhoub 20150709.update by liujc 增加hAreaName
        /// </summary>
        ResultModel AddTHArea(int parentId, string cAreaName, string eAreaName, string tAreaName, string hAreaName, string shortName, int areaType);

        /// <summary>
        /// 区域信息修改.update by liujc 增加hAreaName
        /// </summary>
        ResultModel EditTHArea(int areaId, string cAreaName, string eAreaName, string tAreaName, string hAreaName, string shortName, int areaType);

        /// <summary>
        /// 删除区域信息
        /// zhoub 20150709
        /// </summary>
        /// <param name="thAreaId"></param>
        /// <returns></returns>
        ResultModel DelTHArea(int thAreaId);

        /// <summary>
        /// 根据区域语言ID、父ID获取数据
        /// zhoub 20150717
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        ResultModel GetTHAreaByParentID(int languageId, int parentID);

        /// <summary>
        /// 根据区域语言ID、ID获取数据
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        ResultModel GetTHAreaByID(int languageId, int ID);

        /// <summary>
        /// 根据子区域Id,获取整个层级的区域名
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel GetSingleTierAreaNames(SearchUserAddressModel model, int languageID);
    }
}
