using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class ReportTypeMapping : EntityTypeConfiguration<ReportType>
	{
		public ReportTypeMapping()
		{
		    this.ToTable("ReportType");
   			this.HasKey(m => m.ReportTypeId).Property(m=>m.ReportTypeId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		