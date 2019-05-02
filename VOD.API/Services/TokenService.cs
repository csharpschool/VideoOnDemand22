using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VOD.Common.DTOModels;
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
