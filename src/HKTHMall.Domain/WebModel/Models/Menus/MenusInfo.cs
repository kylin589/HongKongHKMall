using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Menus
{
    /// <summary>
    /// 菜单详细内容
    /// </summary>
    public class MenusInfo
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenusName { get; set; }
        /// <summary>
        /// 菜单Id
        /// </summary>
        public int MenusID { get; set; }
        /// <summary>
        /// 菜单父id
        /// </summary>
        public int PMenusID { get; set; }
        /// <summary>
        /// 菜单图片显示目录
        /// </summary>
        public string MenusImg { get; set; }
    }
}
