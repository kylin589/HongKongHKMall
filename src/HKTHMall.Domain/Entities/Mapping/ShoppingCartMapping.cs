using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class ShoppingCartMapping : EntityTypeConfiguration<ShoppingCart>
	{
		public ShoppingCartMapping()
		{
		    this.ToTable("ShoppingCart");
   			this.HasKey(m => m.ShoppingCartId).Property(m=>m.ShoppingCartId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		