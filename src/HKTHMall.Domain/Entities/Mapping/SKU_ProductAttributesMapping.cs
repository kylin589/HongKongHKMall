using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class SKU_ProductAttributesMapping : EntityTypeConfiguration<SKU_ProductAttributes>
	{
		public SKU_ProductAttributesMapping()
		{
		    this.ToTable("SKU_ProductAttributes");
   			this.HasKey(m => m.SKU_ProductAttributesId).Property(m=>m.SKU_ProductAttributesId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		