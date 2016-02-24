using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class ZJ_RebateMapping : EntityTypeConfiguration<ZJ_Rebate>
	{
		public ZJ_RebateMapping()
		{
		    this.ToTable("ZJ_Rebate");
   			this.HasKey(m => m.ID).Property(m=>m.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		