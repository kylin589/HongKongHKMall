using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class SKU_ProductMapping : EntityTypeConfiguration<SKU_Product>
	{
		public SKU_ProductMapping()
		{
		    this.ToTable("SKU_Product");
   			this.HasKey(m => m.SKU_ProducId).Property(m=>m.SKU_ProducId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		