using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class ProductMapping : EntityTypeConfiguration<Product>
	{
		public ProductMapping()
		{
		    this.ToTable("Product");
   			this.HasKey(m => m.ProductId).Property(m=>m.ProductId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		