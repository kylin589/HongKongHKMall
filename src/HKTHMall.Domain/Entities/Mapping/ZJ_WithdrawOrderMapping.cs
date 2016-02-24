using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class ZJ_WithdrawOrderMapping : EntityTypeConfiguration<ZJ_WithdrawOrder>
	{
		public ZJ_WithdrawOrderMapping()
		{
		    this.ToTable("ZJ_WithdrawOrder");
   			this.HasKey(m => m.OrderNO).Property(m=>m.OrderNO).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		