using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class YH_MerchantInfoMapping : EntityTypeConfiguration<YH_MerchantInfo>
	{
		public YH_MerchantInfoMapping()
		{
		    this.ToTable("YH_MerchantInfo");
   			this.HasKey(m => m.MerchantID).Property(m=>m.MerchantID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		