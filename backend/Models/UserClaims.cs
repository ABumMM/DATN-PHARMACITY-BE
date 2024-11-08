using Microsoft.AspNetCore.Identity;

namespace backend.Models
{
    public class UserClaims : IdentityUserClaim<Guid>
    {
        public DateTime? CreateAt { get; set; }
    }
}
