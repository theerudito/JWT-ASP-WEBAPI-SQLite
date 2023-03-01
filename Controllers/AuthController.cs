using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWT_ASP_WEBAPI.Context;
using JWT_ASP_WEBAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JWT_ASP_WEBAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : Controller
  {
    private readonly DB_Context context;
    private readonly IConfiguration config;

    public AuthController(DB_Context context, IConfiguration config)
    {
      this.context = context;
      this.config = config;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
      return await context.Users.ToListAsync();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] UserDTO userDTO)
    {
      // Find user in database
      var user = await context.Users.FirstOrDefaultAsync(x => x.Email == userDTO.Email);

      // Check if user exists
      if (user == null) return Unauthorized("user does not exist");

      // Check if password is correct
      if (!BCrypt.Net.BCrypt.Verify(userDTO.Password, user.Password)) return Unauthorized("password or email is incorrect");

      // Create token
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(config["Jwt:Token"]);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
        {
          // password userName email role id 
          new Claim(ClaimTypes.Name, user.Username),
          new Claim(ClaimTypes.Email, user.Email),
          new Claim(ClaimTypes.Role, user.Role),
          new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
          //new Claim(ClaimTypes.Hash, user.Password)

        }),
        Expires = DateTime.UtcNow.AddDays(1),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);

      return Ok(new
      {
        token = tokenHandler.WriteToken(token)
      });
    }


    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] User user)
    {
      context.Users.Add(new User
      {
        Username = user.Username,
        Email = user.Email,
        Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
        Role = "Admin"
      });
      await context.SaveChangesAsync();
      return Ok("User created");
    }
  }
}
