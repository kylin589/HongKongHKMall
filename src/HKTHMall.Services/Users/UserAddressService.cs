using BrCms.Framework.Collections;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Login;
using HKTHMall.Services.Common.MultiLangKeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Users
{
    public class UserAddressService : BaseService, IUserAddressService
    {
        /// <summary>
        /// 根据ID获取收货地址信息
        /// zhoub 20150717
        /// </summary>
        /// <param name="userAddressId"></param>
        /// <returns></returns>
        public ResultModel GetUserAddressById(long userAddressId)
        {
            dynamic one, two,three;
            var result = new ResultModel()
            { 
                Data = _database.Db.UserAddress.All()
                    .LeftJoin(_database.Db.THArea.As("t1"), out one).On(one.THAreaID == _database.Db.UserAddress.THAreaID)
                    .LeftJoin(_database.Db.THArea.As("t2"), out two).On(two.THAreaID == one.ParentID)
                     .LeftJoin(_database.Db.THArea.As("t3"), out three).On(three.THAreaID == two.ParentID)
                    .Select(_database.Db.UserAddress.UserAddressId, _database.Db.UserAddress.UserID, _database.Db.UserAddress.Receiver, _database.Db.UserAddress.DetailsAddress, _database.Db.UserAddress.PostalCode, _database.Db.UserAddress.Mobile, _database.Db.UserAddress.Phone, _database.Db.UserAddress.Flag, _database.Db.UserAddress.Email, one.ParentID.As("ShiTHAreaID"), two.ParentID.As("ShengTHAreaID"), three.ParentID.As("CountryTHAreaID"), _database.Db.UserAddress.THAreaID)
                    .Where(_database.Db.UserAddress.UserAddressId == userAddressId).ToList<UserAddress>()
            };
            return result;
        }

        /// <summary>
        /// 添加收货地址
        /// zhoub 20150716
        /// </summary>
        /// <param name="model">模型</param>
        public ResultModel AddUserAddress(UserAddress model)
        {
            ResultModel result = new ResultModel();
            result.Data = null;
            var userAddressCount = _database.Db.UserAddress.GetCount(_database.Db.UserAddress.UserID == model.UserID);
            if (userAddressCount < 20)
            {
                var isExits = _database.Db.UserAddress.GetCount(_database.Db.UserAddress.UserID == model.UserID && _database.Db.UserAddress.THAreaID == model.THAreaID && _database.Db.UserAddress.DetailsAddress == model.DetailsAddress && _database.Db.UserAddress.Receiver == model.Receiver && _database.Db.UserAddress.Mobile == model.Mobile && _database.Db.UserAddress.PostalCode == model.PostalCode);
                if (isExits == 0)
                {
                    result.Data = _database.Db.UserAddress.Insert(model);
                    result.Messages = new List<string> { CultureHelper.GetLangString("ACCOUNT_USERINFO_ADDRESS_SavaAddressSuccess") + "." };
                }
                else
                {
                    result.IsValid = false;
                    result.Data = 100;//已存在
                    result.Messages = new List<string> {CultureHelper.GetLangString("ACCOUNT_USERINFO_ADDRESS_ALREADYEXISTS")+"." };
                }
            }
            else
            {
                result.IsValid = false;
                result.Data = 101;//超过数量
                result.Messages = new List<string> { CultureHelper.GetLangString("ACCOUNT_USERINFO_ADDRESS_SavaAddressERROR") + "." };
            }
            return result;
        }

        /// <summary>
        /// 更新收货地址
        /// zhoub 20150716
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel UpdateUserAddress(UserAddress model)
        {
            ResultModel result = new ResultModel();
            var isExits = _database.Db.UserAddress.GetCount(_database.Db.UserAddress.UserID == model.UserID && _database.Db.UserAddress.THAreaID == model.THAreaID && _database.Db.UserAddress.DetailsAddress == model.DetailsAddress && _database.Db.UserAddress.UserAddressId != model.UserAddressId && _database.Db.UserAddress.Receiver == model.Receiver && _database.Db.UserAddress.Mobile == model.Mobile && _database.Db.UserAddress.PostalCode == model.PostalCode);
            if (isExits == 0)
            {
                result.Data = _database.Db.UserAddress.UpdateByUserAddressId(UserAddressId: model.UserAddressId, THAreaID: model.THAreaID, Receiver: model.Receiver, DetailsAddress: model.DetailsAddress, Mobile: model.Mobile, Phone: model.Phone, Email: model.Email, PostalCode: model.PostalCode);
                if (result.Data > 0)
                {
                    result.Messages = new List<string> { CultureHelper.GetLangString("ACCOUNT_USERINFO_ADDRESS_UPDATEADDRESSSUCCESS") + "." };
                }
                else
                {
                    result.IsValid = false;                    
                    result.Messages = new List<string> { CultureHelper.GetLangString("ACCOUNT_USERINFO_ADDRESS_UPDATEADDRESSFailure") + "." };
                }
            }
            else
            {
                result.IsValid = false;
                result.Data = 100;
                result.Messages = new List<string> { CultureHelper.GetLangString("ACCOUNT_USERINFO_ADDRESS_ALREADYEXISTS") + "." };
            }
            return result;
        }

        /// <summary>
        /// 常用收货地址修改
        /// zhoub 20150720
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel UpdateUserAddressFlag(UserAddress model)
        {
            ResultModel result = new ResultModel();
            _database.Db.UserAddress.UpdateByUserID(UserID: model.UserID, Flag: 0);
            result.Data = _database.Db.UserAddress.UpdateByUserAddressId(UserAddressId: model.UserAddressId, Flag: 1);
            if (result.Data > 0)
            {
                result.Messages = new List<string> { CultureHelper.GetLangString("ACCOUNT_USERINFO_ADDRESS_SaveDefaultAddressSuccess") + "." };
            }
            else
            {
                result.IsValid = false;
                result.Messages = new List<string> { CultureHelper.GetLangString("ACCOUNT_USERINFO_ADDRESS_SavaDefaultADDRESSFAILURE") + "." };
            }
            return result;
        }

        /// <summary>
        /// 删除收货地址
        /// zhoub 20150716
        /// </summary>
        /// <param name="userAddressId"></param>
        /// <returns></returns>
        public ResultModel DelUserAddress(long userAddressId)
        {
            var result = new ResultModel();
            result.Data = _database.Db.UserAddress.DeleteByUserAddressId(userAddressId);
            return result;
        }


        /// <summary>
        /// 分页获取收获地址列表
        /// zhoub 20150716
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel GetPagingUserAddress(SearchUserAddressModel model, int languageID)
        {
            dynamic one, two, three, langOne, langTwo, langThree, langFour;
            var user = _database.Db.UserAddress;

            var result = new ResultModel
            {
                Data = new SimpleDataPagedList<UserAddress>(user.All()
                    .LeftJoin(_database.Db.THArea.As("t1"), out one).On(one.THAreaID == _database.Db.UserAddress.THAreaID)
                    .LeftJoin(_database.Db.THArea.As("t2"), out two).On(two.THAreaID == one.ParentID)
                     .LeftJoin(_database.Db.THArea.As("t3"), out three).On(three.THAreaID == two.ParentID)
                    .LeftJoin(_database.Db.THArea_lang.As("l1"), out langOne).On(langOne.THAreaID == one.ParentID && langOne.LanguageID == languageID)
                    .LeftJoin(_database.Db.THArea_lang.As("l2"), out langTwo).On(langTwo.THAreaID == two.ParentID && langTwo.LanguageID == languageID)
                    .LeftJoin(_database.Db.THArea_lang.As("l3"), out langThree).On(langThree.THAreaID == user.THAreaID && langThree.LanguageID == languageID)
                     .LeftJoin(_database.Db.THArea_lang.As("l4"), out langFour).On(langFour.THAreaID == three.ParentID && langFour.LanguageID == languageID)
                    .Select(_database.Db.UserAddress.UserAddressId, _database.Db.UserAddress.UserID, _database.Db.UserAddress.Receiver, _database.Db.UserAddress.DetailsAddress, _database.Db.UserAddress.PostalCode, _database.Db.UserAddress.Mobile, _database.Db.UserAddress.Phone, _database.Db.UserAddress.Flag, _database.Db.UserAddress.Email, langOne.AreaName.As("ShiAreaName"), langTwo.AreaName.As("ShengAreaName"), _database.Db.UserAddress.THAreaID, langThree.AreaName.As("QuAreaName"), langFour.AreaName.As("CountryTHAreaName"))
                    .Where(user.UserID == model.UserID).OrderByDescending(user.Flag).OrderBy(user.UserAddressId), model.PagedIndex, model.PagedSize)
            };
            return result;
        }

        /// <summary>
        /// 获取用户所有收货地址
        /// </summary>
        /// <param name="model">查询模型</param>
        /// <param name="languageID">语言代码</param>
        /// <returns>地址列表</returns>
        public ResultModel GetUserAllAddress(SearchUserAddressModel model, int languageID)
        {
            dynamic one, two,three, langOne, langTwo, langThree,langFour;
            var user = _database.Db.UserAddress;

            var result = new ResultModel
            {
                Data = user.All()
                    .LeftJoin(_database.Db.THArea.As("t1"), out one).On(one.THAreaID == _database.Db.UserAddress.THAreaID)
                    .LeftJoin(_database.Db.THArea.As("t2"), out two).On(two.THAreaID == one.ParentID)
                    .LeftJoin(_database.Db.THArea.As("t3"), out three).On(three.THAreaID == two.ParentID)
                    .LeftJoin(_database.Db.THArea_lang.As("l1"), out langOne).On(langOne.THAreaID == one.ParentID && langOne.LanguageID == languageID)
                    .LeftJoin(_database.Db.THArea_lang.As("l2"), out langTwo).On(langTwo.THAreaID == two.ParentID && langTwo.LanguageID == languageID)
                    .LeftJoin(_database.Db.THArea_lang.As("l3"), out langThree).On(langThree.THAreaID == user.THAreaID && langThree.LanguageID == languageID)
                    .LeftJoin(_database.Db.THArea_lang.As("l4"), out langFour).On(langFour.THAreaID == user.THAreaID && langFour.LanguageID == languageID)
                    .Select(_database.Db.UserAddress.UserAddressId, _database.Db.UserAddress.UserID, _database.Db.UserAddress.Receiver, _database.Db.UserAddress.DetailsAddress, _database.Db.UserAddress.PostalCode, _database.Db.UserAddress.Mobile, _database.Db.UserAddress.Phone, _database.Db.UserAddress.Flag, _database.Db.UserAddress.Email, langOne.AreaName.As("ShiAreaName"), langTwo.AreaName.As("ShengAreaName"), _database.Db.UserAddress.THAreaID, langThree.AreaName.As("QuAreaName"), langFour.AreaName)
                    .Where(user.UserID == model.UserID).OrderByDescending(user.Flag).OrderBy(user.UserAddressId).ToList<UserAddress>(),
                IsValid = true
            };
            return result;
        }

        /// <summary>
        /// 根据区域ID获取区域详细信息
        /// zhoub 20150805
        /// </summary>
        /// <param name="thAreaID">区域ID</param>
        /// <param name="languageID">语言ID</param>
        /// <returns></returns>
        public ResultModel GetTHAreaAreaName(long thAreaID, int languageID)
        {
            ResultModel model = new ResultModel();
            dynamic two, langOne, langTwo, langThree;
            var area = _database.Db.THArea;
            string resultStr = "";
            var result = area.All()
                       .LeftJoin(_database.Db.THArea.As("t2"), out two).On(two.THAreaID == area.ParentID)
                       .LeftJoin(_database.Db.THArea_lang.As("l1"), out langOne).On(langOne.THAreaID == area.ParentID && langOne.LanguageID == languageID)
                       .LeftJoin(_database.Db.THArea_lang.As("l2"), out langTwo).On(langTwo.THAreaID == two.ParentID && langTwo.LanguageID == languageID)
                       .LeftJoin(_database.Db.THArea_lang.As("l3"), out langThree).On(langThree.THAreaID == area.THAreaID && langThree.LanguageID == languageID)
                       .Select(langOne.AreaName.As("ShiAreaName"), langTwo.AreaName.As("ShengAreaName"), langThree.AreaName.As("QuAreaName"))
                       .Where(area.THAreaID == thAreaID).ToList<UserAddress>();

            if (result != null)
            {
                foreach (UserAddress user in result)
                {
                    resultStr += user.ShengAreaName + user.ShiAreaName + user.QuAreaName;
                }
            }
            model.Data = resultStr;

            return model;
        }
    }
}
