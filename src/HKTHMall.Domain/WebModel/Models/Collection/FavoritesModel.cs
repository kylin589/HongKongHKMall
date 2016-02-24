using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using HKTHMall.Domain.WebModel.Validators.Collection;

namespace HKTHMall.Domain.WebModel.Models.Collection
{
    [Validator(typeof(FavoritesValidator))]
    public class FavoritesModel
    {
        public long FavoritesID { get; set; }
        public long UserID { get; set; }
        public long ProductId { get; set; }
        public System.DateTime FavoritesDate { get; set; }
    }
}
