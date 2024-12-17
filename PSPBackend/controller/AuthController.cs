using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PSPBackend.Dto;
using PSPBackend.Model;
using System.Net;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("AuthController encountered invalid model during login (returning status 400)");
                return BadRequest(ModelState);
            }
            try
            {
                UserModel userFromDb = await _userManager.FindByNameAsync(loginRequestDto.Username);
                if (
                    userFromDb == null
                    || !await _userManager.CheckPasswordAsync(userFromDb, loginRequestDto.Password)
                )
                {
                    _logger.LogWarning("AuthController login failed for username: {Username}. Invalid credentials.", loginRequestDto.Username);
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

                LoginResponseDto loginResponse =
                    new()
                    {
                        Username = userFromDb.UserName,
                        AuthToken = token
                    };

                _logger.LogInformation("AuthController. Token generated for user {Username} with role {Role}.", userFromDb.UserName, role);

                _logger.LogInformation("AuthController. User {Username} logged in successfully.", userFromDb.UserName);

                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError("AuthController encountered an error during login for {Username}: {Error}", loginRequestDto.Username, ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Internal server error." });
            }
        }

        [Authorize(Roles = "SUPER_ADMIN, OWNER")]
        [HttpPost("register/user")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("AuthController encountered invalid model during user registration (returning status 400)");
                return BadRequest(ModelState);
            }

            UserModel currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                _logger.LogWarning("AuthController could not find user.");
                return Unauthorized(new { message = "User not found." });
            }

            var roles = await _userManager.GetRolesAsync(currentUser);

            if (roles == null || !roles.Any())
            {
                _logger.LogError("AuthController. User {Username} has no assigned roles.", currentUser.UserName);
                return Unauthorized(new { message = "User has no assigned roles." });
            }

            string currentUserRole = roles.First();

            if (currentUserRole != UserRole.SUPER_ADMIN.ToString() && currentUserRole != UserRole.OWNER.ToString())
            {
                _logger.LogWarning("AuthController. User {Username} does not have permission to register a user.", currentUser.UserName);
                return Forbid();
            }

            UserModel employeeUserFromDb = _context.User.FirstOrDefault(
                u => u.NormalizedUserName == registerUserDto.Username.ToUpper()
            );

            if (employeeUserFromDb != null)
            {
                _logger.LogError("AuthController encountered user being registered already exists (returning status 400)");
                return BadRequest(new { message = "User being registered already exists."});
            }

            var business = await _context.Business.FindAsync(registerUserDto.BusinessId);
            if (business == null)
            {
                _logger.LogError("AuthController. Business ID {BusinessId} not found.", registerUserDto.BusinessId);
                return BadRequest(new { message = "Invalid Business ID." });
            }

            UserModel newUser = new UserModel
            {
                UserName = registerUserDto.Username,
                FullName = registerUserDto.FullName,
                AccountStatus = 1,
                BusinessId = registerUserDto.BusinessId,
                TipsAmount = 0m,
            };

            try
            {
                var result = await _userManager.CreateAsync(newUser, registerUserDto.Password);
                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(registerUserDto.UserRole))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(UserRole.SUPER_ADMIN.ToString()));
                        await _roleManager.CreateAsync(new IdentityRole(UserRole.OWNER.ToString()));
                        await _roleManager.CreateAsync(new IdentityRole(UserRole.EMPLOYEE.ToString()));
                    }

                    await _userManager.AddToRoleAsync(newUser, registerUserDto.UserRole);
                    _logger.LogInformation("AuthController successfully registered a new user.");
                    return Ok();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            _logger.LogError("AuthController encountered an error during new user registration.");
            return BadRequest(new { message = "AuthController encountered and error  during new user registration."});
        }
    }
}
