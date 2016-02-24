using BrCms.Framework.Collections;
using HKTHMall.Core.Extensions;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Domain.WebModel.Models.Balance;
using HKTHMall.Core.Extensions;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Balance;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple.Data.RawSql;
using HKTHMall.Domain.Entities;

namespace HKTHMall.Services.Users
{
    public class ZJ_UserBalanceChangeLogService : BaseService, IZJ_UserBalanceChangeLogService
    {
        /// <summary>
        /// 获取用户账户金额异动记录表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        public ResultModel GetZJ_UserBalanceChangeLogList(SearchZJ_UserBalanceChangeLogModel model)
        {
            var tb = _database.Db.ZJ_UserBalanceChangeLog;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal); //

            if (model.ID > 0)
            {
                //流水号
                where = new SimpleExpression(where, tb.ID == model.ID, SimpleExpressionType.And);
            }
            if (model.AddOrCutType > 0)
            {
                //异动类型
                where = new SimpleExpression(where, tb.AddOrCutType == model.AddOrCutType, SimpleExpressionType.And);
            }
            if (model.IsAddOrCut != 10)
            {
                //
                where = new SimpleExpression(where, tb.IsAddOrCut == model.IsAddOrCut, SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.OrderNo) && model.OrderNo.Trim() != "")
            {
                //订单编号
                where = new SimpleExpression(where, tb.OrderNo.Like("%" + model.OrderNo.Trim() + "%"), SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.CreateBy) && model.CreateBy.Trim() != "")
            {
                //创建人（操作人）
                where = new SimpleExpression(where, tb.CreateBy.Like("%" + model.CreateBy.Trim() + "%"), SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.Account) && model.Account.Trim() != "")
            {
                //用户名（YH_User表,登陆账号）
                where = new SimpleExpression(where, _database.Db.YH_User.Account.Like("%" + model.Account.Trim() + "%"), SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.RealName) && model.RealName.Trim() != "")
            {
                //用户真实姓名
                where = new SimpleExpression(where, _database.Db.YH_User.RealName.Like("%" + model.RealName.Trim() + "%"), SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.Phone) && model.Phone.Trim() != "")
            {
                //手机
                where = new SimpleExpression(where, _database.Db.YH_User.Phone.Like("%" + model.Phone.Trim() + "%"), SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.Email) && model.Email.Trim() != "")
            {
                //Email
                where = new SimpleExpression(where, _database.Db.YH_User.Email.Like("%" + model.Email.Trim() + "%"), SimpleExpressionType.And);
            }
            if (model.EndCreateDT != null && model.EndCreateDT.Value.Year != 0001)
            {
                //结束 时间加一天是为了查询结束当天的数据
                where = new SimpleExpression(where, tb.CreateDT < model.EndCreateDT, SimpleExpressionType.And);
            }
            if (model.BeginCreateDT != null && model.BeginCreateDT.Value.Year != 0001)
            {
                //开始时间
                where = new SimpleExpression(where, tb.CreateDT >= model.BeginCreateDT, SimpleExpressionType.And);
            }

            dynamic pc;
            dynamic zj;
            var query = tb
                .Query()
                .LeftJoin(_database.Db.ZJ_AmountChangeType, out zj)
                .On(_database.Db.ZJ_AmountChangeType.ID == tb.AddOrCutType)
                .LeftJoin(_database.Db.YH_User, out pc)
                .On(_database.Db.YH_User.UserID == tb.UserID)
                .Select(
                    tb.ID,
                    tb.UserID,
                    tb.AddOrCutAmount,
                    tb.IsAddOrCut,
                    tb.OldAmount,
                    tb.NewAmount,
                    tb.AddOrCutType,
                    tb.OrderNo,
                    tb.Remark,
                    tb.IsDisplay,
                    tb.CreateBy,
                    tb.CreateDT,

                    zj.TypeName,
                    pc.Phone,
                    pc.Email,
                    pc.NickName,
                    pc.RealName,
                    pc.Account
                )
                .Where(where)
                .OrderByIDDescending();

            var result = new ResultModel
            {
                Data = new SimpleDataPagedList<ZJ_UserBalanceChangeLogModel>(query,
                        model.PagedIndex, model.PagedSize)
            };

            return result;
        }

        /// <summary>
        /// 添加用户账户金额异动记录表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>是否成功</returns>
        public ResultModel AddZJ_UserBalanceChangeLog(ZJ_UserBalanceChangeLogModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.ZJ_UserBalanceChangeLog.Insert(model)
            };
            return result;
        }

        /// <summary>
        /// 更新用户账户金额异动记录表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateZJ_UserBalanceChangeLog(ZJ_UserBalanceChangeLogModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.ZJ_UserBalanceChangeLog.UpdateByID(model)
            };
            return result;
        }

        #region 获取资金异动记录Web
        /// <summary>
        /// 获取用户资金异动记录
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        public ResultModel GetUserBalanceChangeLogList(SearchZJ_UserBalanceChangeLogModel model)
        {
            var log = _database.Db.ZJ_UserBalanceChangeLog;
            var logType = _database.Db.ZJ_AmountChangeType_lang;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal); //

            //移动类型语言ID
            where = new SimpleExpression(where, logType.LanguageID == model.LanguageId, SimpleExpressionType.And);

            if (model.AddOrCutTypeArry.Length > 0)
            {
                //异动类型
                where = new SimpleExpression(where, log.AddOrCutType == model.AddOrCutTypeArry, SimpleExpressionType.And);
            }

            //显示
            where = new SimpleExpression(where, log.IsDisplay == 1, SimpleExpressionType.And);

            if (model.UserID > 0)
            {
                //用户ID
                where = new SimpleExpression(where, log.UserID == model.UserID, SimpleExpressionType.And);
            }
            if (model.EndCreateDT != null && model.EndCreateDT.Value.Year != 0001)
            {
                //结束 时间加一天是为了查询结束当天的数据
                where = new SimpleExpression(where, log.CreateDT < model.EndCreateDT, SimpleExpressionType.And);
            }
            if (model.BeginCreateDT != null && model.BeginCreateDT.Value.Year != 0001)
            {
                //开始时间
                where = new SimpleExpression(where, log.CreateDT >= model.BeginCreateDT, SimpleExpressionType.And);
            }

            var query = log
                .Query()
                .LeftJoin(logType)
                .On(logType.ID == log.AddOrCutType)
                .Select(
                    log.ID,
                    log.AddOrCutAmount,
                    log.IsAddOrCut,
                    log.AddOrCutType,
                    log.OrderNo,
                    log.CreateDT,
                    logType.TypeName
                )
                .Where(where)
                .OrderByCreateDTDescending();

            var result = new ResultModel
            {
                Data = new SimpleDataPagedList<ZJ_UserBalanceChangeLogModel>(query,
                        model.PagedIndex, model.PagedSize)
            };

            return result;
        }
        #endregion 获取资金异动记录Web


        #region 惠粉相关方法
        #region 获取惠粉消费总收益
        /// <summary>
        /// 获取惠粉消费总收益
        /// </summary>
        /// <param name="model">用户Id</param>
        /// <returns></returns>
        public ResultModel GetSellIncome(long userId)
        {
            var result = new ResultModel();
            StringBuilder strSql = new StringBuilder();

            strSql.Append(" SELECT SUM(AddOrCutAmount) amount FROM ZJ_UserBalanceChangeLog ");
            strSql.AppendFormat(" WHERE UserID={0} AND IsDisplay=1  and IsAddOrCut=1 AND ", userId);
            strSql.Append(" AddOrCutType IN (6,7,8,9,10,11,12);");
            //执行sql
            var data = _database.RunSqlQuery(x => x.ToResultSets(strSql.ToString()));
            if (data[0][0].amount == null)
            {
                result.Data = 0M;
            }
            else
            {
                result.Data = data[0][0].amount;
            }
            return result;
        }
        #endregion

        #region 获取账户账单记录
        /// <summary>
        /// 获取账户账单记录
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageNo">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="type">0:收支明细 1.消费记录 2.惠粉收益记录 3.退款 4.分红</param>
        /// <param name="lang">1:中文,2:英文,3:泰文</param>
        /// <returns></returns>
        public ResultModel GetCapitalRecordList(long userId, int lang, int type = 0)
        {
            var result = new ResultModel();
            try
            {
                StringBuilder commonBuilder = new StringBuilder();
                commonBuilder.Append(" FROM dbo.ZJ_UserBalanceChangeLog a");
                commonBuilder.Append(" LEFT JOIN (SELECT zja.ID,zjal.TypeName,zjal.ZJTypeId,zjal.LanguageID,zjal.ZJ_Remark FROM dbo.ZJ_AmountChangeType zja");
                commonBuilder.AppendFormat(" LEFT JOIN dbo.ZJ_AmountChangeType_lang zjal ON zjal.ID = zja.ID WHERE zjal.LanguageID={0}) zj ON a.AddOrCutType=zj.ID", lang);
                commonBuilder.Append(" LEFT JOIN dbo.[Order] s ON a.OrderNo=s.OrderID");
                commonBuilder.Append(" LEFT JOIN yh_user bu on bu.UserID = a.UserID");
                commonBuilder.Append(" LEFT JOIN yh_merchantinfo mi on s.MerchantID = mi.MerchantID");
                commonBuilder.AppendFormat(" WHERE a.IsDisplay=1 and a.UserID={0} AND (a.AddOrCutType IN (2,4,6,7,8,9,10,11,12,13,14))", userId);
                //异动类型:2:购物消费 公司虚拟账号 4:退款 6全球代理消费收益 7省级代理消费收益 8:市级代理消费收益 9:区级代理消费收益 10:感恩粉丝消费收益/感恩[一级分销商]粉丝消费收益 11:感动粉丝消费收益/感动[二级分销商]粉丝消费收益 12:感谢粉丝消费收益/感谢[三级分销商]粉丝消费收益 13省代月度毛利分红 14全球代月度毛利分红
                switch (type)
                {
                    case 1:  //1:消费记录
                        commonBuilder.Append(" AND a.AddOrCutType IN (2)");
                        break;
                    case 2:// 2:惠粉收益记录
                        commonBuilder.Append(" AND (a.AddOrCutType IN (6,7,8,9,10,11,12))");
                        break;
                    case 3:// 3:退款
                        commonBuilder.Append(" AND a.AddOrCutType IN (4)");
                        break;
                    case 4://分红
                        commonBuilder.Append(" AND a.AddOrCutType IN (13,14)");
                        break;
                    default:
                        break;
                }
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append("SELECT a.ID,a.UserID,a.AddOrCutAmount,a.IsAddOrCut,a.OldAmount,a.NewAmount,a.AddOrCutType,a.OrderNo,a.Remark,a.CreateDT,");
                sqlBuilder.Append("bu.Account,mi.ShopName,zj.TypeName,zj.ZJ_Remark");
                sqlBuilder.Append(commonBuilder.ToString());
                sqlBuilder.Append(" ORDER BY a.ID DESC");

                //执行sql
                var data = _database.RunSqlQuery(x => x.ToResultSets(sqlBuilder.ToString()));
                result.IsValid = data[0].Count > 0;
                if (result.IsValid)
                {
                    List<dynamic> source = data[0];
                    result.Data = source;
                }
                return result;
            }
            catch (Exception e)
            {
                result.IsValid = false;
                result.Data = e.Message;
                return result;
            }
        }
        /// <summary>
        /// 获取商品名称
        /// </summary>
        /// <param name="orderNo">订单ID</param>
        /// <param name="lang">1:中文,2:英文,3:泰文</param>
        /// <returns></returns>
        public ResultModel GetProductName(string orderNo, int lang)
        {
            ResultModel result = new ResultModel();
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT d.OrderDetailsID,d.OrderID,dl.ProductName,d.ProductSnapshotID,d.ProductId,d.CostPrice,d.SalesPrice,d.DiscountInfo,");
                strSql.Append("d.Quantity,d.Unit,d.SKU_ProducId,d.SkuName,d.SubTotal,d.Iscomment,d.IsReturn,dl.OrderDetails_langId,dl.LanguageID");
                strSql.Append(" FROM dbo.OrderDetails d LEFT JOIN dbo.OrderDetails_lang dl on d.OrderDetailsID=dl.OrderDetailsID");
                strSql.AppendFormat(" WHERE dl.LanguageID={0} AND d.OrderID='{1}'", lang, orderNo);
                //执行sql
                var data = _database.RunSqlQuery(x => x.ToResultSets(strSql.ToString()));
                result.IsValid = data[0].Count > 0;
                if (result.IsValid)
                {
                    string productName = "";
                    for (int i = 0; i < data[0].Count; i++)
                    {
                        productName += data[0][i].ProductName + ",";
                    }
                    result.Data = productName.Substring(0, productName.Length - 1);
                }
                return result;
            }
            catch (Exception e)
            {
                result.IsValid = false;
                result.Data = e.Message;
                return result;
            }
        }
        /// <summary>
        /// 获取商品图片
        /// </summary>
        /// <param name="orderNo">订单ID</param>
        /// <returns></returns>
        public ResultModel GetProductURL(string orderNo)
        {
            ResultModel result = new ResultModel();
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("SELECT * FROM dbo.ProductPic WHERE ProductID IN (SELECT ProductId FROM OrderDetails WHERE OrderID='{0}') AND Flag=1", orderNo);
                //执行sql
                var data = _database.RunSqlQuery(x => x.ToResultSets(strSql.ToString()));
                result.IsValid = data[0].Count > 0;
                if (result.IsValid)
                {
                    result.Data = data[0][0].PicUrl;
                }
                return result;
            }
            catch (Exception e)
            {
                result.IsValid = false;
                result.Data = e.Message;
                return result;
            }
        }
        #endregion

        #region 获取账户账单记录详情
        /// <summary>
        /// 获取账户账单记录详情 
        /// </summary>
        /// <param name="id">记录Id</param>
        ///  <param name="lang">1:中文,2:英文,3:泰文</param>
        /// <returns></returns>
        public ResultModel GetCapitalRecordDetails(int id, int lang)
        {
            ResultModel result = new ResultModel();
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT a.ID,a.UserID,a.AddOrCutAmount,a.IsAddOrCut,a.OldAmount,a.NewAmount,a.AddOrCutType,a.OrderNo,a.Remark,");
                strSql.Append(" a.CreateBy,a.CreateDT,zj.TypeName,zj.ZJ_Remark,b.Account,b.RealName,z.BankSubbranch,z.BankName,");
                strSql.Append(" z.WithdrawResult,mi.MerchantID,mi.ShopName,o.PayChannel,pp.outOrderId,pp.PaymentOrderID");
                strSql.Append(" FROM ZJ_UserBalanceChangeLog a ");
                strSql.Append(" LEFT JOIN (SELECT zja.ID,zjal.TypeName,zjal.ZJTypeId,zjal.LanguageID,zjal.ZJ_Remark FROM dbo.ZJ_AmountChangeType zja");
                strSql.AppendFormat(" LEFT JOIN dbo.ZJ_AmountChangeType_lang zjal ON zjal.ID = zja.ID WHERE zjal.LanguageID={0}) zj ON a.AddOrCutType=zj.ID", lang);
                strSql.Append(" LEFT JOIN YH_User b on b.UserID=a.UserID");
                strSql.Append(" LEFT JOIN zj_withdraworder z on a.OrderNo = z.OrderNo  and  a.AddOrCutType=3 ");
                strSql.Append(" LEFT JOIN dbo.[Order] o ON o.OrderID = a.OrderNo ");
                strSql.Append(" LEFT JOIN yh_merchantinfo mi on o.MerchantID = mi.MerchantID ");
                strSql.Append(" LEFT JOIN dbo.PaymentOrder_Orders po ON po.OrderID = a.OrderNo ");
                strSql.Append(" LEFT JOIN dbo.PaymentOrder pp ON pp.PaymentOrderID = po.PaymentOrderID ");
                strSql.AppendFormat(" WHERE a.IsDisplay=1  and a.Id={0} ", id);

                //执行sql
                var data = _database.RunSqlQuery(x => x.ToResultSets(strSql.ToString()));
                result.IsValid = data[0].Count > 0;
                if (result.IsValid)
                {
                    List<dynamic> source = data[0];
                    result.Data = source;
                }
                return result;
            }
            catch (Exception e)
            {
                result.IsValid = false;
                result.Data = e.Message;
                return result;
            }
        }
        #endregion

        #region 获取消费收益列表
        /// <summary>
        /// 获取消费收益列表
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="GType">类型（1.感恩惠粉,2.感动惠粉,3.感谢惠粉 4.外围惠粉）</param>
        /// <param name="PageNo">分页码</param>
        /// <param name="PageSize">分页大小</param>
        /// <returns></returns>
        public ResultModel GetJoinAndConsumeList(long userId, int GType, int PageNo, int PageSize)
        {
            ResultModel result = new ResultModel();
            try
            {
                decimal gnTotal = 0;
                decimal gdTotal = 0;
                decimal gxTotal = 0;
                decimal periphery = 0;
                string strSql = "";
                string type = "";
                switch (GType)
                {
                    case 2:
                        type = "11"; break;
                    case 3:
                        type = "12"; break;
                    case 4:
                        type = "6,7,8,9"; break;
                    case 1:
                    default:
                        type = "10";
                        break;
                }
                var log = _database.Db.ZJ_UserBalanceChangeLog;
                //头部数据（固定）
                strSql = string.Format("SELECT SUM(A.AddOrCutAmount) as v1 "
                    + "FROM [ZJ_UserBalanceChangeLog] A WHERE A.UserID= {0} AND A.AddOrCutType IN (10) AND A.IsDisplay = 1 ;", userId);
                strSql += string.Format("SELECT SUM(A.AddOrCutAmount) as v2 "
                    + "FROM [ZJ_UserBalanceChangeLog] A WHERE A.UserID= {0} AND A.AddOrCutType IN (11) AND A.IsDisplay = 1 ;", userId);
                strSql += string.Format("SELECT SUM(A.AddOrCutAmount) as v3 "
                    + "FROM [ZJ_UserBalanceChangeLog] A WHERE A.UserID= {0} AND A.AddOrCutType IN (12) AND A.IsDisplay = 1 ;", userId);
                strSql += string.Format("SELECT SUM(A.AddOrCutAmount) as v4 "
                    + "FROM [ZJ_UserBalanceChangeLog] A WHERE A.UserID= {0} AND A.AddOrCutType IN (6,7,8,9) AND A.IsDisplay = 1 ;", userId);
                //列表数据
                string sql = "SELECT TOP({0}) SUM(A.AddOrCutAmount) AS Amount,D.Account,D.NickName,D.RegisterDate,D.UserID,D.HeadImageUrl   FROM [ZJ_UserBalanceChangeLog] A "
                    + " JOIN [PaymentOrder_Orders] B ON A.OrderNo=B.OrderID "
                    + " JOIN [PaymentOrder] C ON B.PaymentOrderID=C.PaymentOrderID "
                    + " JOIN [YH_User] D ON D.UserID=C.UserID "
                    + " WHERE A.UserID={1} AND A.AddOrCutType IN ({2}) AND A.IsDisplay=1 AND A.IsAddOrCut=1 "
                    + " AND C.UserID NOT IN ( "
                    + " SELECT DISTINCT TOP ({3}) G.UserID FROM [ZJ_UserBalanceChangeLog] E "
                    + " JOIN [PaymentOrder_Orders] F ON E.OrderNo=F.OrderID "
                    + " JOIN [PaymentOrder] G ON F.PaymentOrderID=G.PaymentOrderID "
                    + " WHERE E.UserID={1} AND E.AddOrCutType IN ({2}) AND E.IsDisplay=1 AND E.IsAddOrCut=1 "
                    + " ORDER BY G.UserID ASC ) "
                    + " GROUP BY D.UserID,D.Account,D.NickName,D.RegisterDate,D.HeadImageUrl  ORDER BY D.UserID ASC";
                strSql += string.Format(sql, PageSize, userId, type, (PageNo - 1) * PageSize);
                var list = _database.RunSqlQuery(x => x.ToResultSets(strSql));
                gnTotal = list[0][0].ToScalar() == null ? 0 : list[0][0].ToScalar();
                gdTotal = list[1][0].ToScalar() == null ? 0 : list[1][0].ToScalar();
                gxTotal = list[2][0].ToScalar() == null ? 0 : list[2][0].ToScalar();
                periphery = list[3][0].ToScalar() == null ? 0 : list[3][0].ToScalar();
                List<dynamic> tableList = list[4];
                List<JoinAndConsumeList> datalist = tableList.ToEntity<JoinAndConsumeList>(); //new List<JoinAndConsumeList>();
                result.IsValid = datalist.Count > 0;
                result.Data = new Tuple<decimal, decimal, decimal, decimal, List<JoinAndConsumeList>>(gnTotal, gdTotal, gxTotal, periphery, datalist);
                return result;
            }
            catch (Exception e)
            {
                result.IsValid = false;
                result.Data = e.Message;
                return result;
            }
        }
        #endregion

        #region 惠粉消费收益明细
        /// <summary>
        /// 惠粉消费收益明细
        /// </summary>
        /// <param name="loginId">当前登录人ID</param>
        /// <param name="userId">下级用户id</param>
        /// <param name="gtype">类型（1.感恩惠粉,2.感动惠粉,3.感谢惠粉 4.外围惠粉）</param>
        /// <param name="pageNo">分页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public ResultModel GetConsumeDetails(long loginId, long userId, int gtype, int pageNo, int pageSize)
        {
            ResultModel result = new ResultModel();
            //检查俩两人关系
            try
            {
                string strSql, sql, type;
                strSql = sql = type = "";
                switch (gtype)
                {
                    case 2:
                        type = "11";
                        break;
                    case 3:
                        type = "12";
                        break;
                    case 4:
                        type = "6,7,8,9";
                        break;
                    case 1:
                    default:
                        type = "10";
                        break;
                }
                //string strSql = string.Format(" SELECT A.* FROM [YH_User] A WHERE A.UserID = {0} and {1}", userId, (gtype != 4 ? "A."
                //    + relation + "=" + loginId.ToString() : "charindex('" + loginId + "',A.ParentIDs)>0"));
                //List<dynamic> list = _database.RunSqlQuery(x => x.ToResultSets(strSql))[0];
                //strSql = string.Format("SELECT TOP({0}) A.CreateDT,A.AddOrCutAmount FROM [ZJ_UserBalanceChangeLog] A "
                //    + " WHERE A.UserID NOT IN "
                //    + " ( SELECT TOP({1}) B.UserID FROM [ZJ_UserBalanceChangeLog] B "
                //    + " WHERE B.AddOrCutType=2 AND B.IsDisplay=1 AND B.UserID={2} ) "
                //    + " AND A.IsDisplay=1 AND IsDisplay=1 AND A.UserID={3} ORDER BY A.CreateDT DESC;", pageSize, (pageNo - 1) * pageSize, userId, userId);
                //list = _database.RunSqlQuery(x => x.ToResultSets(strSql))[0];
                sql = " SELECT TOP({0}) SUM(A.AddOrCutAmount) AS Amount,D.Account,D.NickName,D.RegisterDate,D.UserID  FROM [ZJ_UserBalanceChangeLog] A "
                    + " JOIN [PaymentOrder_Orders] B ON A.OrderNo=B.OrderID "
                    + " JOIN [PaymentOrder] C ON B.PaymentOrderID=C.PaymentOrderID "
                    + " JOIN [YH_User] D ON D.UserID=C.UserID "
                    + " WHERE A.UserID={1} AND A.AddOrCutType IN ({2}) AND A.IsDisplay=1 AND A.IsAddOrCut=1 "
                    + " AND C.UserID= {3} AND B.OrderID NOT IN ( "
                    + " SELECT DISTINCT TOP ({4}) E.OrderNo FROM [ZJ_UserBalanceChangeLog] E "
                    + " JOIN [PaymentOrder_Orders] F ON E.OrderNo=F.OrderID "
                    + " JOIN [PaymentOrder] G ON F.PaymentOrderID=G.PaymentOrderID "
                    + " WHERE E.UserID={1} AND E.AddOrCutType IN ({2}) AND E.IsDisplay=1 AND E.IsAddOrCut=1 "
                    + " AND G.UserID={3} ORDER BY E.OrderNo ASC ) "
                    + " GROUP BY D.UserID,D.Account,D.NickName,D.RegisterDate  ORDER BY D.UserID ASC";
                strSql = string.Format(sql, pageSize, loginId, type, userId, (pageNo - 1) * pageSize);
                //strSql = string.Format("SELECT TOP({0}) A.CreateDT,A.AddOrCutAmount FROM [ZJ_UserBalanceChangeLog] A "
                //    + " WHERE A.UserID NOT IN "
                //    + " ( SELECT TOP({1}) B.UserID FROM [ZJ_UserBalanceChangeLog] B "
                //    + " WHERE B.AddOrCutType=2 AND B.IsDisplay=1 AND B.UserID={2} ) "
                //    + " AND A.IsDisplay=1 AND IsDisplay=1 AND A.UserID={3} ORDER BY A.CreateDT DESC;", pageSize, (pageNo - 1) * pageSize, userId, userId);
                List<dynamic> list = _database.RunSqlQuery(x => x.ToResultSets(strSql))[0];
                List<ConsumeDetails> datalist = list.ToEntity<ConsumeDetails>();
                result.Data = datalist;
                result.IsValid = true;
                return result;
            }
            catch (Exception e)
            {
                result.IsValid = false;
                return result;
            }
        }
        #endregion

        #endregion

        public ResultModel GetConsumeList(long userid, int pageNo, int pageSize)
        {
            try
            {
                var tb = _database.Db.ZJ_UserBalanceChangeLog;
                var query = tb.All().Where(tb.UserID == userid && tb.IsDisplay == 1 
                    && (tb.AddOrCutType != 17 || tb.CreateDT < DateTime.Now.Date))
                    .Select(tb.CreateDT, tb.AddOrCutType, tb.AddOrCutAmount, tb.OrderNo, tb.ID, tb.Remark).OrderByDescending(tb.CreateDT);
                ResultModel result = new ResultModel
                {
                    Data = new SimpleDataPagedList<dynamic>(query, pageNo, pageSize)
                };
                return result;
            }
            catch (Exception ex)
            {
                return new ResultModel() { };
            }
        }

        /// <summary>
        /// 获取某一天的返现资金
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public ResultModel GetRebeatAmountByDate(long UserId, DateTime dt)
        {
            DateTime StartDT = dt.Date;
            DateTime EndDT = dt.Date.AddDays(1).AddMilliseconds(-1);
            var tb = _database.Db.ZJ_UserBalanceChangeLog;
            ResultModel result = new ResultModel
            {
                Data = tb.All().Where(tb.UserID == UserId && tb.AddOrCutType == 17//返现
                && tb.CreateDT == StartDT.to(EndDT)).Select(tb.AddOrCutAmount.Sum().As("Rebeat"))
            };
            return result;
        }
    }
}
