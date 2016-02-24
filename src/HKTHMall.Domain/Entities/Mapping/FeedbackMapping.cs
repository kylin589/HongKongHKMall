using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class FeedbackMapping : EntityTypeConfiguration<Feedback>
	{
		public FeedbackMapping()
		{
		    this.ToTable("Feedback");
   			this.HasKey(m => m.FeedbackId).Property(m=>m.FeedbackId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		