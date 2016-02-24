using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.YH
{

    public partial class YH_UserVisitingCard
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string HeadImgUrl { get; set; }
        /// <summary>
        /// 二维码地址
        /// </summary>
        public string CurrentCardUrl { get; set; }
        /// <summary>
        /// 是否生成
        /// </summary>
        public byte IsSuccess { get; set; }
        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime? CreateDt { get; set; }
    }
}
