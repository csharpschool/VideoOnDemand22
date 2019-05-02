using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VOD.Common.DTOModels;
using VOD.Common.Entities;
using VOD.Database.Contexts;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VOD.Common.Extensions;
using System.Security.Claims;

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
                    Token = new TokenDTO(user.Token, user.TokenExpires),
                    IsAdmin = _db.UserRoles.Any(ur =>
                        ur.UserId.Equals(user.Id) &&
                        ur.RoleId.Equals(1.ToString()))
                }
                ).ToListAsync();

        }
        public async Task<UserDTO> GetUserAsync(string userId)
        {
            return await(_db.Users
                .Select(user => new UserDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    Token = new TokenDTO(user.Token, user.TokenExpires),
                    IsAdmin = _db.UserRoles.Any(ur =>
                        ur.UserId.Equals(user.Id) &&
                        ur.RoleId.Equals(1.ToString()))
                })
            ).FirstOrDefaultAsync(u => u.Id.Equals(userId));
        }
        public async Task<UserDTO> GetUserEmailAsync(string email)
        {
            return await(_db.Users
                .Select(user => new UserDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    Token = new TokenDTO(user.Token, user.TokenExpires),
                    IsAdmin = _db.UserRoles.Any(ur =>
                        ur.UserId.Equals(user.Id) &&
                        ur.RoleId.Equals(1.ToString()))
                })
            ).FirstOrDefaultAsync(u => u.Email.Equals(email));
        }
        public async Task<IdentityResult> AddUserAsync(RegisterUserDTO user)
        {
            var dbUser = new VODUser { UserName = user.Email, Email = user.Email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(dbUser, user.Password);
            return result;
        }
        public async Task<bool> UpdateUserAsync(UserDTO user)
        {
            var dbUser = await _db.Users.FirstOrDefaultAsync(u => u.Id.Equals(user.Id));
            if (dbUser == null) return false;
            if (string.IsNullOrEmpty(user.Email)) return false;

            dbUser.Email = user.Email;

            #region Create New Token
            if (user.Token != null && user.Token.Token != null && user.Token.Token.Length > 0)
            {
                dbUser.Token = user.Token.Token;
                dbUser.TokenExpires = user.Token.TokenExpires;

                // Create new claims for the token and expiration date
                var newTokenClaim = new Claim("Token", user.Token.Token);
                var newTokenExpires = new Claim("TokenExpires", user.Token.TokenExpires.ToString("yyyy-MM-dd hh:mm:ss"));

                // Fetch the token and expiration claims if they exist
                var userClaims = await _userManager.GetClaimsAsync(dbUser);
                var currentTokenClaim = userClaims.SingleOrDefault(c => c.Type.Equals("Token"));
                var currentTokenClaimExpires = userClaims.SingleOrDefault(c => c.Type.Equals("TokenExpires"));

                // Add or replace the claims for the token and expiration date
                if (currentTokenClaim == null)
                    await _userManager.AddClaimAsync(dbUser, newTokenClaim);
                else
                    await _userManager.ReplaceClaimAsync(dbUser,
                        currentTokenClaim, newTokenClaim);

                if (currentTokenClaimExpires == null)
                    await _userManager.AddClaimAsync(dbUser, newTokenExpires);
                else
                    await _userManager.ReplaceClaimAsync(dbUser,
                        currentTokenClaimExpires, newTokenExpires);
            }
            #endregion

            #region Admin Role and Claim
            var admin = "Admin";
            var isAdmin = await _userManager.IsInRoleAsync(dbUser, admin);
            var adminClaim = new Claim(admin, "true");

            if (isAdmin && !user.IsAdmin)
            {
                // Remove Admin Role
                await _userManager.RemoveFromRoleAsync(dbUser, admin);

                // Remove Admin Claim
                await _userManager.RemoveClaimAsync(dbUser, adminClaim);
            }
            else if (!isAdmin && user.IsAdmin)
            {
                // Add Admin Role
                await _userManager.AddToRoleAsync(dbUser, admin);

                // Add Admin Claim
                await _userManager.AddClaimAsync(dbUser, adminClaim);
            }
            #endregion

            var result = await _db.SaveChangesAsync();
            return result >= 0;
        }
        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                // Fetch user
                var dbUser = await _userManager.FindByIdAsync(userId);
                if (dbUser == null) return false;

                // Remove roles from user
                var userRoles = await _userManager.GetRolesAsync(dbUser);
                var roleRemoved = await _userManager.RemoveFromRolesAsync(dbUser, userRoles);

                // Remove the user's claims
                var userClaims = _db.UserClaims.Where(ur => ur.UserId.Equals(dbUser.Id));
                _db.UserClaims.RemoveRange(userClaims);

                // Remove the user
                var deleted = await _userManager.DeleteAsync(dbUser);
                return deleted.Succeeded;
            }
            catch
            {
                return false;
            }
        }
        public async Task<VODUser> GetUserAsync(LoginUserDTO loginUser, bool includeClaims = false)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginUser.Email);
                if (user == null) return null;
                if (loginUser.Password.IsNullOrEmptyOrWhiteSpace() && loginUser.PasswordHash.IsNullOrEmptyOrWhiteSpace()) return null;
                if (loginUser.Password.Length > 0)
                {
                    var password = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash,loginUser.Password);
                    if (password == PasswordVerificationResult.Failed) return null;
                }
                else
                {
                    if (!user.PasswordHash.Equals(loginUser.PasswordHash)) return null;
                }

                // Include the user's claims
                if (includeClaims) user.Claims = await _userManager.GetClaimsAsync(user);

                return user;
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
