using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    /// <summary>
    /// 用户基类
    /// </summary>
    public class UserInfoBaseModel
    {
        public string userId { get; set; }
        public int lang { get; set; }
    }
    /// <summary>
    /// 获取用户请求参数
    /// </summary>
    public class RequersUserInfoModel:UserInfoBaseModel
    {
    }
    /// <summary>
    /// 修改昵称请求参数
    /// </summary>
    public class RequersUpdateUserNickNameModel : UserInfoBaseModel
    {
        public string nickName { get; set; }
    }
    /// <summary>
    /// 修改头像请求参数
    /// </summary>
    public class RequersUpdateUserHeadImage : UserInfoBaseModel
    {
        public string imageUrl { get; set; }
    }
    /// <summary>
    /// 修改性别请求参数
    /// </summary>
    public class RequersUpdateUserSexModel : UserInfoBaseModel
    {
        public int sex { get; set; }
    }
    /// <summary>
    /// 修改年龄请求参数
    /// </summary>
    public class RequersUpdateUserBirthdayModel : UserInfoBaseModel
    {
        public string birthday { get; set; }
    }
    /// <summary>
    /// 修改签名请求参数
    /// </summary>
    public class RequersUpdateUserSignatureModel : UserInfoBaseModel
    {
        public string signature { get; set; }
    }
    /// <summary>
    /// 修改签名请求参数
    /// </summary>
    public class RequersUpdateResidentAddress : UserInfoBaseModel
    {
        public int tHAreaID { get; set; }
    }
}