using FluentValidation.Attributes;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Validators.AC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.AC
{
    /// <summary>
    /// 系统角色表
    /// <remarks>added by jimmy,2015-7-6</remarks>
    /// </summary>
    [Validator(typeof(AC_RoleValidator))]
    public class AC_RoleModel
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleModuleValue { get; set; }
        public string RoleFuctionValue { get; set; }
        public string RoleDescription { get; set; }
        public string CreateUser { get; set; }
        public System.DateTime CreateDT { get; set; }
        public string UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateDt { get; set; }

        public virtual ICollection<AC_User> AC_User { get; set; }
    }
}
