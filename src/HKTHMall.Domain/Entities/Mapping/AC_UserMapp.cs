namespace HKTHMall.Domain.Entities.Mapping
{
    public partial class AC_UserMapping
    {
        partial void Mapping()
        {
            this.HasRequired(m => m.AC_Role)
                .WithMany(m => m.AC_User)
                .HasForeignKey(m => m.RoleID);
            this.HasRequired(m => m.AC_Department)
                .WithMany(m => m.AC_User)
                .HasForeignKey(m => m.ID);
        }
    }
}
