using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class FeedbackType_langMapping : EntityTypeConfiguration<FeedbackType_lang>
	{
		public FeedbackType_langMapping()
		{
		    this.ToTable("FeedbackType_lang");
   			this.HasKey(m => m.FeedbackType_langId).Property(m=>m.FeedbackType_langId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		