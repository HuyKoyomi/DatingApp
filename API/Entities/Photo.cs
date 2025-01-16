using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Entities;

[Table("Photos")]
public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; }

    // navigation properties
    public int AppUserId { get; set; }
    [JsonIgnore]
    public AppUser AppUser { get; set; } = null!; // Yêu cầu tham chiếu điều hướng 
}