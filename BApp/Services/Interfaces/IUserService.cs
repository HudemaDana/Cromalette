using BApp.Domain.DTOs;
using BApp.Domain.Models;
using WBizTrip.Client.Authentication;

namespace BApp.Services.Interfaces
{
    public interface IUserService
    {
        Task CreateUser(SignUpDTO signUpModel);

        Task<HttpResponseMessage> LoginUser(LoginRequest userDto);

        Task<User> GetUserById(int id);

        Task UpdateUser(int id, User user);

        Task DeleteUser(int id);
    }
}
