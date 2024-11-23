namespace backend.Models
{
    public class Suppliers
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; } 

        public DateTime? CreateAt { get; set; } 

    }
}
