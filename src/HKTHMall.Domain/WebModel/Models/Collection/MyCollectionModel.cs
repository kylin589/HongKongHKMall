using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Collection
{
    [DataContract]
    public class MyCollectionModel
    {
        [DataMember]
        public long FavoritesID { get; set; }
        [DataMember]
        public long UserID { get; set; }
        [DataMember]
        public long ProductId { get; set; }
        [DataMember]
        public decimal HKPrice { get; set; }
        [DataMember]
        public decimal MarketPrice { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string PicUrl { get; set; }
        [DataMember]
        public string Flag { get; set; }
        [DataMember]
        public string FavoritesDate { get; set; }
        public decimal Discount { get; set; }
        public DateTime? StarDate { get; set; }
        public DateTime? EndDate { get; set; }
        [DataMember]
        public decimal activityPrice { get; set; }
        [DataMember]
        public bool isActivity { get; set; }
    }
}
