using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class OrderDetails_langMapping : EntityTypeConfiguration<OrderDetails_lang>
	{
		public OrderDetails_langMapping()
		{
		    this.ToTable("OrderDetails_lang");
   			this.HasKey(m => m.OrderDetails_langId).Property(m=>m.OrderDetails_langId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		