using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class CompoundKeyMasterMapping : EntityTypeConfiguration<CompoundKeyMaster>
	{
		public CompoundKeyMasterMapping()
		{
		    this.ToTable("CompoundKeyMaster");
   			this.HasKey(m => m.IdPart1).Property(m=>m.IdPart1).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		