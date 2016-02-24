using System;
namespace HKTMall.Web
{
    /// <summary>
    /// AJAX方法 访问权限
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class AjaxAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aliasActionNames"> 是否只允许AJAX访问 0只能AJAX访问 1 ajax和地址栏访问都行</param>
        public AjaxAttribute(bool aliasActionNames=true)
        {
            Type = aliasActionNames;
        }
        /// <summary>
        /// 是否只允许AJAX访问 true 只能AJAX访问 falseajax和地址栏访问都行
        /// </summary>
        public bool Type
        {
            get;
            set;
        }
    }
}