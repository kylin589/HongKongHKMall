using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class bannerMapping : EntityTypeConfiguration<banner>
	{
		public bannerMapping()
		{
		    this.ToTable("banner");
   			this.HasKey(m => m.bannerId).Property(m=>m.bannerId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		