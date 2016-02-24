using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Shipment
{
    /// <summary>
    /// 香港邮费模板
    /// </summary>
    public class YF_FareTempModel
    {
        /// <summary>
        /// ID,自增
        /// </summary>
        public int FareTempID { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 发货时间
        /// </summary>
        public int DeliveryTime { get; set; }
        /// <summary>
        /// 是否包邮
        /// </summary>
        public int IsFreeShip { get; set; }
        /// <summary>
        /// 计价方式
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 初始量
        /// </summary>
        public decimal InitialAmount { get; set; }
        /// <summary>
        /// 初始值
        /// </summary>
        public decimal InitialValue { get; set; }
        /// <summary>
        /// 递加量
        /// </summary>
        public decimal AdditiveAmount { get; set; }
        /// <summary>
        /// 递加值
        /// </summary>
        public decimal AdditiveValue { get; set; }
        /// <summary>
        /// 到达多少金额包邮
        /// </summary>
        public bool IsFree { get; set; }
        /// <summary>
        /// 包邮金额
        /// </summary>
        public decimal IsFreeValue { get; set; }
        /// <summary>
        /// 寄送地址ID
        /// </summary>
        public long AddressID { get; set; }
        /// <summary>
        /// 寄送地址
        /// </summary>
        public string Address { get; set; }
    }
}
