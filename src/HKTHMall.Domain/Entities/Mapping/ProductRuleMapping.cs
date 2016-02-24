using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class ProductRuleMapping : EntityTypeConfiguration<ProductRule>
	{
		public ProductRuleMapping()
		{
		    this.ToTable("ProductRule");
   			this.HasKey(m => m.ProductRuleId).Property(m=>m.ProductRuleId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		