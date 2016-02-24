using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class OrderAddressMapping : EntityTypeConfiguration<OrderAddress>
	{
		public OrderAddressMapping()
		{
		    this.ToTable("OrderAddress");
   			this.HasKey(m => m.OrderAddressId).Property(m=>m.OrderAddressId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		