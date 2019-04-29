using System.Collections.Generic;
using System.Threading.Tasks;
using VOD.Common.DTOModels;

namespace VOD.Database.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetUsersAsync();
    }
}
