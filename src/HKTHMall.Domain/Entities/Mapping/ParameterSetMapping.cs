using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class ParameterSetMapping : EntityTypeConfiguration<ParameterSet>
	{
		public ParameterSetMapping()
		{
		    this.ToTable("ParameterSet");
   			this.HasKey(m => m.ParamenterID).Property(m=>m.ParamenterID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		