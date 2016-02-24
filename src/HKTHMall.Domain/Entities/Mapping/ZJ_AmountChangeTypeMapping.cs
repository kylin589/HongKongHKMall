using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class ZJ_AmountChangeTypeMapping : EntityTypeConfiguration<ZJ_AmountChangeType>
	{
		public ZJ_AmountChangeTypeMapping()
		{
		    this.ToTable("ZJ_AmountChangeType");
   			this.HasKey(m => m.ID).Property(m=>m.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		