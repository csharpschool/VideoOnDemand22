using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
        private TokenDTO CreateToken(IList<Claim> claims)
        {
            try
            {
                var signingKey = Convert.FromBase64String(_configuration["Jwt:SigningSecret"]);
                var credentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature);
                var duration = int.Parse(_configuration["Jwt:Duration"]);
                var now = DateTime.UtcNow;

                var jwtToken = new JwtSecurityToken
                (
                    issuer: "http://your-domain.com",
                    audience: "http://audience-domain.com",
                    notBefore: now,
                    expires: now.AddDays(duration),
                    claims: claims,
                    signingCredentials: credentials
                );

                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var token = jwtTokenHandler.WriteToken(jwtToken);
                return new TokenDTO(token, jwtToken.ValidTo);
            }
            catch
            {
                throw;
            }
        }
        private async Task<bool> AddTokenToUserAsync(string userId, TokenDTO token)
        {
            var userDTO = await _users.GetUserAsync(userId);
            userDTO.Token.Token = token.Token;
            userDTO.Token.TokenExpires = token.TokenExpires;

            return await _users.UpdateUserAsync(userDTO);
        }
        #endregion

        #region Token Methods
        public async Task<TokenDTO> GenerateTokenAsync(LoginUserDTO loginUserDto)
        {
            try
            {
                var user = await _users.GetUserAsync(loginUserDto, true);

                if (user == null) throw new UnauthorizedAccessException();

                var claims = GetClaims(user, true);

                var token = CreateToken(claims);

                var succeeded = await AddTokenToUserAsync(user.Id, token);
                if (!succeeded) throw new SecurityTokenException("Could not add token to user");

                return token;
            }
            catch
            {
                throw;
            }
        }

        public async Task<TokenDTO> GetTokenAsync(LoginUserDTO loginUserDto, string userId)
        {
            try
            {
                var user = await _users.GetUserAsync(loginUserDto, true);

                if (user == null) throw new UnauthorizedAccessException();
                if (!userId.Equals(user.Id)) throw new UnauthorizedAccessException();

                return new TokenDTO(user.Token, user.TokenExpires);
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
