using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class ReportType_langMapping : EntityTypeConfiguration<ReportType_lang>
	{
		public ReportType_langMapping()
		{
		    this.ToTable("ReportType_lang");
   			this.HasKey(m => m.ReportType_langId).Property(m=>m.ReportType_langId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		