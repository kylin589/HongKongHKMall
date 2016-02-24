using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class PurchaseOrderMapping : EntityTypeConfiguration<PurchaseOrder>
	{
		public PurchaseOrderMapping()
		{
		    this.ToTable("PurchaseOrder");
   			this.HasKey(m => m.PurchaseOrderId).Property(m=>m.PurchaseOrderId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		