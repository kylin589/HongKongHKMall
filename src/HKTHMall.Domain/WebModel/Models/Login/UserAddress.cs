using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Login
{
    [DataContract]
    public class UserAddress
    {
        [DataMember(Name = "userAddressId")]
        public long UserAddressId { get; set; }

        [DataMember(Name = "userID")]
        public long UserID { get; set; }

        [DataMember(Name = "receiver")]
        public string Receiver { get; set; }
        [DataMember(Name = "tHAreaID")]
        public long THAreaID { get; set; }

        [DataMember(Name = "detailsAddress")]
        public string DetailsAddress { get; set; }
        [DataMember(Name = "postalCode")]
        public string PostalCode { get; set; }
        [DataMember(Name = "mobile")]
        public string Mobile { get; set; }
        [DataMember(Name = "phone")]
        public string Phone { get; set; }
        [DataMember(Name = "flag")]
        public int Flag { get; set; }
        [DataMember(Name = "email")]
        public string Email { get; set; }
        [DataMember(Name = "countryTHAreaID")]
        public int CountryTHAreaID { get; set; }
        [DataMember(Name = "shengTHAreaID")]
        public int ShengTHAreaID { get; set; }
        [DataMember(Name = "shiTHAreaID")]

        public int ShiTHAreaID { get; set; }
        [DataMember(Name = "countryTHAreaName")]
        public string CountryTHAreaName{ get; set; }
        [DataMember(Name = "shengAreaName")]
        public string ShengAreaName { get; set; }
        [DataMember(Name = "shiAreaName")]
        public string ShiAreaName { get; set; }
        [DataMember(Name = "quAreaName")]
        public string QuAreaName { get; set; }


    }



    public partial class THAreaInfo
    {
        public int THAreaID { get; set; }
        public string AreaName { get; set; }
        public int LanguageID { get; set; }
        public int ParentID { get; set; }

    }

}
