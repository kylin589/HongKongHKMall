using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class SalesRuleMapping : EntityTypeConfiguration<SalesRule>
	{
		public SalesRuleMapping()
		{
		    this.ToTable("SalesRule");
   			this.HasKey(m => m.SalesRuleId).Property(m=>m.SalesRuleId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		