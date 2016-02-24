using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class RecommendBonusRecordMapping : EntityTypeConfiguration<RecommendBonusRecord>
	{
		public RecommendBonusRecordMapping()
		{
		    this.ToTable("RecommendBonusRecord");
   			this.HasKey(m => m.RecommendBonusRecordId).Property(m=>m.RecommendBonusRecordId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		