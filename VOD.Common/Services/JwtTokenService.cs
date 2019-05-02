using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VOD.Common.DTOModels;
using VOD.Common.Entities;
using VOD.Common.Extensions;

namespace VOD.Common.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        #region Properties
        private readonly HttpClientFactoryService _http;
        private readonly UserManager<VODUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor
        public JwtTokenService(HttpClientFactoryService http, UserManager<VODUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _http = http;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Token Methods
        public async Task<TokenDTO> CreateTokenAsync()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await _userManager.FindByIdAsync(userId);
                var tokenUser = new LoginUserDTO
                {
                    Email = user.Email,
                    Password = "",
                    PasswordHash = user.PasswordHash
                };
                var token = await _http.CreateTokenAsync(tokenUser, "api/token", "AdminClient");

                return token;
            }
            catch
            {
                return default;
            }
        }
        public async Task<TokenDTO> GetTokenAsync()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await _userManager.FindByIdAsync(userId);
                var claims = await _userManager.GetClaimsAsync(user);
                var token = claims.Single(c => c.Type.Equals("Token")).Value;
                var date = claims.Single(c => c.Type.Equals("TokenExpires")).Value;
                DateTime expires;
                var succeeded = DateTime.TryParse(date, out expires);

                // Return token from the user object
                if (succeeded && !token.IsNullOrEmptyOrWhiteSpace()) return new TokenDTO(token, expires);

                // Return token from the API
                var tokenUser = new LoginUserDTO
                {
                    Email = user.Email,
                    Password = "",
                    PasswordHash = user.PasswordHash
                };
                var newToken = await _http.GetTokenAsync(tokenUser, $"api/token/{user.Id}", "AdminClient");
                return newToken;
            }
            catch
            {
                return default;
            }
        }
        public async Task<TokenDTO> CheckTokenAsync(TokenDTO token)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
