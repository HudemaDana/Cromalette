using BApp.Domain.Models;

namespace BApp.Services.Interfaces
{
    public interface IUserLevelService
    {
        Task CreateUserLevelOnUserSignUp(int userId);

        Task DeleteUserLevelOnUserDelete(int userId);

        Task UpdateUserLevelOnColorSave(UserColor userColor);

    }
}
