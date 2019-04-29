using Microsoft.AspNetCore.Identity;
using VOD.Common.Entities;
using VOD.Database.Contexts;

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
    }
}
