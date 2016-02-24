using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class ItemsMapping : EntityTypeConfiguration<Items>
	{
		public ItemsMapping()
		{
		    this.ToTable("Items");
   			this.HasKey(m => m.ItemId).Property(m=>m.ItemId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		