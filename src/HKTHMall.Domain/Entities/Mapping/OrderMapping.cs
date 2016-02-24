using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class OrderMapping : EntityTypeConfiguration<Order>
	{
		public OrderMapping()
		{
		    this.ToTable("Order");
   			this.HasKey(m => m.OrderID).Property(m=>m.OrderID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		