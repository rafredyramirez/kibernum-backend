namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
