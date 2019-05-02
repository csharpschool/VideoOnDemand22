using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
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
        public Task<TokenDTO> CheckTokenAsync(TokenDTO token)
        {
            throw new NotImplementedException();
        }
        public Task<TokenDTO> CreateTokenAsync()
        {
            throw new NotImplementedException();
        }
        public Task<TokenDTO> GetTokenAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
