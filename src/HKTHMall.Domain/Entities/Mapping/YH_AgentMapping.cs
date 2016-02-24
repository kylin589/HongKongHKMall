using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class YH_AgentMapping : EntityTypeConfiguration<YH_Agent>
	{
		public YH_AgentMapping()
		{
		    this.ToTable("YH_Agent");
   			this.HasKey(m => m.AgentID).Property(m=>m.AgentID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		