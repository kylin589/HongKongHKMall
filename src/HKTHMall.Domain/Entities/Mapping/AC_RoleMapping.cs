using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class AC_RoleMapping : EntityTypeConfiguration<AC_Role>
	{
		public AC_RoleMapping()
		{
		    this.ToTable("AC_Role");
   			this.HasKey(m => m.RoleID).Property(m=>m.RoleID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		