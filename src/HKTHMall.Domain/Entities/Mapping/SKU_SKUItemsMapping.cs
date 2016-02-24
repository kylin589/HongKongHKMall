using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class SKU_SKUItemsMapping : EntityTypeConfiguration<SKU_SKUItems>
	{
		public SKU_SKUItemsMapping()
		{
		    this.ToTable("SKU_SKUItems");
   			this.HasKey(m => m.SKU_SKUItemsId).Property(m=>m.SKU_SKUItemsId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		