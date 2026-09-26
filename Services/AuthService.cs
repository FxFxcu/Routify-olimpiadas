namespace Routify.Services;

using System.Net.Http.Json;
using Routify.Shared.Dtos;


public class AuthService
{
    private readonly HttpClient _http;
    private readonly TokenStore _tokenStore;

    public AuthService(HttpClient http, TokenStore tokenStore)
    {
        _http = http;
        _tokenStore = tokenStore;
    }

    public async Task<(bool Exito, string? Error)> LoginAsync(string nombreUsuario, string password)
    {
        var request = new LoginRequestDto
        {
            NombreUsuario = nombreUsuario,
            Password = password
        };

        HttpResponseMessage response;
        try
        {
            response = await _http.PostAsJsonAsync("api/auth/login", request);
        }
        catch (Exception)
        {
            return (false, "No se pudo conectar con el servidor.");
        }

        if (!response.IsSuccessStatusCode)
        {
            return (false, "Usuario o contraseña incorrectos.");
        }

        var resultado = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        if (resultado is null) return (false, "Respuesta inválida del servidor.");

        await _tokenStore.GuardarAsync(resultado.Token);
        return (true, null);
    }

    public async Task<bool> EstaAutenticadoAsync()
    {
        var token = await _tokenStore.ObtenerAsync();
        return !string.IsNullOrEmpty(token);
    }

    public void Logout()
    {
        _tokenStore.Limpiar();
    }
}