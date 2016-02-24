using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class BD_NewsTypelangMapping : EntityTypeConfiguration<BD_NewsTypelang>
	{
		public BD_NewsTypelangMapping()
		{
		    this.ToTable("BD_NewsTypelang");
   			this.HasKey(m => m.BD_NewsTypelangId).Property(m=>m.BD_NewsTypelangId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		