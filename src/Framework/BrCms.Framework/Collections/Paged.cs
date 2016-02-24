using System.Runtime.Serialization;
namespace BrCms.Framework.Collections
{
    [DataContract]
    public class Paged
    {
        /// <summary>
        /// 页码
        /// </summary>
        [DataMember]
        public int PagedIndex { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        [DataMember]
        public int PagedSize { get; set; }
    }
}
