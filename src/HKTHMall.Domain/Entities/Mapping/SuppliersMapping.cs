using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class SuppliersMapping : EntityTypeConfiguration<Suppliers>
	{
		public SuppliersMapping()
		{
		    this.ToTable("Suppliers");
   			this.HasKey(m => m.SupplierId).Property(m=>m.SupplierId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		