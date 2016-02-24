using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class SKU_ProductTypesMapping : EntityTypeConfiguration<SKU_ProductTypes>
	{
		public SKU_ProductTypesMapping()
		{
		    this.ToTable("SKU_ProductTypes");
   			this.HasKey(m => m.SkuTypeId).Property(m=>m.SkuTypeId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		