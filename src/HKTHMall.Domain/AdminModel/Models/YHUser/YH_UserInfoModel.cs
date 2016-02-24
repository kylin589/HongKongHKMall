using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.YHUser
{
    public class YH_UserInfoModel
    {
        /// <summary>
        /// 用户ID 
        /// </summary>
        public long userId { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string account { get; set; }
        /// <summary>
        /// 是否设置支付密码  0.未设置 1.设置 
        /// </summary>
        public string isPayPassWord { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickName { get; set; }
        /// <summary>
        /// 用户头像地址
        /// </summary>
        public string imageUrl { get; set; }
        /// <summary>
        /// 性别1:男,2:女,0:未设)
        /// </summary>
        public string sex { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string realName { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 生日(时间戳)
        /// </summary>
        public dynamic birthday { get; set; }
        /// <summary>
        /// 三级地区ID
        /// </summary>
        public int tHAreaID { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 个性签名 
        /// </summary>
        public string signature { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal balance { get; set; }
        /// 消费收益
        /// </summary>
        public decimal consumptionIncome { get; set; }
        /// <summary>
        /// 代理商类别:1区2市3省4全球
        /// </summary>
        public int agentType { get; set; }
        /// <summary>
        /// 分销商层级
        /// </summary>
        public int level { get; set; }
        /// <summary>
        /// <summary>
        /// 二维码地址
        /// </summary>
        public string orcodeUrl { get; set; }
        /// <summary>
        /// 待付款
        /// </summary>
        public int obligation { get; set; }
        /// <summary>
        /// 待发货
        /// </summary>
        public int toBeShipped { get; set; }
        /// <summary>
        /// 待收货
        /// </summary>
        public int incomingGoods { get; set; }
        /// <summary>
        /// 待评价
        /// </summary>
        public int evaluationOfStay { get; set; }

        /// <summary>
        /// 用户类型 0:会员 1:商家（待定）
        /// </summary>
        public int userType { get; set; }
    }

}
