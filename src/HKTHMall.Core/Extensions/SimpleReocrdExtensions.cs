using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HKTHMall.Core.Extensions
{
    /// <summary>
    /// SimpleRecord扩展方法
    /// </summary>
    public static class SimpleReocrdExtensions
    {
        /// <summary>
        /// 将SimpleRecord集合转换为实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="records">SimpleRecord集合</param>
        /// <returns>实体集合</returns>
        public static List<T> ToEntity<T>(this IEnumerable<dynamic> records) where T : new()
        {
            /*
             * const string sql = @"SELECT * FROM dbo.SKU_Attributes";
             * Database db = Database.Open();
             * var data = db.ToResultSets(sql);
             * var rows = data.ElementAt(0);
             * var entities = rows.ToEntity<SKU_AttributesModel>();
             *
             */
            List<T> models = new List<T>();
            foreach (var record in records)
            {
                T model = record;
                models.Add(model);
            }
            return models;
        }

        /// <summary>
        /// 枚举转list公共方法
        /// zhoub 20150715
        /// </summary>
        /// <param name="valueEnum"></param>
        /// <returns></returns>
        public static List<SelectListItem> ToSelectListItem(Enum valueEnum)
        {
            List<SelectListItem> list = (from int value in Enum.GetValues(valueEnum.GetType())
                                         select new SelectListItem
                                         {
                                             Text = Enum.GetName(valueEnum.GetType(), value),
                                             Value = value.ToString()
                                         }).ToList();

            SelectListItem item = new SelectListItem();
            item.Text = "-请选择-";
            item.Value = "-1";
            list.Insert(0, item);

            return list;
        }
    }
}
