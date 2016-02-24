using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.New
{
    public class SearchNewInfoModel:Paged
    {
        public long NewInfoId { get; set; }
        /// <summary>
        /// 新闻类型
        /// </summary>
        public int NewType { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string NewTitle { get; set; }

        /// <summary>
        /// 新闻内容
        /// </summary>
        public string NewContent { get; set; }

        /// <summary>
        /// 是否推荐 0否,1是(整张表只有一个推荐,必须有图片才能推荐)
        /// </summary>
        public int IsRecommend { get; set; }
    }
}
