using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators.New;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.New
{
    [Validator(typeof(NewInfoValidator))]
    public class NewInfoModel
    {
        public long NewInfoId { get; set; }

        /// <summary>
        /// 新闻类型 0公告 1特惠
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

        /// <summary>
        /// 新闻图片
        /// </summary>
        public string NewImage { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime CreateDT { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public Nullable<System.DateTime> UpdateDT { get; set; }
    }
}
