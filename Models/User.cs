using System.ComponentModel.DataAnnotations;

namespace JWT_ASP_WEBAPI.Models
{
  public class User
  {
    [Key]
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
  }
}
