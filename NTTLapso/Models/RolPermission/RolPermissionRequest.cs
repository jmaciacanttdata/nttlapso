namespace NTTLapso.Models.RolPermission
{
    public class RolPermissionRequest
    {
        public int IdRol { get; set; }

        public List<int> PermissionList { get; set; }
    }
}
