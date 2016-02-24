using HKSJ.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Services.Sys;
using HKTHMall.Domain.Models.Sys;
using HKTHMall.Services.Common.MultiLangKeys;

namespace HKTHMall.Services.Common
{
    public class SMSCommon
    {
        /// <summary>
        /// 前端发送短信
        /// </summary>
        /// <param name="SendType">发送类型（1:验证码,2:任意内容）</param>
        /// <param name="MobileNumber">手机号码</param>
        /// <param name="VerificationCode">验证码</param>
        /// <param name="SMSContent">短信内容（可为空）</param>
        /// <param name="BusinessType">业务类型(1、注册 2、找回密码 3、设置交易密码 4、修改交易密码,5、修改登陆密码)</param>
        public static ResultHelper QTSubmitSMS(int SendType, string MobileNumber, string VerificationCode, string SMSContent, int BusinessType)
        {

            ParameterSetService parameterSet = new ParameterSetService();
            string sysId = parameterSet.GetParametePValueById(1215895928).Data;//系统ID
            string Account = parameterSet.GetParametePValueById(1215895926).Data;//短信平台账号
            string Password = parameterSet.GetParametePValueById(1215895927).Data;//短信平台密码
            int InvokerID = int.Parse(sysId);
            return SubmitSMS(SendType, MobileNumber, VerificationCode, SMSContent, Account, Password, InvokerID, BusinessType);
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="SendType">发送类型（1:验证码,2:任意内容）</param>
        /// <param name="MobileNumber">手机号码</param>
        /// <param name="VerificationCode">验证码</param>
        /// <param name="SMSContent">短信内容（可为空）</param>
        /// <param name="Account">系统账号</param>
        /// <param name="Password">系统密码</param>
        /// <param name="InvokerID">系统ID</param>
        /// <param name="BusinessType">业务类型</param>
        /// <returns></returns>
        public static ResultHelper SubmitSMS(int SendType, string MobileNumber, string VerificationCode, string SMSContent, string Account, string Password, int InvokerID, int BusinessType)
        {
            ResultHelper result = new ResultHelper() { Code = 0, Message = "" };

            ParameterSetService parameterSet = new ParameterSetService();
            string kaiguan = parameterSet.GetParametePValueById(1215895923).Data;//短信开关
            string strAccount = parameterSet.GetParametePValueById(1215895925).Data;//获取短信接口地址

            //判断短信开关是否开启,开启才能发送短信
            if (int.Parse(kaiguan) == 1)
            {
                SMSContent = SendType == 1 ? VerificationCode : SMSContent;

                SubmitSMS sms = new SubmitSMS()
                {
                    PhoneNumber = MobileNumber,
                    SendType = SendType,
                    SMSContent = SMSContent,
                    Account = Account,
                    Password = Password,
                    BusinessType = BusinessType,
                    InvokerID = InvokerID
                };
                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(sms);

                string returnString = HKSJ.Common.PostHelper.PostData(strAccount, jsonString, "application/json");

                return Newtonsoft.Json.JsonConvert.DeserializeObject<ResultHelper>(returnString);
            }
            else
            {
                result.Code = 0;
                result.Message = CultureHelper.GetLangString("SENDPHONEMSG_CLOSED");

                return result;
            }
        }
    }
}
