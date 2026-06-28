using BuildingBlocks.Application.Exceptions;
using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using System.Net.Http.Json;

namespace Identity.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly HttpClient _httpClient;

    public IdentityService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginDto> Login(string email, string password)
    {
        try
        {
            var request = new
            {
                UserName = email,
                Password = password,
                RememberMe = true
            };

            var response = await _httpClient.PostAsJsonAsync("login", request);
            response.EnsureSuccessStatusCode();

            var apiResponse = await response.Content.ReadFromJsonAsync<LoginApiResponseDto>();
            if (apiResponse == null || !apiResponse.Success || apiResponse.Data == null)
                throw AppException.Unauthorized("Usuário ou Senha incorretos!");

            return new LoginDto
            {
                Token = apiResponse.Data.Token,
                ExpiresAt = apiResponse.Data.ExpiresIn,
                User = apiResponse.Data.User
            };
        }
        catch
        {
            throw AppException.Unauthorized("Usuário ou senha inválidos!");
        }
    }

    public async Task ForgotPassword(string email)
    {
        var request = new
        {
            Email = email,
            UrlTemplate = "string"
        };

        var response = await _httpClient.PostAsJsonAsync("/forgot-password", request);
        response.EnsureSuccessStatusCode();

        throw new NotImplementedException();
    }
}