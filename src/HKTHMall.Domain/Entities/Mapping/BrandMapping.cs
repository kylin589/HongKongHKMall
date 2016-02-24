using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class BrandMapping : EntityTypeConfiguration<Brand>
	{
		public BrandMapping()
		{
		    this.ToTable("Brand");
   			this.HasKey(m => m.BrandID).Property(m=>m.BrandID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		