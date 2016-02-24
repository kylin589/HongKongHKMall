using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class BD_BankMapping : EntityTypeConfiguration<BD_Bank>
	{
		public BD_BankMapping()
		{
		    this.ToTable("BD_Bank");
   			this.HasKey(m => m.BankID).Property(m=>m.BankID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		