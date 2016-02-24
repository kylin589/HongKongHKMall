using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class ProductConsultMapping : EntityTypeConfiguration<ProductConsult>
	{
		public ProductConsultMapping()
		{
		    this.ToTable("ProductConsult");
   			this.HasKey(m => m.ProductConsultId).Property(m=>m.ProductConsultId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		