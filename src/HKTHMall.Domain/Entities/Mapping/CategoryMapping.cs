using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class CategoryMapping : EntityTypeConfiguration<Category>
	{
		public CategoryMapping()
		{
		    this.ToTable("Category");
   			this.HasKey(m => m.CategoryId).Property(m=>m.CategoryId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		