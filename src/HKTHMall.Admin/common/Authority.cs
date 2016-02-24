using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HKTHMall.Core;
using HKTHMall.Domain.Models.AC;
using HKTHMall.Services.AC;

namespace HKTHMall.Admin.common
{
    public class Authority
    {
        private readonly IAC_RoleService _aC_RoleService;
        //private readonly IAC_FunctionService _FunctionService;
        //private readonly IAC_ModuleService _ModuleService;

        public Authority(IAC_RoleService aC_RoleService)
        {
            _aC_RoleService = aC_RoleService;
            //this._ModuleService = _ModuleService;
            //this._FunctionService = _FunctionService;
        }
        /// <summary>
        /// 检查按钮权限
        /// </summary>
        /// <param name="funcNum"></param>
        /// <returns></returns>
        public bool CheckAction(int funcNum)
        {
            bool res = false;
            int roleId = UserInfo.CurrentUserRoleID;
            List<AC_FunctionModel> funList = new List<AC_FunctionModel>();
            if (roleId > 0)
            {
                funList = (_aC_RoleService.GetFunctionList(roleId)).Data;
                var tempList = funList.Where(p => p.FunctionID == funcNum).ToList();
                if (tempList.Count > 0)
                {
                    res = true;
                }
            }
            return res;
        }
        public bool Add
        {
            get;
            set;
        }
        /// <summary>
        /// 删除权限
        /// </summary>
        public bool Delete
        {
            get;
            set;
        }
        /// <summary>
        /// 修改权限
        /// </summary>
        public bool Edit
        {
            get;
            set;
        }
        /// <summary>
        /// 查看权限
        /// </summary>
        public bool View
        {
            get;
            set;
        }
        /// <summary>
        /// 审核权限
        /// </summary>
        public bool Audit
        {
            get;
            set;
        }
        /// <summary>
        /// 重置密码权限
        /// </summary>
        public bool Reset
        {
            get;
            set;
        }
        /// <summary>
        /// 移动权限
        /// </summary>
        public bool Move
        {
            get;
            set;
        }
        /// <summary>
        /// 启用禁用权限
        /// </summary>
        public bool Enable
        {
            get;
            set;
        }
        /// <summary>
        /// 品牌关联
        /// </summary>
        public bool BrandAssociation
        {
            get;
            set;
        }
        /// <summary>
        /// 用户帐单
        /// </summary>
        public bool UserAccount
        {
            get;
            set;
        }
        /// <summary>
        /// 商城消费订单
        /// </summary>
        public bool ConsumerOrders
        {
            get;
            set;
        }
        /// <summary>
        /// 锁定
        /// </summary>
        public bool Lock
        {
            get;
            set;
        }
        /// <summary>
        /// 解锁
        /// </summary>
        public bool Unlock
        {
            get;
            set;
        }
        /// <summary>
        /// 重置交易密码
        /// </summary>
        public bool ResetTradPwd
        {
            get;
            set;
        }
        /// <summary>
        /// 恢复
        /// </summary>
        public bool Restore
        {
            get;
            set;
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        public bool IsGoodsReceipt
        {
            get;
            set;
        }

        /// <summary>
        /// 确认退款
        /// </summary>
        public bool IsRefund
        {
            get;
            set;
        }

        /// <summary>
        /// 供应商采购单明细
        /// </summary>
        public bool PurchaseOrderDetails
        {
            get;
            set;
        }

        /// <summary>
        /// 升级商家
        /// zhoub 20150922
        /// </summary>
        public bool UpgradeMerchant
        {
            get;
            set;
        }

        /// <summary>
        /// 升级代理商
        /// zhoub 20150925
        /// </summary>
        public bool UpgradeAgent
        { get; set; }

        /// <summary>
        /// 修改价格权限
        /// 刘文宁 20160218
        /// </summary>
        public bool ModifyPrice
        { get; set; }
    }
}