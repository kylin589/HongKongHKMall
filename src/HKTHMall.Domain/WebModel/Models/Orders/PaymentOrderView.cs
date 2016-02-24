using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Orders
{
    [DataContract]
    public class PaymentOrderView
    {
        [DataMember(Name = "paymentOrderId")]
        public string PaymentOrderID { get; set; }

        [DataMember(Name = "userId")]
        public long UserID { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        [DataMember(Name = "productAmount")]
        public decimal ProductAmount { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        [DataMember(Name = "realAmount")]
        public decimal RealAmount { get; set; }

        [DataMember(Name = "flag")]
        public int Flag { get; set; }

        [DataMember(Name = "createDT")]
        public System.DateTime CreateDT { get; set; }

        [DataMember(Name = "paymentDate")]
        public Nullable<System.DateTime> PaymentDate { get; set; }

        [DataMember(Name = "payType")]
        public int PayType { get; set; }
        [DataMember(Name = "payChannel")]
        public int PayChannel { get; set; }

        [DataMember(Name = "outOrderId")]
        public string outOrderId { get; set; }

        
        [DataMember(Name = "orderNO")]
        public string OrderNO { get; set; }


        [DataMember(Name = "rechargeAmount")]
        public decimal RechargeAmount { get; set; }

        




    }


}
