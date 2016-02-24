using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class APP_VersionInfoMapping : EntityTypeConfiguration<APP_VersionInfo>
	{
		public APP_VersionInfoMapping()
		{
		    this.ToTable("APP_VersionInfo");
   			this.HasKey(m => m.ID).Property(m=>m.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		