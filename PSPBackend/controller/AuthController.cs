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
        private ApiResponse _apiResponse;
        private string _secretKey;

        public AuthController(
            AppDbContext context,
            UserManager<UserModel> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration
        )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _secretKey = configuration.GetValue<string>("JwtSettings:Secret");
            _apiResponse = new ApiResponse();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            _apiResponse = new ApiResponse();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserModel userFromDb = await _userManager.FindByNameAsync(loginRequestDTO.Username);
            if (
                userFromDb == null
                || !await _userManager.CheckPasswordAsync(userFromDb, loginRequestDTO.Password)
            )
            {
                _apiResponse.StatusCode = HttpStatusCode.Unauthorized;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add("Invalid username or password");
                return Unauthorized(_apiResponse);
            }

            var roles = await _userManager.GetRolesAsync(userFromDb);
            var role = roles.FirstOrDefault();

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

            var business = await _context.Business.FindAsync(userFromDb.BusinessId);

            LoginResponseDTO loginResponse =
                new()
                {
                    Username = userFromDb.UserName,
                    Id = userFromDb.Id,
                    AuthToken = token,
                    Role = role,
                    BusinessId = userFromDb.BusinessId
                };

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.Data = loginResponse;
            return Ok(_apiResponse);
        }
    }
}
