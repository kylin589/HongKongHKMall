using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class CustomersMapping : EntityTypeConfiguration<Customers>
	{
		public CustomersMapping()
		{
		    this.ToTable("Customers");
   			this.HasKey(m => m.CustomerId).Property(m=>m.CustomerId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		