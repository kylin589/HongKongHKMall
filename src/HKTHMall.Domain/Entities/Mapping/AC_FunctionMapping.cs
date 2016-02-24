using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class AC_FunctionMapping : EntityTypeConfiguration<AC_Function>
	{
		public AC_FunctionMapping()
		{
		    this.ToTable("AC_Function");
   			this.HasKey(m => m.FunctionID).Property(m=>m.FunctionID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		