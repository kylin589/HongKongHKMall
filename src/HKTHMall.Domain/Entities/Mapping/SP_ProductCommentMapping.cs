using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class SP_ProductCommentMapping : EntityTypeConfiguration<SP_ProductComment>
	{
		public SP_ProductCommentMapping()
		{
		    this.ToTable("SP_ProductComment");
   			this.HasKey(m => m.ProductCommentId).Property(m=>m.ProductCommentId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		