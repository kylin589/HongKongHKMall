using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    public class RequestConsumeDetails
    {
        //当前登录用户ID (RSA加密)
        public string loginId { get; set; }
        //下级用户id(下级,值从消费收益列表接口传入过来的) (RSA加密)
        public string userId { get; set; }
        //类型（1.感恩惠粉,2.感动惠粉,3.感谢惠粉,4其他惠粉）
        public int gtype { get; set; }
        //分页码
        public int pageNo { get; set; }
        //分页大小
        public int pageSize { get; set; }
        //1:中文,2:英文,3:泰文
        public int lang { get; set; }
    }
}