using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class ZJ_RechargeOrderMapping : EntityTypeConfiguration<ZJ_RechargeOrder>
	{
		public ZJ_RechargeOrderMapping()
		{
		    this.ToTable("ZJ_RechargeOrder");
   			this.HasKey(m => m.OrderNO).Property(m=>m.OrderNO).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		