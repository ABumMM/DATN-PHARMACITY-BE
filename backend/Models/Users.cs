using Microsoft.AspNetCore.Identity;

namespace backend.Models;

public class Users : IdentityUser<Guid>
{
    public string? Name { get; set; }

    public string Password { get; set; } = null!;
     
    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? PathImg { get; set; }

    public Guid? IdRole { get; set; }
        
    public DateTime? CreateAt { get; set; }

    public virtual Roles? IdRoleNavigation { get; set; }

    public virtual ICollection<Products> Products { get; set; } = new List<Products>();
}
