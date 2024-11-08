using Microsoft.AspNetCore.Identity;

namespace backend.Models
{
    public class RoleClaims : IdentityRoleClaim<Guid>
    {
        public DateTime? CreateAt { get; set; }
    }
}
