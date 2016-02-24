using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class UserAddressMapping : EntityTypeConfiguration<UserAddress>
	{
		public UserAddressMapping()
		{
		    this.ToTable("UserAddress");
   			this.HasKey(m => m.UserAddressId).Property(m=>m.UserAddressId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		