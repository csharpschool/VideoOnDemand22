using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VOD.Common.DTOModels;
using VOD.Common.Entities;
using VOD.Database.Services;

namespace VOD.API.Services
{
    public class TokenService : ITokenService
    {
        #region Properties
        private readonly IConfiguration _configuration;
        private readonly IUserService _users;
        #endregion

        #region Constructors
        public TokenService(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _users = userService;
        }
        #endregion

        #region Helper Methods
        private List<Claim> GetClaims(VODUser user, bool includeUserClaims)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (includeUserClaims)
                foreach (var claim in user.Claims)
                    if (!claim.Type.Equals("Token") &&
                        !claim.Type.Equals("TokenExpires")) claims.Add(claim);

            return claims;
        }

        #endregion

        #region Token Methods
        public Task<TokenDTO> GenerateTokenAsync(LoginUserDTO loginUserDto)
        {
            throw new NotImplementedException();
        }

        public Task<TokenDTO> GetTokenAsync(LoginUserDTO loginUserDto, string userId)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
