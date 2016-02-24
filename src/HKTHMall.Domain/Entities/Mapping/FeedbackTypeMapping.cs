using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class FeedbackTypeMapping : EntityTypeConfiguration<FeedbackType>
	{
		public FeedbackTypeMapping()
		{
		    this.ToTable("FeedbackType");
   			this.HasKey(m => m.FeedbackTypeId).Property(m=>m.FeedbackTypeId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		