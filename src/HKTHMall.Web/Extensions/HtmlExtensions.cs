using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;
using System.Linq;
using HKSJ.Common;
using System.Web.Mvc.Html;
using HKTHMall.Core.Config;

using System.Collections.Specialized;
using HKTHMall.Services.Common.MultiLangKeys;

namespace System.Web.Mvc
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// 分页控件
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">每页条数</param>
        /// <param name="count">总条数</param>
        /// <param name="type">那种方式 0 URL 1:ajax 2:from</param>
        /// <returns></returns>
        public static string ToPageHtml(this HtmlHelper html, long pageindex, int pagesize, long count, int type = 0, string url = null)
        {
            StringBuilder sb = new StringBuilder();
            long RecordCount = count;

            long curPage = pageindex;
            long PageSize = pagesize;
            long pageSum = count % pagesize > 0 ? count / pagesize + 1 : count / pagesize; //总页数
            if (pageSum <= 1) return "";
            RouteValueDictionary vs = html.ViewContext.RouteData.Values;
            var lihtml = "<li><a class='cut' href='javascript:;'>{0}</a></li>";
            if (RecordCount > 0)
            {
                int pageMaxSum = 2; //分页栏当前页按钮或左或右最多显示的按钮
                if (curPage < 1) curPage = 1;
                if (curPage > pageSum) curPage = pageSum;
                sb.AppendLine("<div class='A_page'><ul class='tunepage' data-xxx='" + url + "'>");
                //上一页
                if (1 != curPage)
                {
                    sb.Append("<li><a class='cut prev' href='javascript:;'><i class='page_ico'></i>" + CultureHelper.GetLangString("ORDER_LIST_PREVIOUSPAGE") + "</a></li>");//上一页
                }
                long startIndex = 1;
                long endIndex = pageSum;
                if (pageSum > pageMaxSum * 2 + 1)
                {
                    if (curPage - pageMaxSum > 0)
                    {
                        if (curPage + pageMaxSum < pageSum)
                        {
                            startIndex = curPage - pageMaxSum;
                            endIndex = curPage + pageMaxSum;
                        }
                        else
                        {
                            startIndex = pageSum - pageMaxSum * 2;
                        }
                    }
                    else
                    {
                        endIndex = pageMaxSum * 2 + 1;
                    }
                }
                if (pageSum > 1)
                {
                    if (startIndex.Equals(2))
                    {
                        sb.AppendFormat(lihtml, 1);
                        endIndex--;
                    }
                    else if (startIndex > 2)
                    {
                        sb.AppendFormat(lihtml, 1);
                        sb.AppendLine("<li><a href='javascript:;'>...</a></li>");
                    }
                    //页码
                    for (var i = startIndex; i <= endIndex; i++)
                    {
                        if (curPage == i)
                        {
                            sb.Append("<li><a href='javascript:;' class=\"select\">" + curPage + "</a></li>");
                        }
                        else
                        {
                            sb.AppendFormat(lihtml, i);
                        }
                    }
                    if (endIndex < pageSum - 1)
                    {
                        sb.AppendLine("<li><a href='javascript:;'>...</a></li>");
                        sb.AppendFormat(lihtml, pageSum);
                    }
                    else if (endIndex < pageSum)
                    {
                        if (curPage == pageSum)
                        {
                            sb.Append("<li><a href='javascript:;' class=\"select\">" + curPage + "</a></li>");
                        }
                        else
                            sb.AppendFormat(lihtml, pageSum);
                    }
                }
                //下一页
                if (curPage != pageSum && pageSum > 1)
                {
                    sb.Append("<li><a class='cut next' href='javascript:;'>" + CultureHelper.GetLangString("ORDER_LIST_NEXTPAGE") + "<i class='page_ico2'></i></a></li>");//下一页
                }
                sb.AppendFormat("<li>" + CultureHelper.GetLangString("ORDER_LIST_TOTAL") + "{0}" + CultureHelper.GetLangString("ORDER_LIST_PAGE") + "," + CultureHelper.GetLangString("ORDER_LIST_TOPAGENUM") + "<input value='{1}' class='pro_bk' type='text'>" + CultureHelper.GetLangString("GOTOPAGE") + "</li>", pageSum, pageindex);
                sb.Append("<li><a href='javascript:;' class='cut pggo'>" + CultureHelper.GetLangString("ORDER_LIST_SURE") + "</a></li>");
                sb.AppendLine("</ul></div>");
                //sb.AppendLine("<script>jQuery('.tunepage>li>a').Page({type:" + type + "})</script>");
                sb.AppendFormat("<input type='hidden' class='PgCount' value='{0}'>", pageSum);
                sb.AppendFormat("<input name='page' type='hidden' class='PgIndex' value='{0}'>", pageindex);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 分页控件新，add by liujc
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">每页条数</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public static string ToPageHtmlNew(this HtmlHelper html, long pageindex, int pagesize, long count)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder url = new StringBuilder();
            url.Append(HttpContext.Current.Request.Url.AbsolutePath + "?page={0}");

            long pageMaxSum = 10;
            long RecordCount = count;
            long curPage = pageindex;
            long PageSize = pagesize;
            long pageSum = count % pagesize > 0 ? count / pagesize + 1 : count / pagesize; //总页数

            if (pageSum <= 1 || RecordCount<=0) return "";

            if (curPage < 1) curPage = 1;
            if (curPage > pageSum) curPage = pageSum;

            long prev = (curPage < 2 ? 1 : curPage - 1);
            long next = (curPage >= pageSum ? pageSum : curPage + 1);

            NameValueCollection collection = HttpContext.Current.Request.QueryString;
            string[] keys = collection.AllKeys;

            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].ToLower() != "page")
                    url.AppendFormat("&{0}={1}", keys[i], collection[keys[i]]);
            }

            sb.Append("<div class=\"sectionPaging\"><ul>");
            sb.Append("<li class=\"secPrevPage\"><a href=\"" + string.Format(url.ToString(), prev) + "\"><i class=\"fa-angle-left\"></i>" + CultureHelper.GetLangString("ORDER_LIST_PREVIOUSPAGE") + "</a></li>");

            #region 数字分页

            long k = curPage - pageMaxSum / 2 - pageMaxSum % 2;
            if (k < 1)
            {
                k = 1;
            }
            long m = k + pageMaxSum - 1;
            if (m > pageSum)
            {
                m = pageSum;
            }

            for (long i = k; i <= m; i++)
            {
                if (i == curPage)
                    sb.AppendFormat("<li><a href=\"{0}\" class=\"secPageOn\">{1}</a></li>", string.Format(url.ToString(), i), i);
                else
                    sb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", string.Format(url.ToString(), i), i);
            }

            #endregion

            sb.Append("<li class=\"secNextPage\"><a href=\"" + string.Format(url.ToString(), next) + "\">" + CultureHelper.GetLangString("ORDER_LIST_NEXTPAGE") + "<i class=\"fa-angle-right\"></i></a></li>");
            sb.Append("<span class=\"clearfix\"></span></ul></div>");

            return sb.ToString();
        }

        public static string ToPageHtmlFullUrl(this HtmlHelper html, int pageindex, int pagesize, int count)
        {
            string fullurl = html.ViewContext.HttpContext.Request.RawUrl;
            int valuenum = fullurl.Split('?').Length;
            int pagenum = fullurl.IndexOf("page");
            StringBuilder sb = new StringBuilder();
            int RecordCount = count;
            int curPage = pageindex;
            int PageSize = pagesize;
            int pageSum = count % pagesize > 0 ? count / pagesize + 1 : count / pagesize; //总页数
            RouteValueDictionary vs = html.ViewContext.RouteData.Values;
            if (RecordCount > 0)
            {
                int pageMaxSum = 3; //分页栏当前页按钮或左或右最多显示的按钮
                if (curPage < 1) curPage = 1;
                if (curPage > pageSum) curPage = pageSum;
                sb.AppendLine("<div class='jiamu-bottom'><ul class='tunepage'>");
                //上一页
                if (1 != curPage)
                {
                    string newurl = "";
                    if (pagenum == -1)
                        newurl = fullurl + "&page=" + (curPage - 1).ToString();
                    else
                        newurl = fullurl.Substring(0, pagenum) + "page=" + (curPage - 1).ToString();
                    if (valuenum >= 2)
                        sb.Append("<li><a href='" + newurl + "'>" + CultureHelper.GetLangString("ORDER_LIST_PREVIOUSPAGE") + "</a></li>");
                    else
                    {
                        sb.Append("<li>");
                        sb.Append(Html.LinkExtensions.ActionLink(html, CultureHelper.GetLangString("ORDER_LIST_PREVIOUSPAGE"), vs["action"].ToString(), new { page = curPage - 1 }));
                        sb.Append("</li>");
                    }
                }
                var startIndex = 1;
                var endIndex = pageSum;
                if (pageSum > pageMaxSum * 2 + 1)
                {
                    if (curPage - pageMaxSum > 0)
                    {
                        if (curPage + pageMaxSum < pageSum)
                        {
                            startIndex = curPage - pageMaxSum;
                            endIndex = curPage + pageMaxSum;
                        }
                        else
                        {
                            startIndex = pageSum - pageMaxSum * 2;
                        }
                    }
                    else
                    {
                        endIndex = pageMaxSum * 2 + 1;
                    }
                }
                if (pageSum > 1)
                {
                    if (startIndex.Equals(2))
                    {
                        string newurl = "";
                        if (pagenum == -1)
                            newurl = fullurl + "&page=1";
                        else
                            newurl = fullurl.Substring(0, pagenum) + "page=1";
                        if (valuenum >= 2)
                            sb.Append("<li><a href='" + newurl + "'>1</a></li>");
                        else
                        {
                            sb.Append("<li>");
                            sb.Append(Html.LinkExtensions.ActionLink(html, "1", vs["action"].ToString(), new { @page = 1 }, null));
                            sb.Append("</li>");
                        }
                        endIndex--;
                    }
                    else if (startIndex > 2)
                    {
                        string newurl = "";
                        if (pagenum == -1)
                            newurl = fullurl + "&page=1";
                        else
                            newurl = fullurl.Substring(0, pagenum) + "page=1";
                        if (valuenum >= 2)
                            sb.Append("<li><a href='" + newurl + "'>1</a></li>");
                        else
                        {
                            sb.Append("<li>");
                            sb.Append(Html.LinkExtensions.ActionLink(html, "1", vs["action"].ToString(), new { @page = 1 }, null));
                            sb.Append("</li>");
                            sb.AppendLine("<li><a href='javascript:void(0)'>...</a></li>");
                        }
                    }
                    //页码
                    for (var i = startIndex; i <= endIndex; i++)
                    {
                        if (curPage == i)
                        {
                            sb.Append("<li><a href='javascript:;' class=\"current\">" + curPage + "</a></li>");
                        }
                        else
                        {
                            string newurl = "";
                            if (pagenum == -1)
                                newurl = fullurl + "&page=" + i;
                            else
                                newurl = fullurl.Substring(0, pagenum) + "page=" + i;
                            if (valuenum >= 2)
                                sb.Append("<li><a href='" + newurl + "'>" + i + "</a></li>");
                            else
                            {
                                var a = Html.LinkExtensions.ActionLink(html, i.ToString(), vs["action"].ToString(), new { @page = i }, null);
                                sb.Append("<li>");
                                sb.Append(a);
                                sb.Append("</li>");
                            }
                        }
                    }
                    if (endIndex < pageSum - 1)
                    {
                        sb.AppendLine("<li><a href='javascript:void(0)'>...</a></li>");
                        string newurl = "";
                        if (pagenum == -1)
                            newurl = fullurl + "&page=" + pageSum;
                        else
                            newurl = fullurl.Substring(0, pagenum) + "page=" + pageSum;
                        if (valuenum >= 2)
                            sb.Append("<li><a href='" + newurl + "'>" + pageSum + "</a></li>");
                        else
                        {
                            sb.Append("<li>");
                            sb.Append(Html.LinkExtensions.ActionLink(html, pageSum.ToString(), vs["action"].ToString(), new { @page = pageSum }, null));
                            sb.Append("</li>");
                        }
                    }
                    else if (endIndex < pageSum)
                    {
                        string newurl = "";
                        if (pagenum == -1)
                            newurl = fullurl + "&page=" + pageSum;
                        else
                            newurl = fullurl.Substring(0, pagenum) + "page=" + pageSum;
                        if (valuenum >= 2)
                            sb.Append("<li><a href='" + newurl + "'>" + pageSum + "</a></li>");
                        else
                        {
                            sb.Append("<li>");
                            sb.Append(Html.LinkExtensions.ActionLink(html, pageSum.ToString(), vs["action"].ToString(), new { @page = pageSum }, null));
                            sb.Append("</li>");
                        }
                    }
                }
                //下一页
                if (curPage != pageSum && pageSum > 1)
                {
                    string newurl = "";
                    if (pagenum == -1)
                        newurl = fullurl + "&page=" + (curPage + 1).ToString();
                    else
                        newurl = fullurl.Substring(0, pagenum) + "page=" + (curPage + 1).ToString();
                    if (valuenum >= 2)
                        sb.Append("<li><a href='" + newurl + "'>" + CultureHelper.GetLangString("ORDER_LIST_NEXTPAGE") + "</a></li>");
                    else
                    {
                        sb.Append("<li>");
                        sb.Append(Html.LinkExtensions.ActionLink(html, CultureHelper.GetLangString("ORDER_LIST_NEXTPAGE"), vs["action"].ToString(), new { @page = curPage + 1 }));
                        sb.Append("</li>");
                    }
                }
                sb.AppendLine("</ul></div>");
            }
            return sb.ToString();
        }


        public static string DDAmountFormat(this decimal amount)
        {
            //只取两位,不做四舍五入
            decimal ramount = (decimal)((long)(amount * 100)) / 100;
            if (ramount == 0)
            {
                return "0.00";
            }
            return ramount.ToString("#,##0.00");
        }





        /// <summary>
        /// 账号系统获取用户头像
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="ImageUrl">图片地址</param>
        /// <param name="ImageSize"></param>
        /// <param name="height"></param>
        /// <param name="hType">用户类型 0:会员 1:商家（待定）</param>
        /// <param name="istrue">是否启用账号系统</param>
        /// <returns></returns>
        public static string GetAvatarImagesUrl(this HtmlHelper helper, string ImageUrl, int ImageSize, int height = 0, int hType = 0, bool istrue = false)
        {
            string str = "";
            if (height == 0)
            {
                str = HKTHMall.Core.Extensions.HtmlExtensions.GetThumbsImage(ImageUrl, ImageSize);
            }
            else
            {
                str = HKTHMall.Core.Extensions.HtmlExtensions.GetThumbsImage(ImageUrl, ImageSize, height);
            }
            if (string.IsNullOrWhiteSpace(str))
            {
                if (hType == 0)
                {
                    return "/Content/newcss/css/images/default.png";
                    //return "/images/touxiang.png";
                }
                else if (hType == 1)
                {
                    return "/images/load.jpg";
                }
                return "/images/Head.jpg";
            }
            if (istrue)
            {
                return  HKTHMall.Core.Extensions.HtmlExtensions.GetThumbsImage(ImageUrl, ImageSize, height);
            }
            else
            {
                return GetConfig.FullPath() + HKTHMall.Core.Extensions.HtmlExtensions.GetThumbsImage(ImageUrl, ImageSize, height);
            }

        }

        public static MvcHtmlString ToPagerOld(this HtmlHelper html,string currentPageStr, int pageSize, int totalCount)
        {
            var queryString = html.ViewContext.HttpContext.Request.QueryString;
            int currentPage = 1; //当前页  
            var totalPages = Math.Max((totalCount + pageSize - 1) / pageSize, 1); //总页数  
            var dict = new System.Web.Routing.RouteValueDictionary(html.ViewContext.RouteData.Values);
            var output = new StringBuilder();
            if (!string.IsNullOrEmpty(queryString[currentPageStr]))
            {
                //与相应的QueryString绑定 
                foreach (string key in queryString.Keys)
                    if (queryString[key] != null && !string.IsNullOrEmpty(key))
                        dict[key] = queryString[key];
                int.TryParse(queryString[currentPageStr], out currentPage);
            }
            else
            {
                //获取 ～/Page/{page number} 的页号参数
                if (dict.ContainsKey(currentPageStr))
                    int.TryParse(dict[currentPageStr].ToString(), out currentPage);
            }

            //保留查询字符到下一页
            foreach (string key in queryString.Keys)
                dict[key] = queryString[key];

            output.Append("<ul>");
            if (currentPage <= 0) currentPage = 1;
            if (totalPages > 1)
            {
                if (currentPage > 1)
                {
                    //处理上一页的连接  
                    dict[currentPageStr] = currentPage - 1;
                    output.Append("<li>");
                    output.Append(html.RouteLink("<", dict));
                    output.Append("</li>");
                }
                else
                {
                    output.Append("<li><</li>");
                }
                output.Append(" ");
                output.AppendFormat("  <li><a href=\"javascript:;\" style=\"color: #696969;\"><span class=\"icon5\">{0}</span>/{1}</a> </li>", currentPage, totalPages);
                if (currentPage < totalPages)
                {
                    //处理下一页的链接 
                    dict[currentPageStr] = currentPage + 1;
                    output.Append("<li>");
                    output.Append(html.RouteLink(">", dict));
                    output.Append("</li>");
                }
                else
                {
                    output.Append("<li>></li>");
                }
              
            }
            else if (totalPages == 1 && totalCount > 0)
            {
                output.Append("<li><</li>");
                output.AppendFormat("<li><a href=\"javascript:;\" style=\"color: #696969;\"><span class=\"icon5\">{0}</span>/{1}</a></li>", currentPage, totalPages);
                output.Append("<li>></li>");
            }
            output.Append("</ul>");
            return new MvcHtmlString(output.ToString());
        }
  
        public static MvcHtmlString ToPager(this HtmlHelper html, string currentPageStr, int pageSize, int totalCount)
        {
            StringBuilder url = new StringBuilder();
            int currentPage = 1; //当前页  
            var totalPages = Math.Max((totalCount + pageSize - 1) / pageSize, 1); //总页数  
            string path = HttpContext.Current.Request.Url.AbsolutePath + "?";
           // string path = HttpContext.Current.Request.Url.AbsolutePath + "?page={0}";
            // url.Append(HttpContext.Current.Request.Url.AbsolutePath);
            NameValueCollection collection = HttpContext.Current.Request.QueryString;
            string[] keys = collection.AllKeys;
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i]!=null && keys[i].ToLower() != "page")
                {
                    url.AppendFormat("&{0}={1}", keys[i], collection[keys[i]]);
                }
                else
                {
                    currentPage = int.Parse(collection[keys[i]]);
                }
            }
            if (keys.Length != 0)
            {
                path += url.ToString().Substring(1);
            }
            else
            {
                path += url.ToString();
            }           
            var output = new StringBuilder();
            if (currentPage <= 0) currentPage = 1;
            if (totalPages > 1)
            {
                if (currentPage > 1)
                {
                    //处理上一页的连接  
                    path = path + "&page={0}";
                    string pre = string.Format(path, currentPage - 1);
                    output.AppendFormat("<a href={0} class='prev_092 used'><</a>", pre);
                }
                else
                {
                    output.Append("<a href='' class='prev_092 unused'><</a>");
                }
                output.AppendFormat("<span class='page_c092'>{0}/{1}</span>", currentPage, totalPages);
                if (currentPage < totalPages)
                {
                    //处理下一页的链接 
                    path = path + "&page={0}";
                    string aft = string.Format(path, currentPage + 1);
                    output.AppendFormat("<a href={0} class='next_092 used'>></a>", aft);
                }
                else
                {
                    output.Append("<a href='' class='next_092 unused'>></a>");
                }
            }
            else if (totalPages == 1 && totalCount > 0)
            {
                output.Append("<a href='' class='prev_092 unused'><</a>");
                output.AppendFormat("<span class='page_c092'>{0}/{1}</span>", currentPage, totalPages);
                output.Append("<a href='' class='next_092 unused'>></a>");
            }
            return new MvcHtmlString(output.ToString());  
        }

      
        public static object GetPropertyValue(this object info, string field)
        {
            if (info == null) return null;
            Type t = info.GetType();
            IEnumerable<System.Reflection.PropertyInfo> property = from pi in t.GetProperties() where pi.Name.ToLower() == field.ToLower() select pi;
            return property.First().GetValue(info, null);
        }

        //刘文宁 20151016
        public static string GetPageHtmlForMenber(int pagedIndex, int pagedSize, int totalCount, string functionName)
        {
            int PagesLen = (int)Math.Ceiling((double)totalCount / (double)pagedSize); //总页数
            int PageNum = 8;  //分页链接接数
            int startPage, endPage;
            int PageNum_Front = PageNum % 2 == 0 ? (int)Math.Ceiling((double)PageNum / 2) + 1 : (int)Math.Ceiling((double)PageNum / 2);
            int PageNum_Behind = PageNum % 2 == 0 ? (int)Math.Ceiling((double)PageNum / 2) : (int)Math.Ceiling((double)PageNum / 2) + 1;
            if (PageNum >= PagesLen)
            {
                startPage = 0;
                endPage = PagesLen - 1;
            }
            else if (pagedIndex < PageNum_Front)
            {
                startPage = 0;
                endPage = PagesLen - 1 > PageNum ? PageNum : PagesLen - 1;
            }
            else
            {
                startPage = pagedIndex + PageNum_Behind >= PagesLen ? PagesLen - PageNum - 1 : pagedIndex - PageNum_Front + 1;
                var t = startPage + PageNum;
                endPage = t > PagesLen ? PagesLen - 1 : t;
            }
            //var html = "<span " + (pagedIndex == 1 ? "" : "onclick=\"" + functionName + "(1)\"") + ">" + CultureHelper.GetLangString("WGD_BUSIHOME_FIRSTPAGE") + "</span>";//第一页
            var html = "<li class='secPrevPage'> <a " + (pagedIndex == 1 ? "" : "onclick=\"" + functionName + "(" + (pagedIndex - 1) + ")\"") + "><i class='fa-angle-left'></i>" + CultureHelper.GetLangString("ORDER_LIST_PREVIOUSPAGE") + "</a></li>";//上一页
            for (var i = startPage; i <= endPage; i++)
            {
                html += "<li><a class=\"" + (pagedIndex == (i + 1) ? "ly_paging_n" : "") + "\" " + (pagedIndex == (i + 1) ? "" : "onclick=\"" + functionName + "(" + (i + 1) + ")\"") + ">" + (i + 1) + "</a></li>";
            }
            if (PagesLen > (endPage + 1))
            {
                html += "<li><a>...</a></li>";
            }
            html += "<li class='secNextPage'> <a " + (pagedIndex == PagesLen ? "" : "onclick=\"" + functionName + "(" + (pagedIndex + 1) + ")\"") + ">" + CultureHelper.GetLangString("ORDER_LIST_NEXTPAGE") + "<i class='fa-angle-right'></i></a></li>";//下一页
            //html += "<span " + (pagedIndex == PagesLen ? "" : "onclick=\"" + functionName + "(" + PagesLen + ")\"") + ">" + CultureHelper.GetLangString("WGD_BUSIHOME_LASTPAGE") + "</span>";//最后页
            return html;
        }
    }
}