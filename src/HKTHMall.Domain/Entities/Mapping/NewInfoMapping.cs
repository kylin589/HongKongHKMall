using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class NewInfoMapping : EntityTypeConfiguration<NewInfo>
	{
		public NewInfoMapping()
		{
		    this.ToTable("NewInfo");
   			this.HasKey(m => m.NewInfoId).Property(m=>m.NewInfoId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		