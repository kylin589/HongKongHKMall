using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class OrdersMapping : EntityTypeConfiguration<Orders>
	{
		public OrdersMapping()
		{
		    this.ToTable("Orders");
   			this.HasKey(m => m.OrderId).Property(m=>m.OrderId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		