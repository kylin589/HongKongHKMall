using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class SchemaTableMapping : EntityTypeConfiguration<SchemaTable>
	{
		public SchemaTableMapping()
		{
		    this.ToTable("SchemaTable");
   			this.HasKey(m => m.Id).Property(m=>m.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		