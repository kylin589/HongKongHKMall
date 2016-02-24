using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class SalesProductMapping : EntityTypeConfiguration<SalesProduct>
	{
		public SalesProductMapping()
		{
		    this.ToTable("SalesProduct");
   			this.HasKey(m => m.SalesProductId).Property(m=>m.SalesProductId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		