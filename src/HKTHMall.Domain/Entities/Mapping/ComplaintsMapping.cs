using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class ComplaintsMapping : EntityTypeConfiguration<Complaints>
	{
		public ComplaintsMapping()
		{
		    this.ToTable("Complaints");
   			this.HasKey(m => m.ComplaintsID).Property(m=>m.ComplaintsID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		