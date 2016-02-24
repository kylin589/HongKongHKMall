using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class ZJ_AmountChangeType_langMapping : EntityTypeConfiguration<ZJ_AmountChangeType_lang>
	{
		public ZJ_AmountChangeType_langMapping()
		{
		    this.ToTable("ZJ_AmountChangeType_lang");
   			this.HasKey(m => m.ZJTypeId).Property(m=>m.ZJTypeId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		