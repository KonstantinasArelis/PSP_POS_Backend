// using Microsoft.AspNetCore.Mvc;
// using PSPBackend.Model;
// using PSPBackend.Service;

// [ApiController]
// [Route("[controller]")]
// public class UserController : ControllerBase
// {
//     private readonly UserService _userService;

//     public UserController(UserService userService)
//     {
//         _userService = userService;
//     }

//     [HttpGet]
//     public IActionResult Get(
//         [FromQuery] int pageNr = 0,
//         [FromQuery] int limit = 20,
//         [FromQuery] int? id = null,
//         [FromQuery] int? businessId = null,
//         [FromQuery] string userName = null,
//         [FromQuery] string userUsername = null,
//         [FromQuery] int? userRole = null,
//         [FromQuery] int? accountStatus = null
//     )
//     {
//         if (limit > 100)
//         {
//             return BadRequest();
//         }

//         List<UserModel> gottenUsers = _userService.GetUsers(
//             pageNr,
//             limit,
//             id,
//             businessId,
//             userName,
//             userUsername,
//             userRole,
//             accountStatus
//         );

//         return Ok(gottenUsers);
//     }

//     [HttpPost]
//     public IActionResult CreateUser([FromBody] UserModel user)
//     {
//         if (!ModelState.IsValid)
//         {
//             return BadRequest("Invalid model");
//         }

//         UserModel createdUser = _userService.CreateUser(user);

//         if (createdUser != null)
//         {
//             return Ok(createdUser);
//         }
//         else
//         {
//             return StatusCode(500, "An error occurred while creating the user");
//         }
//     }
// }
