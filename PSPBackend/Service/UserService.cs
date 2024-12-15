// using PSPBackend.DTO;
// using PSPBackend.Model;

// namespace PSPBackend.Service
// {
//     public class UserService
//     {
//         private readonly UserRepository _userRepository;

//         public UserService(UserRepository userRepository)
//         {
//             _userRepository = userRepository;
//         }

//         public List<UserModel> GetUsers(
//             int pageNr,
//             int limit,
//             int? id,
//             int? businessId,
//             string userName,
//             string userUsername,
//             int? userRole,
//             int? accountStatus
//         )
//         {
//             var query = _userRepository.GetUsers(
//                 pageNr,
//                 limit,
//                 id,
//                 businessId,
//                 userName,
//                 userUsername,
//                 userRole,
//                 accountStatus
//             );

//             var user = query.Skip(pageNr * limit).Take(limit).ToList();
//             return user;
//         }

//         public UserModel CreateUser(UserModel user)
//         {
//             if (_userRepository.CreateUser(user) > 0)
//             {
//                 return user;
//             }
//             else
//             {
//                 return null;
//             }
//         }

//         public bool VerifyPassword(string password, string passwordHash)
//         {
//             // Use BCrypt to verify the password against its hash
//             return BCrypt.Net.BCrypt.Verify(password, passwordHash);
//         }
//     }
// }
