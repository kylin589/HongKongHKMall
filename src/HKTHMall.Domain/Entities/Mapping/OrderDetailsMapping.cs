using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class OrderDetailsMapping : EntityTypeConfiguration<OrderDetails>
	{
		public OrderDetailsMapping()
		{
		    this.ToTable("OrderDetails");
   			this.HasKey(m => m.OrderDetailsID).Property(m=>m.OrderDetailsID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		