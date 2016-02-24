using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class FloorConfigMapping : EntityTypeConfiguration<FloorConfig>
	{
		public FloorConfigMapping()
		{
		    this.ToTable("FloorConfig");
   			this.HasKey(m => m.FloorConfigId).Property(m=>m.FloorConfigId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		