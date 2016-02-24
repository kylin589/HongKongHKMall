using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class PaymentOrderMapping : EntityTypeConfiguration<PaymentOrder>
	{
		public PaymentOrderMapping()
		{
		    this.ToTable("PaymentOrder");
   			this.HasKey(m => m.PaymentOrderID).Property(m=>m.PaymentOrderID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		