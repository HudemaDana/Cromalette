using BApp.Domain.Models;

namespace BApp.Services.Interfaces
{
    public interface IUserLevelService
    {
        UserLevel GetUserLevel(int userId);

        Task CreateUserLevelOnUserSignUp(int userId);

        Task DeleteUserLevelOnUserDelete(int userId);

        Task UpdateUserLevelOnColorSave(UserColor userColor);

    }
}
