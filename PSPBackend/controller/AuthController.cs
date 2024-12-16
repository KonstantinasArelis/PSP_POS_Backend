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
            _apiResponse = new ApiResponse();
            _logger = logger;
            _secretKey = configuration.GetValue<string>("JwtSettings:Secret");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            _apiResponse = new ApiResponse();

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
                    _apiResponse.StatusCode = HttpStatusCode.Unauthorized;
                    _apiResponse.IsSuccess = false;
                    _apiResponse.ErrorMessages.Add("Invalid username or password");
                    _logger.LogWarning("AuthController login failed for username: {Username}. Invalid credentials.", loginRequestDTO.Username);
                    return Unauthorized(_apiResponse);
                }

                var roles = await _userManager.GetRolesAsync(userFromDb);
                var role = roles.FirstOrDefault();

                if (role == null)
                {
                    _logger.LogError("AuthController. User {Username} has no assigned roles.", userFromDb.UserName);
                    _apiResponse.StatusCode = HttpStatusCode.Unauthorized;
                    _apiResponse.IsSuccess = false;
                    _apiResponse.ErrorMessages.Add("User has no assigned roles.");
                    return Unauthorized(_apiResponse);
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

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = loginResponse;

                _logger.LogInformation("AuthController. User {Username} logged in successfully.", userFromDb.UserName);

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError("AuthController encountered an error that occurred during login for " + loginRequestDTO.Username + ". | " + ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add("Internal server error.");
                return StatusCode((int)HttpStatusCode.InternalServerError, _apiResponse);
            }
        }
    }
}
