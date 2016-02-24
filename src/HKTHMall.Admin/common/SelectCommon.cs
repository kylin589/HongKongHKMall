using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HKTHMall.Admin.common
{
    public class SelectCommon
    {
        private static ZJ_AmountChangeTypeService _zjAmountChangeTypeService=new ZJ_AmountChangeTypeService();
    //    /// <summary>
    //    ///     获得角色下拉框
    //    /// </summary>
    //    /// <returns></returns>
    //    public static SelectList GetRoleSelectList()
    //    {
    //        List<SystemRoleEntity> roleList = SystemRoleBLLSub.GetRoleLis();
    //        roleList.Insert(0, new SystemRoleEntity { RoleID = 0, RoleName = "-请选择-" });
    //        var slist = new SelectList(roleList, "RoleID", "RoleName");
    //        return slist;
    //    }

        /// <summary>
        /// 获取账户异动类型
        /// </summary>
        /// <returns></returns>
        public static SelectList GetAmountChangeType()
        {
            SearchZJ_AmountChangeTypeModel model = new SearchZJ_AmountChangeTypeModel();
            model.PagedIndex = 0;
            model.PagedSize = 100;
            List<ZJ_AmountChangeTypeModel> rolelsit=_zjAmountChangeTypeService.GetZJ_AmountChangeTypeList(model).Data;
            rolelsit.Insert(0, new ZJ_AmountChangeTypeModel() { ID = 0, TypeName = "--Select--" });
            var slist = new SelectList(rolelsit, "ID", "TypeName");
            return slist;

        }

        /// <summary>
        /// 返回是否
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetIsTrueModel()
        {

            
            var rolelsit = new List<SelectListItem>() { new SelectListItem() { Value = "0", Text = "Not" }, new SelectListItem() { Value = "1", Text = "Yes" } };

            return rolelsit;

        }

        /// <summary>
        /// 返回APP平台
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetAppModel()
        {

            //List<AppModel> rolelsit = new List<AppModel>() { new AppModel { Platform = 1, AppName = "ios" }, new AppModel { Platform = 2, AppName = "Android" } };
            var rolelsit = new List<SelectListItem>() { new SelectListItem() { Value = "1", Text = "ios" }, new SelectListItem() { Value = "2", Text = "Android" } };

            return rolelsit;

        }

        /// <summary>
        /// 返回地区
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetTHArea_lang(List<HKTHMall.Domain.Models.THArea_lang> list)
        {
            var rolelsit = new List<SelectListItem>(){ new SelectListItem() { Value = "0", Text = "--Select--" }};
            if (list!=null&& list.Count>0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    HKTHMall.Domain.Models.THArea_lang model = list[i];
                    SelectListItem listItem = new SelectListItem() { Value = model.THAreaID.ToString(), Text = model.AreaName };
                    rolelsit.Add(listItem);
                }
            }
            return rolelsit;
        }


    }

    
}