using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BrCms.Framework.Data;
 

namespace HKTHMall.Domain.Entities.Mapping
{
	[DbContextType((byte)BcDbType.Default)]
	public partial class MessageMapping : EntityTypeConfiguration<Message>
	{
		public MessageMapping()
		{
		    this.ToTable("Message");
   			this.HasKey(m => m.MsgId).Property(m=>m.MsgId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
   			this.Mapping();
		}
   		partial void Mapping();
	}
}
		