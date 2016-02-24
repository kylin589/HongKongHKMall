using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class Brand_CategoryMapping : EntityTypeConfiguration<Brand_Category>
	{
		public Brand_CategoryMapping()
		{
		    this.ToTable("Brand_Category");
   			this.HasKey(m => m.Brand_CategoryId).Property(m=>m.Brand_CategoryId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		