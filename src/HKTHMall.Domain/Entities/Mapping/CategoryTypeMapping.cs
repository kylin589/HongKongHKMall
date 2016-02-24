using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class CategoryTypeMapping : EntityTypeConfiguration<CategoryType>
	{
		public CategoryTypeMapping()
		{
		    this.ToTable("CategoryType");
   			this.HasKey(m => m.CategoryTypeId).Property(m=>m.CategoryTypeId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		