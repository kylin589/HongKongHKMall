using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class THAreaMapping : EntityTypeConfiguration<THArea>
	{
		public THAreaMapping()
		{
		    this.ToTable("THArea");
   			this.HasKey(m => m.THAreaID).Property(m=>m.THAreaID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		