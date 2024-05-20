using BApp.Domain.DTOs;
using WBizTrip.Client.Authentication;

namespace BApp.Services.Interfaces
{
    public interface IUserService
    {
        Task CreateUser(SignUpDTO signUpModel);

        Task<HttpResponseMessage> LoginUser(LoginRequest userDto);
    }
}
