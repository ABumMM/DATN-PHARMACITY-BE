using Microsoft.AspNetCore.Identity;

namespace backend.Models;
 
public class Roles : IdentityRole<Guid>
{
    public DateTime? CreateAt { get; set; }

    public virtual ICollection<Users> Users { get; set; } = new List<Users>();
}
