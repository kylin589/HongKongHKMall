using System;

namespace BrCms.Framework.Data
{
    /// <summary>
    /// 数据库选择
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DbContextTypeAttribute : Attribute
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        public DbContextTypeAttribute(byte dbType)
        {
            DbType =  dbType;
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public byte DbType { get; set; }
    } 
}
