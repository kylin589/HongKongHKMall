using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HKTHMall.WebApi.Models.Request
{
    public class RequestCreateGroup
    {
        /// <summary>
        /// 用户ID(加密)
        /// </summary>
        [Required]
        public string userId { get; set; }

        /// <summary>
        /// 推荐ID(加密)
        /// </summary>
        public string referrerId { get; set; }

        [Required]
        [Range(1,5)]
        public int lang { get; set; }
    }
}