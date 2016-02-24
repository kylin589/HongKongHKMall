using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class ZJ_UserBalanceMapping : EntityTypeConfiguration<ZJ_UserBalance>
	{
		public ZJ_UserBalanceMapping()
		{
		    this.ToTable("ZJ_UserBalance");
   			this.HasKey(m => m.UserID).Property(m=>m.UserID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		