using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class FavoritesMapping : EntityTypeConfiguration<Favorites>
	{
		public FavoritesMapping()
		{
		    this.ToTable("Favorites");
   			this.HasKey(m => m.FavoritesID).Property(m=>m.FavoritesID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		