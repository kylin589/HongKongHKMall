using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class FloorKeywordMapping : EntityTypeConfiguration<FloorKeyword>
	{
		public FloorKeywordMapping()
		{
		    this.ToTable("FloorKeyword");
   			this.HasKey(m => m.FloorKeywordId).Property(m=>m.FloorKeywordId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		