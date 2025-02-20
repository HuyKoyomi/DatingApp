namespace API.DTOs
{
    public class UserDto
    {
        public string? UserName { get; set; }
        public required string KnowAs { get; set; }
        public required string Token { get; set; }
        public required string Gender { get; set; }
        public string? PhotoUrl { get; set; }

    }
}