namespace Routify.Services;

public class TokenStore
{
    private const string Key = "auth_token";

    public async Task GuardarAsync(string token)
    {
        await SecureStorage.Default.SetAsync(Key, token);
    }

    public async Task<string?> ObtenerAsync()
    {
        return await SecureStorage.Default.GetAsync(Key);
    }

    public void Limpiar()
    {
        SecureStorage.Default.Remove(Key);
    }
}