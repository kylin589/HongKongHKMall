using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class MultiLanguageMapping : EntityTypeConfiguration<MultiLanguage>
	{
		public MultiLanguageMapping()
		{
		    this.ToTable("MultiLanguage");
   			this.HasKey(m => m.LangKey).Property(m=>m.LangKey).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		