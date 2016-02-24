using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Product
{
    public class ProductCommentModel
    {
        /// <summary>
        /// 评论ID
        /// </summary>
        public long ProductCommentId { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public Nullable<long> ProductId { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 评论星级
        /// </summary>
        public byte CommentLevel { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string CommentContent { get; set; }

        /// <summary>
        /// 评论时间
        /// </summary>
        public Nullable<System.DateTime> CommentDT { get; set; }


        /// <summary>
        /// 商品model
        /// </summary>
        public virtual HKTHMall.Domain.Entities.Product Product { get; set; }

        /// <summary>
        /// 商品名称（Product_Lang商品多语言扩展表）
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 用户手机号（用户表）
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 用户HeadImageUrl（用户表）
        /// </summary>
        public string HeadImageUrl { get; set; }

        /// <summary>
        /// 用户Email（用户表）
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public byte CheckStatus { get; set; }

        /// <summary>
        /// 是否匿名
        /// </summary>
        public byte IsAnonymous { get; set; }

        public string LablesStr 
        { 
            get;
            set; 
        }

        public List<CommentLablesList> LablesList
        {
            get
            {
                return CommentLablesList.ReturnList(LablesStr);
            }
        }

    }

    public class CommentCount
    {
        public int CommentTotal { get; set; }
        public int CommentHaoPing { get; set; }
        public int CommentChaPing { get; set; }
        public int CommentZhongPing { get; set; }

    }

    public class CommentLables
    {
        /// <summary>
        /// 自增列
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 评论ID，外键(级联删除)，关联SP_ProductComment.ProductCommentId
        /// </summary>
        public long ProductCommentId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 印象标签
        /// </summary>
        public long Labels { get; set; }
    }

    public class CommentLablesGroup
    {
        public long Labels { get; set; }
        public int totalcnt { get; set; }
    }

    public class CommentLablesList
    {
        private long _label;
        private int _cnt;
        private string _name;

        public long Labels { get{return _label;} }
        public int totalcnt { get { return _cnt; } }
        public string LabelsName { get { return _name; } }


        private CommentLablesList(long _labels,int _totalcnt,string _labelsname)
        {
            _label = _labels;
            _cnt = _totalcnt;
            _name = _labelsname;
        }

        private static readonly List<CommentLablesList> _list;
        static CommentLablesList()
        {
            _list = new List<CommentLablesList>(){
                new CommentLablesList(1,0,"HOME_RATE_LABLE_ONE"),
                new CommentLablesList(2,0,"HOME_RATE_LABLE_TWO"),
                new CommentLablesList(3,0,"HOME_RATE_LABLE_THREE"),
                new CommentLablesList(4,0,"HOME_RATE_LABLE_FOUR"),
                new CommentLablesList(5,0,"HOME_RATE_LABLE_FIVE"),
                new CommentLablesList(6,0,"HOME_RATE_LABLE_SIX"),
                new CommentLablesList(7,0,"HOME_RATE_LABLE_SEVEN"),
                new CommentLablesList(8,0,"HOME_RATE_LABLE_EIGHT"),
                new CommentLablesList(9,0,"HOME_RATE_LABLE_NIGHT")
            };
        }

        public static List<CommentLablesList> ReturnList(List<CommentLablesGroup> t)
        {
            bool flag = false;
            List<CommentLablesList> ret = new List<CommentLablesList>();

            for(int i=0;i<_list.Count;i++)
            {
                flag = false;
                for(int j=0;j<t.Count;j++)
                {
                    if(t[j].Labels == _list[i].Labels && t[j].totalcnt>0)
                    {
                        ret.Add(new CommentLablesList(_list[i].Labels, t[j].totalcnt, _list[i].LabelsName));
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    ret.Add(_list[i]);
                }
            }

            return ret;
        }

        public static List<CommentLablesList> ReturnList(string labelstr)
        {
            List<CommentLablesList> _ret = new List<CommentLablesList>();
            if (string.IsNullOrEmpty(labelstr))
                return _ret;

            long tmp=0;
            string[] split = labelstr.Split(',');

            foreach(string s in split)
            {
                if(Int64.TryParse(s,out tmp))
                {
                    foreach(CommentLablesList m in _list)
                    {
                        if (tmp == m.Labels)
                        {
                            _ret.Add(new CommentLablesList(m.Labels, 0, m.LabelsName));
                            break;
                        }
                    }
                }
            }

            return _ret;
        }

        public static List<CommentLablesList> ReturnListAll(string labelstr)
        {
            List<CommentLablesGroup> _ret = new List<CommentLablesGroup>();
            if (string.IsNullOrEmpty(labelstr))
                return ReturnList(_ret);

            long tmp = 0;
            string[] split = labelstr.Split(',');

            foreach (string s in split)
            {
                if (Int64.TryParse(s, out tmp))
                {
                    CommentLablesGroup m = new CommentLablesGroup();
                    m.Labels = tmp;
                    m.totalcnt = 0;
                    _ret.Add(m);
                }
            }

            return ReturnList(_ret);
        }
        public static List<CommentLablesList> ReturnListAll()
        {
            return _list;
        }
    }
}
