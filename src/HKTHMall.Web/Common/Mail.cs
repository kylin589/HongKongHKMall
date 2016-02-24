using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using HKTHMall.Core;
using HKTHMall.Domain.Models.Sys;
using HKTHMall.Services.Sys;
using BrCms.Framework.Infrastructure;
using Autofac;
//using HKTHMall.Domain.Models;

namespace HKTHMall.Web.Common
{
    public class Mail
    {
       // private readonly IParameterSetService _ParameterSetService;
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="toEmail">接收方邮件地址</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件正文内容</param>
        /// <param name="Attachments">附件地址</param>
        /// <returns></returns>
        /// <author>Eric</author>
        /// <date>2009-07-30</date>
        public static bool sendMail(string toEmail, string title, string content, string Attachments="")
        {
            ParameterSetService parameterSet=new ParameterSetService();

            //string strHost = parameterSet.GetParametePValueById(1215894596).Data;//"go.nshub.com"; // STMP服务器地址
            //string strAccount = parameterSet.GetParametePValueById(1215894597).Data;//"do-not-reply-for-verify-only@hksjinformation.com"; SMTP服务帐号
            //string strPwd = parameterSet.GetParametePValueById(1215894598).Data;//"CMNeGPmAp%ev"; // SMTP服务密码
            //string strFrom = parameterSet.GetParametePValueById(1215894599).Data;//"do-not-reply-for-verify-only@hksjinformation.com";// ("SENDER_EMAIL") 发送方邮件地址
            //string nickName = parameterSet.GetParametePValueById(1215894600).Data;//"dddde";//("SENDER_NICKNAME") 发送人昵称

            //string strHost = "smtp.dealontop.com";//go.nshub.com"; // STMP服务器地址
            //string strAccount = "postmaster@dealontop.com";//"do-not-reply-for-verify-only@hksjinformation.com"; SMTP服务帐号
            //string strPwd = "HUIxin88";//"CMNeGPmAp%ev"; // SMTP服务密码
            //string strFrom = "postmaster@dealontop.com";//"do-not-reply-for-verify-only@hksjinformation.com";// ("SENDER_EMAIL") 发送方邮件地址
            //string nickName = parameterSet.GetParametePValueById(1215894600).Data;//"dddde";//("SENDER_NICKNAME") 发送人昵称


            string strHost = "smtp.8hkhk.com";//go.nshub.com"; // STMP服务器地址
            string strAccount = "postmaster@8hkhk.com";//"do-not-reply-for-verify-only@hksjinformation.com"; SMTP服务帐号
            string strPwd = "HUIxin88";//"CMNeGPmAp%ev"; // SMTP服务密码
            string strFrom = "postmaster@8hkhk.com";//"do-not-reply-for-verify-only@hksjinformation.com";// ("SENDER_EMAIL") 发送方邮件地址
            string nickName = parameterSet.GetParametePValueById(1215894600).Data;//"dddde";//("SENDER_NICKNAME") 发送人昵称


            using (SmtpClient _smtpClient = new SmtpClient())
            {
                _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;    // 指定电子邮件发送方式
                _smtpClient.Host = strHost;   // 指定SMTP服务器
                _smtpClient.Port = 25;
                _smtpClient.Credentials = new System.Net.NetworkCredential(strAccount, strPwd); // 用户名和密码
                using (MailMessage _mailMessage = new MailMessage())
                {
                    try
                    {
                        _mailMessage.From = new MailAddress(strFrom, nickName);
                        _mailMessage.To.Add(toEmail);
                        _mailMessage.Subject = title; // 主题
                        _mailMessage.Body = content; // 内容
                        _mailMessage.BodyEncoding = System.Text.Encoding.UTF8; // 正文编码
                        _mailMessage.IsBodyHtml = true; // 设置为HTML格式
                        _mailMessage.Priority = MailPriority.High; // 优先级

                        //不被当作垃圾邮件的关键代码--Begin            
                        _mailMessage.Headers.Add("X-Priority", "3");
                        _mailMessage.Headers.Add("X-MSMail-Priority", "Normal");
                        _mailMessage.Headers.Add("X-Mailer", "Microsoft Outlook Express 6.00.2900.2869");   //本文以outlook名义发送邮件,不会被当作垃圾邮件            
                        _mailMessage.Headers.Add("X-MimeOLE", "Produced By Microsoft MimeOLE V6.00.2900.2869");
                        _mailMessage.Headers.Add("ReturnReceipt", "1");
                        //不被当作垃圾邮件的关键代码--End   

                        if (Attachments != "")
                        {
                            Attachment att = new Attachment(Attachments);
                            _mailMessage.Attachments.Add(att); // 添加附件
                        }
                        _smtpClient.Send(_mailMessage);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        //ILog log = LogManager.GetLogger("Mail");
                        //log.Error(ex.Message);
                        //throw new Exception();
                        return false;
                    }
                }
            }
        }
    }
}