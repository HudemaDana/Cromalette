using BApp.Domain.Models;

namespace BApp.Services.Interfaces
{
    public interface IUserService
    {
        User GetUserById(int id);
        Task CreateUser(User user);
        Task UpdateUser(int id, User user);
        Task DeleteUser(int Id);
        Task<string> AuthentificateUser(User _userData);
    }
}
