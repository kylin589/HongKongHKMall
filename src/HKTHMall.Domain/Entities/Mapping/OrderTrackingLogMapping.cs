using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class OrderTrackingLogMapping : EntityTypeConfiguration<OrderTrackingLog>
	{
		public OrderTrackingLogMapping()
		{
		    this.ToTable("OrderTrackingLog");
   			this.HasKey(m => m.OrderTrackingId).Property(m=>m.OrderTrackingId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		