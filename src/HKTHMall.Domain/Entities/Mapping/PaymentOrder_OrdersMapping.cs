using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class PaymentOrder_OrdersMapping : EntityTypeConfiguration<PaymentOrder_Orders>
	{
		public PaymentOrder_OrdersMapping()
		{
		    this.ToTable("PaymentOrder_Orders");
   			this.HasKey(m => m.RelateID).Property(m=>m.RelateID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		