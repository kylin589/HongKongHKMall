using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class NotesMapping : EntityTypeConfiguration<Notes>
	{
		public NotesMapping()
		{
		    this.ToTable("Notes");
   			this.HasKey(m => m.NoteId).Property(m=>m.NoteId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		