using System.Threading.Tasks;
using VOD.Common.DTOModels;

namespace VOD.API.Services
{
    public interface ITokenService
    {
        Task<TokenDTO> GenerateTokenAsync(LoginUserDTO loginUserDto);
        Task<TokenDTO> GetTokenAsync(LoginUserDTO loginUserDto, string userId);
    }
}
