using FluentValidation.Attributes;
using HKTHMall.Domain.OfficialWeb.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.OfficialWeb.Models.Suppliers
{
    [Validator(typeof(SuppliersValidator))]
    public class SuppliersModel
    {
        public long SupplierId { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 地址(泰国地区信息多语言表 ID)
        /// </summary>
        public int THAreaID { get; set; }

        /// <summary>
        /// 地址 (泰国地区信息多语言表 AreaName)
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// THArea【泰国地区信息表】 简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// THArea【泰国地区信息表】 二级父ID（市）
        /// </summary>
        public int ShiTHAreaID { get; set; }

        /// <summary>
        /// THArea【泰国地区信息表】 一级父ID（省）
        /// </summary>
        public int ShengTHAreaID { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string LinkMan { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 手机
        /// </summary>

        public string Mobile { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }



        /// <summary>
        /// 密码
        /// </summary>
        public string OldPassWord { get; set; }


    }
}
