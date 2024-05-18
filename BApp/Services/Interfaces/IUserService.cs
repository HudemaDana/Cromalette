using BApp.Domain.DTOs;

namespace BApp.Services.Interfaces
{
    public interface IUserService
    {
        Task CreateUser(SignUpDTO signUpModel);

        Task<HttpResponseMessage> LoginUser(UserDTO userDto);
    }
}
