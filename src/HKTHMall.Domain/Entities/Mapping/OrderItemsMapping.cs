using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class OrderItemsMapping : EntityTypeConfiguration<OrderItems>
	{
		public OrderItemsMapping()
		{
		    this.ToTable("OrderItems");
   			this.HasKey(m => m.OrderItemId).Property(m=>m.OrderItemId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		