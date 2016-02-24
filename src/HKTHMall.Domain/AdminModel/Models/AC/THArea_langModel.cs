using HKTHMall.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.AC
{
    public class THArea_langModel
    {
        public int Id { get; set; }
        public int THAreaID { get; set; }
        public string AreaName { get; set; }
        public int LanguageID { get; set; }

        public int AreaType { get; set; }

        public string ShortName { get; set; }

        public virtual THArea THArea { get; set; }
    }
}
