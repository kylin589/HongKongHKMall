using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class ProductPicMapping : EntityTypeConfiguration<ProductPic>
	{
		public ProductPicMapping()
		{
		    this.ToTable("ProductPic");
   			this.HasKey(m => m.ProductPicId).Property(m=>m.ProductPicId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		