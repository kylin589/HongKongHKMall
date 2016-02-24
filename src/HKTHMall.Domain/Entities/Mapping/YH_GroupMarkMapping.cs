using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class YH_GroupMarkMapping : EntityTypeConfiguration<YH_GroupMark>
	{
		public YH_GroupMarkMapping()
		{
		    this.ToTable("YH_GroupMark");
   			this.HasKey(m => m.UserID).Property(m=>m.UserID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		