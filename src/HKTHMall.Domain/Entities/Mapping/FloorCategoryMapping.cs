using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class FloorCategoryMapping : EntityTypeConfiguration<FloorCategory>
	{
		public FloorCategoryMapping()
		{
		    this.ToTable("FloorCategory");
   			this.HasKey(m => m.FloorCategoryId).Property(m=>m.FloorCategoryId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		