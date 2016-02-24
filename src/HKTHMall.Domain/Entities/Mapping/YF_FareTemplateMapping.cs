using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class YF_FareTemplateMapping : EntityTypeConfiguration<YF_FareTemplate>
	{
		public YF_FareTemplateMapping()
		{
		    this.ToTable("YF_FareTemplate");
   			this.HasKey(m => m.FareTemplateID).Property(m=>m.FareTemplateID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		