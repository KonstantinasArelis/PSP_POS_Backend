// using PSPBackend.Model;
// using PSPBackend.DTO;

// public class UserRepository
// {
//     private readonly AppDbContext _context;

//     public UserRepository(AppDbContext context)
//     {
//         _context = context;
//     }

//     public UserModel GetUserEntityByUsername(string username)
//     {
//         // Fetch the full User entity (with PasswordHash) for internal use
//         return _context.User.FirstOrDefault(u => u.UserUsername == username);
//     }

//     public UserDTO GetUserDTOByUsername(string username)
//     {
//         // Fetch sanitized UserDTO for external use (no PasswordHash)
//         var user = _context.User.FirstOrDefault(u => u.UserUsername == username);
//         if (user == null)
//             return null;

//         return new UserDTO
//         {
//             Id = user.Id,
//             BusinessId = user.BusinessId,
//             UserName = user.UserName,
//             UserUsername = user.UserUsername,
//             UserRole = user.UserRole,
//             TipsAmount = user.TipsAmount,
//             LastWithdrawnTimestamp = user.LastWithdrawnTimestamp,
//             AccountStatus = user.AccountStatus
//         };
//     }

//     public IQueryable<UserModel> GetUsers(
//         int pageNr,
//         int limit,
//         int? id,
//         int? businessId,
//         string userName,
//         string userUsername,
//         int? userRole,
//         int? accountStatus
//     )
//     {
//         var query = _context.User.AsQueryable();

//         if (id.HasValue)
//         {
//             query = query.Where(u => u.Id == id.Value);
//         }
//         if (businessId.HasValue)
//         {
//             query = query.Where(u => u.BusinessId == businessId.Value);
//         }
//         if (!string.IsNullOrEmpty(userName))
//         {
//             query = query.Where(u => u.UserName.Contains(userName));
//         }
//         if (!string.IsNullOrEmpty(userUsername))
//         {
//             query = query.Where(u => u.UserUsername.Contains(userUsername));
//         }
//         if (userRole.HasValue)
//         {
//             query = query.Where(u => u.UserRole == userRole.Value);
//         }
//         if (accountStatus.HasValue)
//         {
//             query = query.Where(u => u.AccountStatus == accountStatus.Value);
//         }

//         return query;
//     }

//     public int CreateUser(UserModel user)
//     {
//         _context.User.Add(user);
//         int rowsAffected = _context.SaveChanges();

//         return rowsAffected;
//     }
// }
