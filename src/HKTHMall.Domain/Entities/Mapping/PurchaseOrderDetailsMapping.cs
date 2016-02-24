using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class PurchaseOrderDetailsMapping : EntityTypeConfiguration<PurchaseOrderDetails>
	{
		public PurchaseOrderDetailsMapping()
		{
		    this.ToTable("PurchaseOrderDetails");
   			this.HasKey(m => m.OrderDetailsID).Property(m=>m.OrderDetailsID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		