using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using VOD.Common.DTOModels;
using VOD.Common.Entities;

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
            throw new NotImplementedException();
        }
        public async Task<TokenDTO> CheckTokenAsync(TokenDTO token)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
