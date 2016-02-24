using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Users
{
    /// <summary>
    /// 邮件订阅类
    /// </summary>
    public class MailSubscriptionModel
    {
        public MailSubscriptionModel()
        {
        }
        public long ID { get; set; }
        public long? MsID { get; set; } 
        public string Mobile { get; set; }
        public string Email { get; set; }
        public DateTime? SubDate { get; set; }  
        public string Ip { get; set; }
        public long? UserID { get; set; }
        public long? ProductId { get; set; }
        public int SubType { get; set; }
        public int SendStatus { get; set; }
    }
}
