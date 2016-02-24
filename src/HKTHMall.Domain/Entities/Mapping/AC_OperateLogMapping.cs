using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class AC_OperateLogMapping : EntityTypeConfiguration<AC_OperateLog>
	{
		public AC_OperateLogMapping()
		{
		    this.ToTable("AC_OperateLog");
   			this.HasKey(m => m.OperateID).Property(m=>m.OperateID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		