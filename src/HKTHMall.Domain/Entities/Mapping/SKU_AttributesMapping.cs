using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class SKU_AttributesMapping : EntityTypeConfiguration<SKU_Attributes>
	{
		public SKU_AttributesMapping()
		{
		    this.ToTable("SKU_Attributes");
   			this.HasKey(m => m.AttributeId).Property(m=>m.AttributeId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		