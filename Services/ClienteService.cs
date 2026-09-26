using System.Net.Http.Json;
using Routify.Shared.Dtos;

namespace Routify.Services;

public class ClienteService
{
    private readonly HttpClient _http;

    public ClienteService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ClienteDto>> ObtenerTodosAsync(string? buscar = null)
    {
        var url = "api/clientes";
        if (!string.IsNullOrWhiteSpace(buscar))
        {
            url += $"?buscar={Uri.EscapeDataString(buscar)}";
        }

        var resultado = await _http.GetFromJsonAsync<List<ClienteDto>>(url);
        return resultado ?? new List<ClienteDto>();
    }

    public async Task<ClienteDto?> CrearAsync(ClienteRequestDto request)
    {
        var response = await _http.PostAsJsonAsync("api/clientes", request);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<ClienteDto>();
    }
}