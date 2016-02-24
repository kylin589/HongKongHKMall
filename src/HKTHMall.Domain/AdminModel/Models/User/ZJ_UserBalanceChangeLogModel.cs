using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.User
{
    /// <summary>
    /// 
    /// </summary>
    [Validator(typeof(ZJ_UserBalanceChangeLogValidator))]
    public class ZJ_UserBalanceChangeLogModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 异动金额
        /// </summary>
        public decimal AddOrCutAmount { get; set; }

        /// <summary>
        /// 增或减（1:增,0:减）
        /// </summary>
        public int IsAddOrCut { get; set; }

        /// <summary>
        /// 原余额
        /// </summary>
        public decimal OldAmount { get; set; }

        /// <summary>
        /// 变动后余额
        /// </summary>
        public decimal NewAmount { get; set; }

        /// <summary>
        /// 异动类型
        /// </summary>
        public int AddOrCutType { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否显示（0:否,1:是）
        /// </summary>
        public int IsDisplay { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime CreateDT { get; set; }

        /// <summary>
        /// 用户名（YH_User表,登陆账号）
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 用户手机 （YH_User表）
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 用户Email （YH_User表）
        /// </summary>
        public string Email { get; set; }

        public string NickName { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 异动类型名称
        /// </summary>
        public string TypeName { get; set; }

    }
}
