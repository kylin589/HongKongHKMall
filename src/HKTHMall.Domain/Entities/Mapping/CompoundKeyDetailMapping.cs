using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class CompoundKeyDetailMapping : EntityTypeConfiguration<CompoundKeyDetail>
	{
		public CompoundKeyDetailMapping()
		{
		    this.ToTable("CompoundKeyDetail");
   			this.HasKey(m => m.Id).Property(m=>m.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		