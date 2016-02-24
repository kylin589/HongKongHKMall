using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class ReportMapping : EntityTypeConfiguration<Report>
	{
		public ReportMapping()
		{
		    this.ToTable("Report");
   			this.HasKey(m => m.ReportId).Property(m=>m.ReportId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		