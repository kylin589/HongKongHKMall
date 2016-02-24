﻿using HKTHMall.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models
{
    public class SearchYH_UserModel
    {
        public SearchYH_UserModel()
        {
            this.Favorites = new HashSet<Favorites>();
            this.ShoppingCart = new HashSet<ShoppingCart>();
            this.UserAddress = new HashSet<UserAddress>();
            this.YH_Agent = new HashSet<YH_Agent>();
            this.YH_UserBankAccount = new HashSet<YH_UserBankAccount>();
            this.ZJ_WithdrawOrder = new HashSet<ZJ_WithdrawOrder>();
        }
    
        public long UserID { get; set; }
        public string Account { get; set; }
        public string PassWord { get; set; }
        public string PayPassWord { get; set; }
        public string NickName { get; set; }
        public string HeadImageUrl { get; set; }
        public Nullable<byte> Sex { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<System.DateTime> RegisterDate { get; set; }
        public Nullable<byte> RegisterSource { get; set; }
        public Nullable<byte> ActivePhone { get; set; }
        public Nullable<byte> ActiveEmail { get; set; }
        public Nullable<int> RecommendCode { get; set; }
        public long ReferrerID { get; set; }
        public long ParentID2 { get; set; }
        public long ParentID3 { get; set; }
        public string ParentIDs { get; set; }
        public int Level { get; set; }
        public byte UserType { get; set; }
        public byte IsLock { get; set; }
        public byte IsDelete { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }
        public Nullable<int> THAreaID { get; set; }
        public string DetailsAddress { get; set; }
        public string signature { get; set; }
        public string OrcodeUrl { get; set; }

        public int RelativelyDistributionLevel { get; set; }

        public decimal Earnings { get; set; }
    
        public virtual ICollection<Favorites> Favorites { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCart { get; set; }
        public virtual ICollection<UserAddress> UserAddress { get; set; }
        public virtual ICollection<YH_Agent> YH_Agent { get; set; }
        public virtual ICollection<YH_UserBankAccount> YH_UserBankAccount { get; set; }
        public virtual ICollection<ZJ_WithdrawOrder> ZJ_WithdrawOrder { get; set; }
    }
}
