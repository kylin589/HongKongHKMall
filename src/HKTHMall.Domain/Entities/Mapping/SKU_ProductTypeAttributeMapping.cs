using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class SKU_ProductTypeAttributeMapping : EntityTypeConfiguration<SKU_ProductTypeAttribute>
	{
		public SKU_ProductTypeAttributeMapping()
		{
		    this.ToTable("SKU_ProductTypeAttribute");
   			this.HasKey(m => m.SKU_ProductTypeAttributeId).Property(m=>m.SKU_ProductTypeAttributeId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		