using Identity.Application.DTOs;

namespace Identity.Application.Interfaces;

public interface IIdentityService
{
    Task<LoginDto> Login(string email, string password);
    Task ForgotPassword(string email);
}
