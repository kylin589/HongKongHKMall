using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class YF_FareTemplateAreaGroupMapping : EntityTypeConfiguration<YF_FareTemplateAreaGroup>
	{
		public YF_FareTemplateAreaGroupMapping()
		{
		    this.ToTable("YF_FareTemplateAreaGroup");
   			this.HasKey(m => m.FareTemplateAreaGroupID).Property(m=>m.FareTemplateAreaGroupID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		