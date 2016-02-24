using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class YH_UserLoginLogMapping : EntityTypeConfiguration<YH_UserLoginLog>
	{
		public YH_UserLoginLogMapping()
		{
		    this.ToTable("YH_UserLoginLog");
   			this.HasKey(m => m.ID).Property(m=>m.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		