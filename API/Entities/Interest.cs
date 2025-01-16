namespace API.Entities;

public class Interest
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Quan hệ
    public ICollection<UserInterest>? UserInterests { get; set; }
}