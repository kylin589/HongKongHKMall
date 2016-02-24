using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Core.Utils;

namespace HKTHMall.Core.Payment.LinePay
{
    /// <summary>
    /// LinePay支付工具类
    /// </summary>
    public class ReserveService
    {
        /// <summary>
        /// 渠道Id
        /// </summary>
        private static string ChannelId = ConfigurationManager.AppSettings["LinePayChannelId"];

        /// <summary>
        /// 渠道密钥
        /// </summary>
        private static string ChannelSecret = ConfigurationManager.AppSettings["LinePayChannelSecret"];


        /// <summary>
        /// 请求Url
        /// </summary>
        private static string ReserveUrl = ConfigurationManager.AppSettings["LinePayReserveUrl"];

        /// <summary>
        /// 请求参数
        /// </summary>
        public ReserveDetailReq ReserveDetailReq { get; set; }


        public dynamic ReserveRequest()
        {
            string jsonStr = JsonConverts.ToJson(ReserveService.GetProperties(this.ReserveDetailReq));
            return this.ReserveRequest(Encoding.GetEncoding("UTF-8").GetBytes(jsonStr));
        }


        private dynamic ReserveRequest(byte[] data)
        {
            Stream responseStream;
            HttpWebRequest request = WebRequest.Create(ReserveUrl) as HttpWebRequest;
            if (request == null)
            {
                throw new ApplicationException(string.Format("Invalid url string: {0}", ReserveUrl));
            }
            request.ContentType = "application/json; charset=UTF-8";
            request.Headers.Add("X-LINE-ChannelId:" + ChannelId);
            request.Headers.Add("X-LINE-ChannelSecret:" + ChannelSecret);
            request.Method = "POST";
            request.ContentLength = data.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
            try
            {
                responseStream = request.GetResponse().GetResponseStream();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            string str = string.Empty;
            using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
            {
                str = reader.ReadToEnd();
            }
            responseStream.Close();

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new DynamicJsonConverter() });

            dynamic obj = serializer.Deserialize(str, typeof(object));
            return obj;
        }


        /// <summary>
        /// 获取相关参数属性
        /// </summary>
        /// <param name="reserveDetailReq">请求实体</param>
        /// <returns></returns>
        private static Dictionary<string, string> GetProperties(ReserveDetailReq reserveDetailReq)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("productName", reserveDetailReq.productName);
            properties.Add("productImageUrl", reserveDetailReq.productImageUrl);
            properties.Add("amount", reserveDetailReq.amount);
            properties.Add("currency", reserveDetailReq.currency);
            properties.Add("orderId", reserveDetailReq.orderId);
            properties.Add("confirmUrl", reserveDetailReq.confirmUrl);
            properties.Add("cancelUrl", reserveDetailReq.cancelUrl);
            return properties;
        }
    }
}
