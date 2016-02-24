using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Core.Payment.LinePay
{
    public class ReserveDetailReq
    {
        /// <summary>
        /// 產品名稱(charset:"UTF-8") 4000长度 必填
        /// </summary>
        public string productName
        {
            get;
            set;
        }
        /// <summary>
        /// 產品影像URL 500长度（或使用品牌logo URL）大小：84 x 84 必填
        /// </summary>
        public string productImageUrl
        {
            get;
            set;
        }
        /// <summary>
        ///  付款金額 必填
        /// </summary>
        public string amount
        {
            get;
            set;
        }
        /// <summary>
        /// 货币 THB 必填
        /// </summary>
        public string currency
        {
            get;
            set;
        }
        /// <summary>
        /// 買家在LINE Pay選擇付款方式並輸入密碼後，被重新導向到商家的 URL 500长度必填
        /// 在重新導向的URL 上，商家可以呼叫付款confirm API 並完成付款。 LINE Pay 會傳遞額外的"transactionId" 參數
        /// </summary>
        public string confirmUrl
        {
            get;
            set;
        }
        /// <summary>
        /// confirmUrl 類型。 非必填
        /// 買家在LINE Pay 選擇付款方式並輸入密碼後，被重新導向到的URL 所屬的類型。
        /// CLIENT: 手機交易流程(預設)  SERVER: 網站交易流程。用戶只需要查看LINE Pay 的付款資訊畫面，然後通知商家伺服器可以付款。
        /// </summary>
        public string confirmUrlType
        {
            get;
            set;
        }
        /// <summary>
        /// 取消付款頁面的URL(當LINE Pay用戶取消付款後，從LINE應用程式付款,畫 面重新導向的 URL( 取消付款後，透過行動裝置進入商家應用程式或網站的商家 URL)) 500长度  非必填
        /// </summary>
        public string cancelUrl
        {
            get;
            set;
        }
        /// <summary>
        /// 订单号或支付单号 必填 100长度
        /// </summary>
        public string orderId
        {
            get;
            set;
        }

    }
}
