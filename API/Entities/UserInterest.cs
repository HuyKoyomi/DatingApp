namespace API.Entities;

public class UserInterest
{
    public int Id { get; set; }
    public int AppUserId { get; set; }
    public required AppUser AppUser { get; set; }

    public int InterestId { get; set; }
    public required Interest Interest { get; set; }
}