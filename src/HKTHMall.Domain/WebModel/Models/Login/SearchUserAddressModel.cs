using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Login
{
    public class SearchUserAddressModel : Paged
    {
        public long UserID { get; set; }

        public long? THAreaID { get; set; }
    }
}
