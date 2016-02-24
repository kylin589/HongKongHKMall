using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class YH_UserMapping : EntityTypeConfiguration<YH_User>
	{
		public YH_UserMapping()
		{
		    this.ToTable("YH_User");
   			this.HasKey(m => m.UserID).Property(m=>m.UserID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		