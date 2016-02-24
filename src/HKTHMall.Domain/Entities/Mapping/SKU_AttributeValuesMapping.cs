using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class SKU_AttributeValuesMapping : EntityTypeConfiguration<SKU_AttributeValues>
	{
		public SKU_AttributeValuesMapping()
		{
		    this.ToTable("SKU_AttributeValues");
   			this.HasKey(m => m.ValueId).Property(m=>m.ValueId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		