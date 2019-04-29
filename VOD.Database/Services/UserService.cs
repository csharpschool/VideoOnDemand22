using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VOD.Common.DTOModels;
using VOD.Common.Entities;
using VOD.Database.Contexts;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace VOD.Database.Services
{
    public class UserService : IUserService
    {
        #region Properties
        private readonly VODContext _db;
        private readonly UserManager<VODUser> _userManager;
        #endregion

        #region Constructor
        public UserService(VODContext db, UserManager<VODUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        #endregion

        #region Methods
        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            return await _db.Users
                .OrderBy(u => u.Email)
                .Select(user => new UserDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    IsAdmin = _db.UserRoles.Any(ur =>
                        ur.UserId.Equals(user.Id) &&
                        ur.RoleId.Equals(1.ToString()))
                }
                ).ToListAsync();

        }
        #endregion
    }
}
