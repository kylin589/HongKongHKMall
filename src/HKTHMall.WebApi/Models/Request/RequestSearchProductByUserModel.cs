using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models
{
    public class RequestSearchProductByUserModel : RequestSearchByUserModel
    {
        public long productId { get; set; }
    }

    public class RequestFavoritesByUserModel : RequestSearchByUserModel
    {
        public long favoritesID { get; set; }
    }
}