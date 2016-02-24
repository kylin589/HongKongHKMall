using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BrCms.Framework.Mvc.Extensions
{
    public static class MvcExtension
    {
        /// <summary>
        /// ToSelectList
        /// </summary>
        /// <typeparam name="TEnum">TEnum</typeparam>
        /// <param name="enumObj">enumObj</param>
        /// <param name="markCurrentAsSelected">markCurrentAsSelected</param>
        /// <param name="valuesToExclude">valuesToExclude</param>
        /// <param name="selected"></param>
        /// <returns>SelectList</returns>
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj, bool markCurrentAsSelected = true, int[] valuesToExclude = null, object selected = null) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("An Enumeration type is required.", "enumObj");
            }

            var values = from TEnum enumValue in Enum.GetValues(typeof(TEnum))
                         where (valuesToExclude == null || !valuesToExclude.Contains(Convert.ToInt32(enumValue)))
                         && EnumDescription.IsAttrbute(enumValue)
                         select new
                         {
                             ID = Convert.ToInt32(enumValue),
                             Name = EnumDescription.GetFieldText(enumValue)
                         };

            object selectedValue = selected;

            if (markCurrentAsSelected && selectedValue != null)
            {
                selectedValue = Convert.ToInt32(enumObj);
            }

            return new SelectList(values, "ID", "Name", selectedValue);
        }

        /// <summary>
        /// ToSelectList
        /// </summary>
        /// <typeparam name="TEnum">TEnum</typeparam>
        /// <param name="markCurrentAsSelected">markCurrentAsSelected</param>
        /// <param name="valuesToExclude">valuesToExclude</param>
        /// <param name="selected"></param>
        /// <returns>SelectList</returns>
        public static SelectList ToSelectList<TEnum>(bool markCurrentAsSelected = true, int[] valuesToExclude = null, object selected = null) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("An Enumeration type is required.", "enumObj");
            }

            var values = from TEnum enumValue in Enum.GetValues(typeof(TEnum))
                         where (valuesToExclude == null || !valuesToExclude.Contains(Convert.ToInt32(enumValue)))
                         && EnumDescription.IsAttrbute(enumValue)
                         select new
                         {
                             ID = Convert.ToInt32(enumValue),
                             Name = EnumDescription.GetFieldText(enumValue)
                         };

         
            return new SelectList(values, "ID", "Name");
        }


        /// <summary>
        /// ToSelectList
        /// </summary>
        /// <typeparam name="TEnum">TEnum</typeparam>
        /// <param name="enumObj">enumObj</param>
        /// <param name="markCurrentAsSelected">markCurrentAsSelected</param>
        /// <param name="valuesToExclude">valuesToExclude</param>
        /// <returns>SelectList</returns>
        public static List<SelectListItem> ToSelectListItems<TEnum>(this TEnum enumObj, bool markCurrentAsSelected = true, int[] valuesToExclude = null) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("An Enumeration type is required.", "enumObj");
            }

            var values = from TEnum enumValue in Enum.GetValues(typeof(TEnum))
                         where (valuesToExclude == null || !valuesToExclude.Contains(Convert.ToInt32(enumValue)))
                         && EnumDescription.IsAttrbute(enumValue)
                         select new SelectListItem()
                         {
                             Selected = markCurrentAsSelected && Convert.ToInt32(enumValue) == Convert.ToInt32(enumObj),
                             Value = Convert.ToInt32(enumValue).ToString(),
                             Text = EnumDescription.GetFieldText(enumValue)
                         };



            return values.ToList();
        }


        /// <summary>
        /// ToSelectList
        /// </summary>
        /// <typeparam name="TEnum">TEnum</typeparam>
        /// <param name="markCurrentAsSelected">markCurrentAsSelected</param>
        /// <param name="valuesToExclude">valuesToExclude</param>
        /// <returns>SelectList</returns>
        public static List<SelectListItem> ToSelectListItems<TEnum>(bool markCurrentAsSelected = true, int[] valuesToExclude = null) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("An Enumeration type is required.", "enumObj");
            }

            var values = from TEnum enumValue in Enum.GetValues(typeof(TEnum))
                         where (valuesToExclude == null || !valuesToExclude.Contains(Convert.ToInt32(enumValue)))
                         && EnumDescription.IsAttrbute(enumValue)
                         select new SelectListItem()
                         {
                             Value = Convert.ToInt32(enumValue).ToString(),
                             Text = EnumDescription.GetFieldText(enumValue)
                         };



            return values.ToList();
        }
    }
}
