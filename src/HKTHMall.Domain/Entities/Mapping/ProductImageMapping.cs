using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class ProductImageMapping : EntityTypeConfiguration<ProductImage>
	{
		public ProductImageMapping()
		{
		    this.ToTable("ProductImage");
   			this.HasKey(m => m.ProductImageId).Property(m=>m.ProductImageId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		