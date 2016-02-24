using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Web;
using System.Xml;
using System.Collections;
using System.IO;

namespace HKTHMall.Core
{
    public class SendSMS
    {
        /// <summary>
        /// 发送泰国手机短信
        /// </summary>
        /// <param name="mobile">接收手机号,多个手机号以,分开</param>
        /// <param name="message">消息内容</param>
        /// <returns></returns>
        public bool SendTHSMS(string mobile,string message)
        {
            WebPostRequest myPost = new WebPostRequest("http://www.thaibulksms.com/sms_api_test.php");
            myPost.Add("username", "Your Username");
            myPost.Add("password", "Your Password");
            myPost.Add("msisdn", mobile);
            myPost.Add("message", message);
            myPost.Add("force", "standard"); //credit type(standard/premium)
            myPost.Add("sender", "THAIBULKSMS"); //sender
            myPost.Add("ScheduledDelivery", ""); //YYmmddhm

            bool result = false;
            try
            {
                string returnData = myPost.GetResponse();
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(returnData);
                XmlNodeList xnList = xml.SelectNodes("/SMS");
                int count_node = xnList.Count;
                if (count_node > 0)
                {
                    foreach (XmlNode xn in xnList)
                    {

                        XmlNodeList xnSubList = xml.SelectNodes("/SMS/QUEUE");
                        int countSubNode = xnSubList.Count;
                        if (countSubNode > 0)
                        {
                            foreach (XmlNode xnSub in xnSubList)
                            {
                                if (xnSub["Status"].InnerText.ToString() == "1")
                                {
                                    string msisdn = xnSub["Msisdn"].InnerText;
                                    string useCredit = xnSub["UsedCredit"].InnerText;
                                    string creditRemain = xnSub["RemainCredit"].InnerText;
                                    //Console.WriteLine("Send SMS to {0} Success.Use credit {1} credit, Credit Remain {2} Credit", msisdn, useCredit, creditRemain);
                                    result = true;
                                }
                                else
                                {
                                    string sub_status_detail = xnSub["Detail"].InnerText;
                                    Console.WriteLine("Error: {0}", sub_status_detail);
                                    result = false;
                                }
                            }
                        }
                        else
                        {
                            if (xn["Status"].InnerText == "0")
                            {
                                string status_detail = xn["Detail"].InnerText;
                                Console.WriteLine("Error: {0}", status_detail);
                            }
                            result = false;
                        }
                    }
                }

                Console.WriteLine("Press any key to end...");
                Console.Read();
            }
            catch
            {
                //Console.WriteLine("Error to sending");
                //Console.WriteLine("Press any key to end...");
                //Console.Read();
                result = false;
            }
            return result;
        }
    }

    class WebPostRequest
    {
        WebRequest theRequest;
        HttpWebResponse theResponse;
        ArrayList theQueryData;

        public WebPostRequest(string url)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            theRequest = WebRequest.Create(url);
            theRequest.Method = "POST";
            theQueryData = new ArrayList();
        }

        public void Add(string key, string value)
        {
            theQueryData.Add(String.Format("{0}={1}", key, value));
        }

        public string GetResponse()
        {
            // Set the encoding type
            theRequest.ContentType = "application/x-www-form-urlencoded";

            // Build a string containing all the parameters
            string Parameters = String.Join("&", (String[])theQueryData.ToArray(typeof(string)));
            theRequest.ContentLength = Parameters.Length;

            // We write the parameters into the request
            StreamWriter sw = new StreamWriter(theRequest.GetRequestStream());
            sw.Write(Parameters);
            sw.Close();

            // Execute the query
            theResponse = (HttpWebResponse)theRequest.GetResponse();
            StreamReader sr = new StreamReader(theResponse.GetResponseStream());
            return sr.ReadToEnd();
        }

    }
}
