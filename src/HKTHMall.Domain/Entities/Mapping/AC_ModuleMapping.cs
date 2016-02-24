using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class AC_ModuleMapping : EntityTypeConfiguration<AC_Module>
	{
		public AC_ModuleMapping()
		{
		    this.ToTable("AC_Module");
   			this.HasKey(m => m.ModuleID).Property(m=>m.ModuleID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		