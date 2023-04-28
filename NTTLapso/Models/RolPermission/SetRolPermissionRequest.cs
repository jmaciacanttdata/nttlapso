namespace NTTLapso.Models.RolPermission
{
    public class SetRolPermissionRequest
    {
        public int IdRol { get; set; }

        public List<int> PermissionList { get; set; }
    }
}
