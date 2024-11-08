using Microsoft.AspNetCore.Identity;

namespace backend.Models
{
    public class UserTokens : IdentityUserToken<Guid>
    {
        public DateTime? CreateAt { get; set; }
    }
}
