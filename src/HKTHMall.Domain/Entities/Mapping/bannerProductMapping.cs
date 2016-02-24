using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class bannerProductMapping : EntityTypeConfiguration<bannerProduct>
	{
		public bannerProductMapping()
		{
		    this.ToTable("bannerProduct");
   			this.HasKey(m => m.bannerProductId).Property(m=>m.bannerProductId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		