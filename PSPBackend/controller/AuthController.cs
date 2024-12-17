using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PSPBackend.DTO;
using PSPBackend.Model;
using System.Net;

namespace PSPBackend.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<OrderController> _logger;
        private string _secretKey;

        public AuthController(
            AppDbContext context,
            UserManager<UserModel> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ILogger<OrderController> logger
        )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _secretKey = configuration.GetValue<string>("JwtSettings:Secret");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("AuthController encountered invalid model during login (returning status 400)");
                return BadRequest(ModelState);
            }
            try
            {
                UserModel userFromDb = await _userManager.FindByNameAsync(loginRequestDTO.Username);
                if (
                    userFromDb == null
                    || !await _userManager.CheckPasswordAsync(userFromDb, loginRequestDTO.Password)
                )
                {
                    _logger.LogWarning("AuthController login failed for username: {Username}. Invalid credentials.", loginRequestDTO.Username);
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                var roles = await _userManager.GetRolesAsync(userFromDb);
                var role = roles.FirstOrDefault();

                if (role == null)
                {
                    _logger.LogError("AuthController. User {Username} has no assigned roles.", userFromDb.UserName);
                    return Unauthorized(new { message = "User has no assigned roles." });
                }

                var claims = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.Sub, userFromDb.Id),
                    new(JwtRegisteredClaimNames.Name, userFromDb.UserName),
                    new(ClaimTypes.Role, role),
                    new("businessId", userFromDb.BusinessId.ToString())
                };

                SecurityTokenDescriptor securityTokenDescriptor =
                    new()
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                            SecurityAlgorithms.HmacSha256Signature
                        )
                    };

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken = tokenHandler.CreateToken(securityTokenDescriptor);
                string token = tokenHandler.WriteToken(securityToken);

                LoginResponseDTO loginResponse =
                    new()
                    {
                        Username = userFromDb.UserName,
                        AuthToken = token,
                        Role = role,
                        BusinessId = userFromDb.BusinessId
                    };

                _logger.LogInformation("AuthController. Token generated for user {Username} with role {Role}.", userFromDb.UserName, role);

                _logger.LogInformation("AuthController. User {Username} logged in successfully.", userFromDb.UserName);

                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError("AuthController encountered an error during login for {Username}: {Error}", loginRequestDTO.Username, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Internal server error." });
            }
        }
    }
}
