namespace Application.DTOs
{
    public class UserGridDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string AreaName { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public int? AreaId { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
