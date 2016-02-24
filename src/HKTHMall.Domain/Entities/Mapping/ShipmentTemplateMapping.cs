using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class ShipmentTemplateMapping : EntityTypeConfiguration<ShipmentTemplate>
	{
		public ShipmentTemplateMapping()
		{
		    this.ToTable("ShipmentTemplate");
   			this.HasKey(m => m.ShipmentTemplateId).Property(m=>m.ShipmentTemplateId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		