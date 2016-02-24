using BrCms.Framework.Logging;
using HKTHMall.Core;
using HKTHMall.Domain.AdminModel.Models.AC;
using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Domain.CashBack;
using HKTHMall.Domain.Models;
using HKTHMall.Services.AC;
using HKTHMall.Services.Sys;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple.Data.RawSql;
using BrCms.Framework.Collections;

namespace HKTHMall.Services.Users
{
    /// <summary>
    /// 返现
    /// </summary>
    public class ZJ_RebateService : BaseService, IZJ_RebateService
    {
        private static IExceptionLogService exceptionLogService;
        private static ILogger logger;
        private static IParameterSetService parameterSetService;
        private static int isFixedDays = 1;//0标示各商品分别设置返还天数，1便是全站统一返还
        private static int backDays = 1500;//返还天数 
        private static decimal backPoint = 1m;//返现比例
        public ZJ_RebateService(IExceptionLogService _exceptionLogService, ILogger _logger,IParameterSetService _parameterSetService)
        {
            exceptionLogService = _exceptionLogService;
            logger = _logger;
            parameterSetService=_parameterSetService;
            try
            {
                int.TryParse(parameterSetService.GetParametePValueById(7529218804).Data, out isFixedDays);
                int.TryParse(parameterSetService.GetParametePValueById(7529218793).Data, out backDays);
                decimal.TryParse(parameterSetService.GetParametePValueById(7529218877).Data, out backPoint);
            }
            catch (Exception ex)
            { 
            
            }
        }
        /// <summary>
        /// 获取返现记录
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        public ResultModel GetList(SearchRebate model)
        {
            var result = new ResultModel();
            try
            {
                var tb = _database.Db.ZJ_RebateInfo;
                var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
                if (model.ID > 0)
                {
                    where = new SimpleExpression(where, tb.ID == model.ID, SimpleExpressionType.And);
                }
                result.Data = tb.All().Where(where).OrderByCreateTime();
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        #region 返现操作
        /// <summary>
        /// 返现操作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void CashBackOrder()
        {           
            ExceptionLogModel exceptionLogModel = new ExceptionLogModel();
            exceptionLogModel.ServiceName = "会员订单返现服务";
            exceptionLogModel.CreateBy = "系统返现服务";
            var zr = _database.Db.ZJ_RebateInfo;
            List<ZJ_RebateInfo> zjorder = zr.FindAll(zr.Status == 1 && zr.StartTime <= DateTime.Now ).ToList<ZJ_RebateInfo>();
            zjorder = zjorder.FindAll(x => x.LastUpdateTime == null || DateTime.Now.Date.Subtract(((DateTime)x.LastUpdateTime).Date).Days > 0);
            if (zjorder != null && zjorder.Count > 0)
            {
                foreach (ZJ_RebateInfo model in zjorder)
                {
                   
                        try
                        {

                            //判断是否已返现完成
                            if (model.TotalMoney <= model.PaidMoney)
                            {
                                zr.UpdateByID(ID: model.ID, Status: 2, LastUpdateBY: "系统服务", LastUpdateTime: DateTime.Now);
                                _logger.Error(typeof(ZJ_RebateService), string.Format("订单【{0}】的已经返现完成", model.ID));

                            }
                            else
                            {

                                decimal m = 0;//本次返回金额
                                decimal pm = 0;//当前已付金额+本次返回金额
                            int pd = 0;
                                if (model.TotalDay - model.PaidDays == 1)//最后一天返回所有余额（小数点问题）
                                {
                                    m = model.TotalMoney - model.PaidMoney;
                                    
                                }
                                else 
                                {
                                    m = ToFixed((model.TotalMoney / model.TotalDay),2);
                                    if (m < 0.01m)
                                    {
                                        m = 0.01m;
                                    }
                                  
                                }
                                pm = model.PaidMoney + m;
                                pd = model.PaidDays + 1;
                                #region 用户余额
                                ZJ_UserBalanceModel ub = new ZJ_UserBalanceModel();
                                 ub = _database.Db.ZJ_UserBalance.FindByUserId(UserId: model.UserID);
                                 if (ub == null)
                                 {
                                     exceptionLogModel.Message = string.Format("未能找到用户【{0}】的余额信息", model.UserID);
                                     exceptionLogModel.ResultType = 2;
                                     exceptionLogModel.Status = 1;
                                 }
                                #endregion
                                #region 添加资金异动流水
                                ZJ_UserBalanceChangeLogModel ulogModel = new ZJ_UserBalanceChangeLogModel();                              
                                ulogModel.AddOrCutAmount = m;
                                ulogModel.AddOrCutType =17;
                                ulogModel.CreateBy = "系统返现服务";
                                ulogModel.CreateDT = DateTime.Now;
                                ulogModel.IsAddOrCut = 1;                               
                                ulogModel.IsDisplay = 1;
                                ulogModel.NewAmount = ub.ConsumeBalance+m;
                                ulogModel.OldAmount = ub.ConsumeBalance;
                                ulogModel.OrderNo = model.OrderDetailsID.ToString();                               
                                ulogModel.Remark = model.Remark;
                                ulogModel.UserID = (long)model.UserID;
                                #endregion
                                using (var bt = _database.Db.BeginTransaction())
                                {
                                    try
                                    {
                                        //用户余额修改
                                        bt.ZJ_UserBalance.UpdateByUserId(UserId: model.UserID, ConsumeBalance: ulogModel.NewAmount, UpdateBy: "系统返现服务", UpdateDT:DateTime.Now);
                                       //添加资金异动记录
                                        bt.ZJ_UserBalanceChangeLog.Insert(ulogModel);
                                        //修改返现表
                                        bt.ZJ_RebateInfo.UpdateByID(ID: model.ID, PaidMoney: pm,PaidDays:pd, LastUpdateBY: "系统服务", LastUpdateTime: DateTime.Now);
                             
                                        bt.Commit();
                                        _logger.Error(typeof(ZJ_RebateService), string.Format("处理返现订单【{0}】的返现成功", model.ID));
                                    }
                                    catch (Exception ex)
                                    {
                                        bt.Rollback();
                                        exceptionLogModel.HandleId = model.ID.ToString();
                                        exceptionLogModel.Status = 1;
                                        exceptionLogModel.ResultType = 1;
                                        exceptionLogModel.Message = string.Format("处理返现订单【{0}】的返现失败,{1},数据库事务回滚", model.ID, ex.Message);
                                        exceptionLogService.Add(exceptionLogModel);
                                        _logger.Error(typeof(ZJ_RebateService), string.Format("处理返现订单【{0}】的返现失败,{1},数据库事务回滚", model.ID, ex.Message));

                                    }
                                }
                            }

                           
                      
                        }
                        catch (Exception ex)
                        { 
                            exceptionLogModel.HandleId = model.ID.ToString();
                            exceptionLogModel.Status = 1;
                            exceptionLogModel.ResultType = 1;
                            exceptionLogModel.Message = string.Format("处理返现订单【{0}】的返现失败,{1}", model.ID, ex.Message);
                            exceptionLogService.Add(exceptionLogModel);
                            _logger.Error(typeof(ZJ_RebateService), string.Format("处理返现订单【{0}】的返现失败,{1}", model.ID, ex.Message));

                        }
                    }
                
            }
            else
            {
                exceptionLogModel.CreateDT = DateTime.Now;
                exceptionLogModel.Message = "本次没有需要处理的返现";
                exceptionLogModel.ResultType = 2;
                exceptionLogModel.Status = 1;

            }           
        }
        #endregion
        #region 生成返现订单
        /// <summary>
        /// 生成返现订单
        /// </summary>
        /// <returns></returns>
        public BackMessage GenerateList(OderModel model)
        {
            var result = new BackMessage();
            result.status = 0;
            ExceptionLogModel exceptionLogModel = new ExceptionLogModel();
            exceptionLogModel.ServiceName = "会员购物订单返现订单生成服务";
            exceptionLogModel.CreateBy = "系统服务";
            List<OrderDetails> orderDetails =  _database.Db.OrderDetails.FindAll(_database.Db.OrderDetails.OrderID == model.OrderID).ToList<OrderDetails>();
            int tuiCount = orderDetails.Count(x => x.IsReturn == 2);//是否有退款的订单明细
            //if (model.RefundFlag != 0)//存在退款
            //{
            //    orderDetails = orderDetails.FindAll(x => x.IsReturn == 3 || x.IsReturn == 0);
            //}
          //  orderDetails = orderDetails.FindAll(x => x.IsReturn == 3 || x.IsReturn == 0);
            if (orderDetails != null)
            {
                List<ZJ_RebateInfo> zj_detils = new List<ZJ_RebateInfo>();
                if (tuiCount==0)
                {
                    foreach (OrderDetails detail in orderDetails)
                    {
                        //商品返现金额= 销售价 * 数量 
                        decimal tmoney = detail.SalesPrice * detail.Quantity;
                        zj_detils.Add(FormartZJ_RebateInfo(tmoney, model, detail));
                    }
                }
                else
                {
                    List<ReturnProductInfoModel> returnList = GetReturnProductListByOrderId(model.OrderID);
                    foreach (OrderDetails detail in orderDetails)
                    {
                        // int yituiCount = returnList.Count(x => x.OrderDetailsID == detail.OrderDetailsID);
                        ReturnProductInfoModel returndetail = returnList.Where(x => x.OrderDetailsID == detail.OrderDetailsID).FirstOrDefault();
                        if (returndetail !=null)
                        {
                            int num = detail.Quantity - returndetail.ReturntNumber;
                            if(num>0)
                            {
                                decimal tmoney = detail.SalesPrice * num;                        //商品返现金额= 销售价 * 数量 
                                zj_detils.Add(FormartZJ_RebateInfo(tmoney, model, detail));
                            }     
                        }
                        else
                        {
                            //商品返现金额= 销售价 * 数量 
                            decimal tmoney = detail.SalesPrice * detail.Quantity;
                            zj_detils.Add(FormartZJ_RebateInfo(tmoney, model, detail));
                        }
                        
                    }
                }
                
                if (SetCashBackList(model, zj_detils))
                {
                    result.status = 1;
                }
            }
            else
            {
                exceptionLogModel.HandleId = model.OrderID;
                exceptionLogModel.Status = 1;
                exceptionLogModel.ResultType = 1;
                exceptionLogModel.Message = string.Format("处理订单【{0}】的返现订单生成失败,获取订单详情失败", model.OrderID);
                exceptionLogService.Add(exceptionLogModel);
                _logger.Error(typeof(ZJ_RebateService), string.Format("处理订单【{0}】的返现订单生成失败,获取订单详情失败", model.OrderID));

            }
            return result;
        }
        /// <summary>
        /// 获取已退款单明细
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<ReturnProductInfoModel> GetReturnProductListByOrderId(string orderId)
        {
            string sql = string.Format("select ProductId,OrderID,OrderDetailsID,ReturntNumber from dbo.ReturnProductInfo where OrderID='{0}' and ReturnStatus =5", orderId);
            List<ReturnProductInfoModel> returnList = new List<ReturnProductInfoModel>();
            returnList= _dataDapper.Query<ReturnProductInfoModel>(sql).ToList();
            return returnList;
        }

        /// <summary>
        /// 添加数据到表
        /// </summary>
        /// <param name="model">订单表实体</param>
        /// <param name="zj_detils">订单详情集合</param>
        public bool SetCashBackList(OderModel model,List<ZJ_RebateInfo> zj_detils)
        {
            bool flag = false;
            ExceptionLogModel exceptionLogModel = new ExceptionLogModel();
            exceptionLogModel.ServiceName = "会员购物订单返现订单生成服务";
            exceptionLogModel.CreateBy = "系统服务";
            using (var bt = _database.Db.BeginTransaction())
            {
                try
                {
                    //添加返现记录
                    foreach (ZJ_RebateInfo detail in zj_detils)
                    {
                        bt.ZJ_RebateInfo.Insert(detail);
                    }

                    //更改订单状态为已完成,表示该订单已经做了订单返现单生成
                    bt.Order.UpdateByOrderID(OrderID: model.OrderID, IsReward: 1, OrderStatus: (int)HKTHMall.Domain.Enum.OrderEnums.OrderStatus.Completed);
                    //添加订单跟踪记录
                    OrderTrackingLogModel orderTrackingLogOne = new OrderTrackingLogModel();
                    orderTrackingLogOne.OrderID = model.OrderID;
                    orderTrackingLogOne.OrderStatus = (int)HKTHMall.Domain.Enum.OrderEnums.OrderStatus.Completed;
                    orderTrackingLogOne.TrackingContent = "已完成";
                    orderTrackingLogOne.CreateTime = DateTime.Now;
                    orderTrackingLogOne.CreateBy = "会员订单返现单生成服务";
                    bt.OrderTrackingLog.Insert(orderTrackingLogOne);
                    bt.Commit();
                    flag = true;

                }
                catch (Exception ex)
                {
                    bt.Rollback();
                    exceptionLogModel.HandleId = model.OrderID;
                    exceptionLogModel.Status = 1;
                    exceptionLogModel.ResultType = 1;
                    exceptionLogModel.Message = string.Format("处理订单【{0}】的返现订单生成失败,{1},数据库事务回滚", model.OrderID, ex.Message);
                    exceptionLogService.Add(exceptionLogModel);
                    _logger.Error(typeof(ZJ_RebateService), string.Format("处理订单【{0}】的返现订单生成失败,{1},数据库事务回滚", model.OrderID, ex.Message));
                }

            }
            return flag;
        }

        /// <summary>
        /// 生成返现实体
        /// </summary>
        /// <param name="totalMoney"></param>
        /// <param name="model"></param>
        /// <param name="dmodel"></param>
        /// <returns></returns>
        private ZJ_RebateInfo FormartZJ_RebateInfo(decimal totalMoney, OderModel model, OrderDetails dmodel)
        {
            ZJ_RebateInfo zrModel = new ZJ_RebateInfo();
            zrModel.ID = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
            zrModel.CreateBY = "系统服务";
            zrModel.CreateTime = DateTime.Now;
            zrModel.OrderDetailsID = dmodel.OrderDetailsID;
            zrModel.OrderID = dmodel.OrderID;
            zrModel.ProductID = dmodel.ProductId;
            zrModel.SKUID = dmodel.SKU_ProducId;
            zrModel.StartTime = DateTime.Now;
            zrModel.Status = 1;
            if (isFixedDays == 1)
            {
                zrModel.TotalDay = backDays;
                zrModel.TotalMoney = totalMoney * backPoint;
            }
            else
            {
                zrModel.TotalDay = dmodel.RetateDays != null ? (int)dmodel.RetateDays : backDays;
                zrModel.TotalMoney = totalMoney * (decimal)(dmodel.ReateRedio != null ? dmodel.ReateRedio : backPoint);
            }
            zrModel.UserID = model.UserID;
            return zrModel;
        }
        #endregion

        /// <summary> 
        /// 将小数值按指定的小数位数截断 
        /// </summary> 
        /// <param name="d">要截断的小数</param> 
        /// <param name="s">小数位数，s大于等于0，小于等于28</param> 
        /// <returns></returns> 
        public  decimal ToFixed(decimal d, int s)
        {
            decimal sp = Convert.ToDecimal(Math.Pow(10, s));

            if (d < 0)
                return Math.Truncate(d) + Math.Ceiling((d - Math.Truncate(d)) * sp) / sp;
            else
                return Math.Truncate(d) + Math.Floor((d - Math.Truncate(d)) * sp) / sp;
        }

        /// <summary> 
        /// 将双精度浮点值按指定的小数位数截断 
        /// </summary> 
        /// <param name="d">要截断的双精度浮点数</param> 
        /// <param name="s">小数位数，s大于等于0，小于等于15</param> 
        /// <returns></returns> 
        public  double ToFixed(double d, int s)
        {
            double sp = Math.Pow(10, s);

            if (d < 0)
                return Math.Truncate(d) + Math.Ceiling((d - Math.Truncate(d)) * sp) / sp;
            else
                return Math.Truncate(d) + Math.Floor((d - Math.Truncate(d)) * sp) / sp;
        } 

        /// <summary>
        /// 获取剩余返现资金
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public ResultModel GetSurplusRebeatAmount(long UserId)
        {
            var tb = _database.Db.ZJ_RebateInfo;
            string sql = string.Format("SELECT SUM([TotalMoney]-[PaidMoney]) AS 'Rebeat' FROM [ZJ_RebateInfo] A WHERE A.UserID={0}", UserId);
            ResultModel result = new ResultModel();
            result.Data  = _database.RunSqlQuery(x => x.ToResultSets(sql))[0][0];
            result.IsValid = true;
            return result;
        }
        /// <summary>
        /// 获取返现列表
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">页面大小</param>
        /// <param name="Lang">语言</param>
        /// <returns></returns>
        public ResultModel GetRebeatAmountList(long UserID,int PageIndex,int PageSize,int Lang=4)
        {
            StringBuilder sb=new StringBuilder();
            sb.Append("SELECT * FROM (SELECT A.[TotalMoney],A.ProductID,A.[TotalDay],A.[StartTime],B.ProductName,C.PicUrl,D.SalesPrice,D.Quantity,E.SkuName ");
                sb.Append(",(SELECT  reverse(stuff(reverse((SELECT DISTINCT BBB.AttributeName+',' FROM dbo.SKU_AttributeValues AAA JOIN dbo.SKU_Attributes BBB ON AAA.AttributeId=BBB.AttributeId ");
            sb.Append("WHERE CHARINDEX(convert(varchar,AAA.ValueId)+'_',E.SKUStr)>0 OR CHARINDEX('_'+convert(varchar,AAA.ValueId),E.SKUStr)>0 OR E.SKUStr=convert(varchar,AAA.ValueId) FOR XML PATH(''))),1,1,''))) As 'AttributeName' ");
            sb.Append(",ROW_NUMBER() OVER (ORDER BY A.CreateTime DESC) AS rowNumber FROM [ZJ_RebateInfo] A ");
            sb.Append("JOIN [Product_Lang] B ON A.ProductID=B.ProductId JOIN [ProductPic] C ON A.ProductID=C.ProductID JOIN [OrderDetails] D ON A.OrderDetailsID=D.OrderDetailsID  JOIN [SKU_Product] E ON A.SKUID=E.SKU_ProducId ");
            sb.Append("WHERE A.UserID={0} AND B.LanguageID={3} AND C.Flag=1) AS tb WHERE tb.rowNumber BETWEEN {1} AND {2}") ;
            string sql=string.Format(sb.ToString(),UserID,(PageIndex-1)*PageSize+1,PageIndex*PageSize,Lang);
            ResultModel result = new ResultModel();
            result.Data = _database.RunSqlQuery(x => x.ToResultSets(sql))[0];
            result.IsValid = true;
            return result;
        }
        /// <summary>
        /// 根据用户ID获取总数
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public ResultModel GetCountByUserID(long UserID)
        {
            var result = new ResultModel()
            {
                Data = base._database.Db.ZJ_RebateInfo.GetCount(base._database.Db.ZJ_RebateInfo.UserID==UserID)
            };
            return result;
        }
        /// <summary>
        /// 获取已返金额
        /// </summary>
        /// <param name="UserID">获取已返金额</param>
        /// <returns></returns>
        public ResultModel GetPaidAmount(long UserID)
        {
            var result = new ResultModel()
            {
                Data = base._database.Db.ZJ_RebateInfo.All().Where(base._database.Db.ZJ_RebateInfo.UserID == UserID).Select(base._database.Db.ZJ_RebateInfo.PaidMoney.Sum()).ToScalarOrDefault()
            };
            return result;
        }

        /// <summary>
        /// 获取总金额
        /// </summary>
        /// <param name="UserID">获取已返金额</param>
        /// <returns></returns>
        public ResultModel GetTotalAmount(long UserID)
        {
            var result = new ResultModel()
            {
                Data = base._database.Db.ZJ_RebateInfo.All().Where(base._database.Db.ZJ_RebateInfo.UserID == UserID).Select(base._database.Db.ZJ_RebateInfo.TotalMoney.Sum()).ToScalarOrDefault()
            };
            return result;
        }

        /// <summary>
        /// 获取每日应返还金额
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public ResultModel GetRebateEveryDay(long UserID)
        {
            var tb = _database.Db.ZJ_RebateInfo;
            string sql = string.Format("SELECT SUM([TotalMoney]/[TotalDay]) AS 'Rebate' FROM [ZJ_RebateInfo] A WHERE A.UserID={0} AND A.Status=1 AND DATEADD(day,A.TotalDay,A.StartTime)>='{1}'", UserID,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            ResultModel result = new ResultModel();
            result.Data = _database.RunSqlQuery(x => x.ToResultSets(sql))[0][0];
            result.IsValid = true;
            return result;
        }
    }
}