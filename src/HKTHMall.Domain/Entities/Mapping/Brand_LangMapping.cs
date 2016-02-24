using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class Brand_LangMapping : EntityTypeConfiguration<Brand_Lang>
	{
		public Brand_LangMapping()
		{
		    this.ToTable("Brand_Lang");
   			this.HasKey(m => m.Id).Property(m=>m.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		