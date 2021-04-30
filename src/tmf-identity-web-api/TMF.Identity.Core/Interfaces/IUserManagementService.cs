using System.Collections.Generic;
using System.Threading.Tasks;
using TMF.Identity.Core.Model;

namespace TMF.Identity.Core.Interfaces
{
    public interface IUserManagementService
    {
        Task<IReadOnlyList<UserEntity>> GetAllUsersAsync();
        Task<UserEntity> GetUserAsync(string userId);
        Task<UserEntity> CreateUserAsync(UserEntity userEntity);
        Task DeleteUserAsync(string userId);
    }
}
