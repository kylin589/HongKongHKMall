using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class YH_UserUpdateInfoMapping : EntityTypeConfiguration<YH_UserUpdateInfo>
	{
		public YH_UserUpdateInfoMapping()
		{
		    this.ToTable("YH_UserUpdateInfo");
   			this.HasKey(m => m.UserID).Property(m=>m.UserID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		