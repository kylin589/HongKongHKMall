using HKTHMall.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.Bra
{
    public class Brand_LangModel
    {
        public int Id { get; set; }
        public int BrandID { get; set; }
        public string BrandName { get; set; }
        public int LanguageID { get; set; }

        public virtual Brand Brand { get; set; }
    }
}
