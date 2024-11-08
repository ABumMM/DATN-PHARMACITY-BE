using Microsoft.AspNetCore.Identity;

namespace backend.Models
{
    public class UserLogins : IdentityUserLogin<Guid>
    {
        public DateTime? CreateAt { get; set; }
    }
}
