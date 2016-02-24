using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class ReturnProductInfoMapping : EntityTypeConfiguration<ReturnProductInfo>
	{
		public ReturnProductInfoMapping()
		{
		    this.ToTable("ReturnProductInfo");
   			this.HasKey(m => m.ReturnOrderID).Property(m=>m.ReturnOrderID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		