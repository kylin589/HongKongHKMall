using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class ExceptionLogMapping : EntityTypeConfiguration<ExceptionLog>
	{
		public ExceptionLogMapping()
		{
		    this.ToTable("ExceptionLog");
   			this.HasKey(m => m.ElId).Property(m=>m.ElId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		