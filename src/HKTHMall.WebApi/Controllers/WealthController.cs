using HKTHMall.Core.Security;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Balance;
using HKTHMall.Services.Common;
using HKTHMall.Services.Common.MultiLangKeys;
using HKTHMall.Services.Users;
using HKTHMall.WebApi.Models.Request;
using HKTHMall.WebApi.Models.Result;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HKTHMall.WebApi.Controllers
{
    /// <summary>
    /// 8.财富相关接口
    /// </summary>
    public class WealthController : ApiController
    {
        /// <summary>
        /// 财富服务类
        /// </summary>
        private readonly IZJ_UserBalanceService _ZJ_UserBalanceService;
        private readonly IZJ_UserBalanceChangeLogService _ZJ_UserBalanceChangeLogService;
        private readonly IEncryptionService _EncryptionService;
        public static int ForTest = Convert.ToInt32(ConfigurationManager.AppSettings["ForTest"]);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="acDepartmentService"></param>
        public WealthController(IZJ_UserBalanceService ZJ_UserBalanceService, IZJ_UserBalanceChangeLogService ZJ_UserBalanceChangeLogService, IEncryptionService enctyptionService)
        {
            this._ZJ_UserBalanceService = ZJ_UserBalanceService;
            this._ZJ_UserBalanceChangeLogService = ZJ_UserBalanceChangeLogService;
            this._EncryptionService = enctyptionService;
        }

        #region  8.1.惠粉-我的财富（惠米）（李霞）
        /// <summary>
        /// 我的财富
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="lang">1:中文,2:英文,3:泰文</param>
        /// <returns></returns>
        [Route("Wealth/GetUserBalance")]
        [HttpPost]
        public IHttpActionResult GetUserBalance(RequestBase request)
        {
            //统一返回类型
            ApiResultModel result = new ApiResultModel();
            result.flag = 0;
            result.rs = new ResponseGetUserBalance();
            if (request == null)
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", 2); //参数不能为空
                return Ok(result);
            }
            if (string.IsNullOrEmpty(request.userId) || request.lang < 1)
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang == 0 ? 1 : request.lang); //参数错误
                return Ok(result);
            }
            request.userId = (ForTest == 1) ? ((string.IsNullOrEmpty(request.userId)) ? "0" : request.userId) : _EncryptionService.RSADecrypt(request.userId);

            long userId;
            if (long.TryParse(request.userId, out userId))
            {
                if (userId > 0)
                {
                    ResponseGetUserBalance getUserBalance = new ResponseGetUserBalance();
                    var userBalance = _ZJ_UserBalanceService.GetZJ_UserBalanceById(userId).Data;
                    if (userBalance == null)
                    {
                        getUserBalance.Balance = 0M;
                    }
                    else
                    {
                        getUserBalance.Balance = userBalance.ConsumeBalance;
                    }
                    getUserBalance.SellIncome = _ZJ_UserBalanceChangeLogService.GetSellIncome(userId).Data;
                    getUserBalance.Remark = new List<Prompt>();
                    Prompt prompt = new Prompt();
                    prompt.Content = CultureHelper.GetAPPLangSgring("USERBALANCE_CONTAIN_TOTALREVENUE", request.lang); //余额包括惠粉总收益.
                    getUserBalance.Remark.Add(prompt);
                    result.flag = 1;
                    result.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_OPERATIONSUCCESSFUL", request.lang); //操作成功
                    result.rs = getUserBalance;
                }
                else
                {
                    result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang); //参数错误
                }
            }
            else
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang); //参数错误
            }
            return Ok(result);
        }
        #endregion

        #region  8.2.账户资金明细记录列表 李霞
        /// <summary>
        /// 账户资金明细记录列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageNo">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="type">0:收支明细 1.消费记录 2.惠粉收益记录 3.退款 4.分红</param>
        /// <param name="lang">1:中文,2:英文,3:泰文</param>
        /// <returns></returns>
        [Route("Wealth/GetCapitalRecordList")]
        [HttpPost]
        public IHttpActionResult GetCapitalRecordList(RequestGetGroupList request)
        {
            //统一返回类型
            ApiPagingResultModel result = new ApiPagingResultModel();
            result.flag = 0;
            result.rs = new List<ResponseGetCapRecordList>();
            if (request == null)
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", 2); //参数不能为空
                return Ok(result);
            }
            if (string.IsNullOrEmpty(request.userId) || (request.type < 0 || request.type > 4) || request.lang < 1)
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang == 0 ? 1 : request.lang); //参数错误
                return Ok(result);
            }
            request.userId = (ForTest == 1) ? ((string.IsNullOrEmpty(request.userId)) ? "0" : request.userId) : _EncryptionService.RSADecrypt(request.userId);
            if (request.pageNo == 0)
            {
                request.pageNo = 1;
            }
            if (request.pageSize == 0)
            {
                request.pageSize = 10;
            }
            long userId;
            if (long.TryParse(request.userId, out userId))
            {
                if (userId > 0)
                {
                    var getCapitalRecordList = _ZJ_UserBalanceChangeLogService.GetCapitalRecordList(userId, request.lang, request.type);
                    if (getCapitalRecordList.IsValid)
                    {
                        List<dynamic> rsList = getCapitalRecordList.Data as List<dynamic>;

                        int tc = 0;//总页数
                        int cnt = (getCapitalRecordList.Data).Count;
                        if (cnt % request.pageSize == 0)
                        {
                            tc = cnt / request.pageSize;
                        }
                        else
                        {
                            tc = cnt / request.pageSize + 1;
                        }
                        List<ResponseGetCapRecordList> gft = new List<ResponseGetCapRecordList>();
                        if (tc >= request.pageNo)
                        {
                            //判断最后一页的个数
                            int pSize = request.pageSize * (request.pageNo - 1) + request.pageSize + 1 > cnt ? cnt - request.pageSize * (request.pageNo - 1) : request.pageSize;
                            rsList = rsList.GetRange(request.pageSize * (request.pageNo - 1), pSize);
                            foreach (var item in rsList)
                            {
                                ResponseGetCapRecordList it = new ResponseGetCapRecordList();
                                it.RecordId = item.ID;
                                it.AddOrCutAmount = item.AddOrCutAmount;
                                it.AddOrCutType = item.AddOrCutType;
                                it.IsAddOrCut = item.IsAddOrCut;
                                it.CreateDt = ConvertsTime.DateTimeToTimeStamp(item.CreateDT);
                                it.OrderNumber = item.OrderNo;
                                it.Remark = item.ZJ_Remark;
                                var getProductName = _ZJ_UserBalanceChangeLogService.GetProductName(item.OrderNo, request.lang);
                                if (getProductName.IsValid)
                                {
                                    it.ShopName = getProductName.Data;
                                }
                                else
                                {
                                    it.ShopName = "";
                                }
                                it.AddOrCutResult = 1;
                                string strAddOrCutType = string.Empty;
                                //异动类型:2:购物消费 公司虚拟账号 4:退款 6全球代理消费收益 7省级代理消费收益 8:市级代理消费收益 9:区级代理消费收益 10:感恩粉丝消费收益/感恩[一级分销商]粉丝消费收益 11:感动粉丝消费收益/感动[二级分销商]粉丝消费收益 12:感谢粉丝消费收益/感谢[三级分销商]粉丝消费收益 13省代月度毛利分红 14全球代月度毛利分红
                                switch (it.AddOrCutType)
                                {
                                    case 2://购物消费
                                        strAddOrCutType = "购物消费";
                                        it.AddOrCutType = 1;
                                        it.Remark = item.ShopName;
                                        break;
                                    case 4://退款
                                        strAddOrCutType = "退款";
                                        it.AddOrCutType = 2;
                                        break;
                                    case 6://全球代理消费收益
                                    case 7://省级代理消费收益
                                    case 8://市级代理消费收益
                                    case 9://区级代理消费收益
                                    case 10://感恩粉丝消费收益/感恩[一级分销商]粉丝消费收益
                                    case 11://感动粉丝消费收益/感动[二级分销商]粉丝消费收益
                                    case 12://感谢粉丝消费收益/感谢[三级分销商]粉丝消费收益
                                        strAddOrCutType = "惠粉消费收益";
                                        it.AddOrCutType = 3;
                                        break;
                                    case 13://省代月度毛利分红
                                    case 14://全球代月度毛利分红
                                        strAddOrCutType = "惠粉月度分红";
                                        it.AddOrCutType = 4;
                                        break;
                                    default:
                                        break;
                                }
                                gft.Add(it);
                            }
                        }
                        result.totalSize = cnt;
                        result.flag = 1;
                        result.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_OPERATIONSUCCESSFUL", request.lang); //操作成功
                        result.rs = gft;
                    }
                    else
                    {
                        result.flag = 1;
                        result.msg = CultureHelper.GetAPPLangSgring("NO_DATA", request.lang); //暂无数据
                    }
                }
                else
                {
                    result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang); //参数错误
                }
            }
            else
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang); //参数错误
            }
            return Ok(result);
        }
        #endregion

        #region  8.3.账户资金明细记录详情 李霞
        /// <summary>
        ///  账户资金明细记录详情 
        /// </summary>
        /// <param name="recordId">账号ID</param>
        /// <param name="lang">1:中文,2:英文,3:泰文</param>
        /// <returns></returns>
        [Route("Wealth/GetCapitalRecordDetails")]
        [HttpPost]
        public IHttpActionResult GetCapitalRecordDetails(RequestGetCapRecordDetails request)
        {
            //统一返回类型
            ApiResultModel result = new ApiResultModel();
            result.flag = 0;
            result.rs = new ResponseGetCapitalRecordDetails();
            if (request == null)
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", 1); //参数不能为空
                return Ok(result);
            }
            if (request.RecordId < 0 || request.Lang < 1)
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.Lang == 0 ? 1 : request.Lang); //参数错误
                return Ok(result);
            }
            var getCapitalRecordDetails = _ZJ_UserBalanceChangeLogService.GetCapitalRecordDetails(request.RecordId, request.Lang);
            if (getCapitalRecordDetails.IsValid)
            {
                List<dynamic> rsList = getCapitalRecordDetails.Data as List<dynamic>;
                ResponseGetCapitalRecordDetails details = new ResponseGetCapitalRecordDetails();
                foreach (var item in rsList)
                {
                    details.RecordId = item.ID;
                    details.OperationAmount = item.AddOrCutAmount;
                    details.AddOrCutType = item.AddOrCutType;
                    details.OperationAccount = item.ZJ_Remark;
                    var getProductName = _ZJ_UserBalanceChangeLogService.GetProductName(item.OrderNo, request.Lang);
                    if (getProductName.IsValid)
                    {
                        details.ShopName = getProductName.Data;
                    }
                    else
                    {
                        details.ShopName = "";
                    }
                    details.CreateDt = ConvertsTime.DateTimeToTimeStamp(item.CreateDT);
                    details.OrderNumber = item.OrderNo;
                    details.SerialNumber = string.IsNullOrEmpty(item.outOrderId) ? item.PaymentOrderID : item.outOrderId;
                    switch (details.AddOrCutType)
                    {
                        case 2://购物消费
                            details.IncomeWay = item.TypeName;
                            details.AddOrCutType = 1;
                            details.PaymentWay = "";
                            int payChannel = int.Parse(item.PayChannel.ToString());
                            switch (payChannel)//充值渠道（1:余额支付；2:PayPal贝宝支付,3omise）
                            {
                                case 1://MONEY_ORDER_PAYMENT_BALANCE 余额支付
                                    details.PaymentWay = CultureHelper.GetAPPLangSgring("MONEY_ORDER_PAYMENT_BALANCE", request.Lang) + "(" + details.OperationAmount.ToString("F2") + ")";
                                    break;
                                case 2://PayPal PAY 支付
                                    details.PaymentWay = "PayPal" + CultureHelper.GetAPPLangSgring("PAY", request.Lang) + "(" + details.OperationAmount.ToString("F2") + ")";
                                    break;
                                case 3://omise PAY 支付
                                    details.PaymentWay = "omise" + CultureHelper.GetAPPLangSgring("PAY", request.Lang) + "(" + details.OperationAmount.ToString("F2") + ")";
                                    break;
                                default:
                                    details.PaymentWay = "";
                                    break;
                            }
                            var getProductURL = _ZJ_UserBalanceChangeLogService.GetProductURL(item.OrderNo);
                            if (getProductURL.IsValid)
                            {
                                details.ImageUrl = getProductURL.Data;
                            }
                            else
                            {
                                details.ImageUrl = "";
                            }
                            break;
                        case 4://退款
                            details.IncomeWay = item.TypeName;
                            details.AddOrCutType = 2;
                            details.PaymentWay = item.ZJ_Remark;
                            break;
                        case 6://全球代理消费收益
                            details.IncomeWay = CultureHelper.GetAPPLangSgring("HOME_INDEX_TITEL", request.Lang); //泰国商城
                            details.ProxyType = 4;
                            details.GsType = 4;
                            details.IncomeRate = 0.06M;
                            details.AddOrCutType = 3;
                            break;
                        case 7://省级代理消费收益
                            details.IncomeWay = CultureHelper.GetAPPLangSgring("HOME_INDEX_TITEL", request.Lang); //泰国商城
                            details.ProxyType = 3;
                            details.GsType = 4;
                            details.IncomeRate = 0.06M;
                            details.AddOrCutType = 3;
                            break;
                        case 8://市级代理消费收益
                            details.IncomeWay = CultureHelper.GetAPPLangSgring("HOME_INDEX_TITEL", request.Lang); //泰国商城
                            details.ProxyType = 2;
                            details.GsType = 4;
                            details.IncomeRate = 0.02M;
                            details.AddOrCutType = 3;
                            break;
                        case 9://区级代理消费收益
                            details.IncomeWay = CultureHelper.GetAPPLangSgring("HOME_INDEX_TITEL", request.Lang); //泰国商城
                            details.ProxyType = 1;
                            details.GsType = 4;
                            details.IncomeRate = 0.02M;
                            details.AddOrCutType = 3;
                            break;
                        case 10://感恩粉丝消费收益/感恩[一级分销商]粉丝消费收益
                            details.IncomeWay = CultureHelper.GetAPPLangSgring("HOME_INDEX_TITEL", request.Lang); //泰国商城
                            details.GsType = 1;
                            details.IncomeRate = 0.3M;
                            details.AddOrCutType = 3;
                            break;
                        case 11://感动粉丝消费收益/感动[二级分销商]粉丝消费收益
                            details.IncomeWay = CultureHelper.GetAPPLangSgring("HOME_INDEX_TITEL", request.Lang); //泰国商城
                            details.GsType = 2;
                            details.IncomeRate = 0.2M;
                            details.AddOrCutType = 3;
                            break;
                        case 12://感谢粉丝消费收益/感谢[三级分销商]粉丝消费收益
                            details.IncomeWay = CultureHelper.GetAPPLangSgring("HOME_INDEX_TITEL", request.Lang); //泰国商城
                            details.GsType = 3;
                            details.IncomeRate = 0.3M;
                            details.AddOrCutType = 3;
                            break;
                        case 13://省代月度毛利分红
                        case 14://全球代月度毛利分红
                            details.IncomeWay = item.TypeName;
                            details.AddOrCutType = 4;
                            break;
                        default:
                            break; ;
                    }
                }
                result.flag = 1;
                result.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_OPERATIONSUCCESSFUL", request.Lang); //操作成功
                result.rs = details;
            }
            else
            {
                result.flag = 1;
                result.msg = CultureHelper.GetAPPLangSgring("NO_DATA", request.Lang); //暂无数据
            }
            return Ok(result);
        }
        #endregion

        #region 8.4消费收益列表
        /// <summary>
        /// 获取消费收益列表
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="GType">类型（1.感恩惠粉,2.感动惠粉,3.感谢惠粉 4.外围惠粉）</param>
        /// <param name="PageNo">分页码</param>
        /// <param name="PageSize">分页大小</param>
        /// <returns></returns>
        [Route("Wealth/GetJoinAndConsumeList")]
        [HttpPost]
        public IHttpActionResult GetJoinAndConsumeList(RequestJoinAndConsumeList req)
        {
            ApiPagingResultModel result = new ApiPagingResultModel();
            result.flag = 0;
            result.totalSize = 0;
            result.rs = new List<ResponseJoinAndConsumeList>().ToArray();
            try
            {
                //参数检测
                if (req == null)
                {
                    result.msg = CultureHelper.GetAPPLangSgring("USER_ADDFAVORITES_PARAMETERS", 1); //参数不能为空
                    return Ok(result);
                }
                //userId是否合法
                req.userId = (ForTest == 1) ? ((string.IsNullOrEmpty(req.userId)) ? "0" : req.userId) : _EncryptionService.RSADecrypt(req.userId);
                long userId = 0;
                if (!long.TryParse(req.userId, out userId))
                {
                    result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", req.lang); //参数错误
                    return Ok(result);
                }
                if (userId == 0 || req.pageNo < 1 || req.pageSize < 0)//页数小于1
                {
                    result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", req.lang); //参数错误
                    return Ok(result);
                }
                //gtype默认为1
                if (req.gtype < 1 || req.gtype > 4)
                    req.gtype = 1;
                Tuple<decimal, decimal, decimal, decimal, List<JoinAndConsumeList>> tablelist = _ZJ_UserBalanceChangeLogService.GetJoinAndConsumeList(userId, req.gtype, req.pageNo, req.pageSize).Data;
                List<JoinAndConsumeList> datalist = tablelist.Item5;
                List<ResponseJoinAndConsumeList.ExtInfo> data = new List<ResponseJoinAndConsumeList.ExtInfo>();
                foreach (JoinAndConsumeList tmp in datalist)
                {
                    ResponseJoinAndConsumeList.ExtInfo item = new ResponseJoinAndConsumeList.ExtInfo();
                    item.jcAmount = tmp.Amount.ToString();
                    item.jcPerson = string.IsNullOrEmpty(tmp.Account) ? "" : tmp.Account;
                    item.jsDT = ConvertsTime.DateTimeToTimeStamp(tmp.RegisterDate);// Convert.ToInt64((tmp.RegisterDate.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
                    item.nickName = tmp.NickName;
                    item.userId = tmp.UserID;
                    item.imageUrl = string.IsNullOrEmpty(tmp.HeadImageUrl) ? "" : ConfigurationManager.AppSettings["ImageHeader"] + tmp.HeadImageUrl;
                    data.Add(item);
                }
                ResponseJoinAndConsumeList list = new ResponseJoinAndConsumeList();
                list.gnTotal = tablelist.Item1;
                list.gdTotal = tablelist.Item2;
                list.gxTotal = tablelist.Item3;
                list.periphery = tablelist.Item4;
                list.totalAmount = tablelist.Item1 + tablelist.Item2 + tablelist.Item3 + tablelist.Item4;
                list.data = data.ToArray();
                result.totalSize = list.data.Count();
                result.flag = 1;
                result.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_OPERATIONSUCCESSFUL", req.lang); //操作成功
                result.rs = list;
                return Ok(result);
            }
            catch (Exception e)
            {
                result.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_MY_ORDERRETURNPRODUCT_FAIL", req.lang);//操作失败,请重试
                return Ok(result);
            }
        }
        #endregion

        #region 8.5.消费收益列表
        /// <summary>
        /// 惠粉消费收益明细
        /// </summary>
        /// <param name="loginId">当前登录人ID</param>
        /// <param name="userId">下级用户id</param>
        /// <param name="gtype">类型（1.感恩惠粉,2.感动惠粉,3.感谢惠粉 4.外围惠粉）</param>
        /// <param name="pageNo">分页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        [Route("Wealth/GetConsumeDetails")]
        [HttpPost]
        public IHttpActionResult GetConsumeDetails(RequestConsumeDetails req)
        {
            ApiPagingResultModel result = new ApiPagingResultModel();
            result.flag = 0;
            result.totalSize = 0;
            result.rs = new List<ResponseConsumeDetails>();
            try
            {
                //参数合法性检测
                if (req == null)
                {
                    result.msg = CultureHelper.GetAPPLangSgring("USER_ADDFAVORITES_PARAMETERS", req.lang); //参数不能为空
                    return Ok(result);
                }
                req.userId = (ForTest == 1) ? ((string.IsNullOrEmpty(req.userId)) ? "0" : req.userId) : _EncryptionService.RSADecrypt(req.userId);
                req.loginId = (ForTest == 1) ? ((string.IsNullOrEmpty(req.loginId)) ? "0" : req.loginId) : _EncryptionService.RSADecrypt(req.loginId);
                long loginId = 0;
                long userId = 0;
                if (!long.TryParse(req.loginId, out loginId) || !long.TryParse(req.userId, out userId))
                {
                    result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", req.lang); //参数错误
                    return Ok(result);
                }
                if (loginId == 0 || userId == 0 || req.pageNo < 1 || req.pageSize < 0)
                {
                    result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", req.lang); //参数错误
                    return Ok(result);
                }
                ResultModel resp = _ZJ_UserBalanceChangeLogService.GetConsumeDetails(loginId, userId, req.gtype, req.pageNo, req.pageSize);
                if (!resp.IsValid)
                {
                    result.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_MY_ORDERRETURNPRODUCT_FAIL", req.lang);
                    return Ok(result);
                }
                List<ConsumeDetails> datalist = resp.Data;
                List<ResponseConsumeDetails> items = new List<ResponseConsumeDetails>();
                foreach (ConsumeDetails data in datalist)
                {
                    ResponseConsumeDetails item = new ResponseConsumeDetails();
                    item.consumeAccount = data.Amount;
                    item.consumeDT = ConvertsTime.DateTimeToTimeStamp(data.RegisterDate);// Convert.ToInt64((data.CreateDT.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
                    items.Add(item);
                }
                result.flag = 1;
                result.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_OPERATIONSUCCESSFUL", req.lang); //操作成功
                result.totalSize = items.Count;
                result.rs = items.ToArray();
                return Ok(result);
            }
            catch (Exception e)
            {
                result.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_MY_ORDERRETURNPRODUCT_FAIL", req.lang);//操作失败,请重试
                return Ok(result);
            }
        }
        #endregion
    }
}
