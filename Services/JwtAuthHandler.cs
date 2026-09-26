using System.Net.Http.Headers;

namespace Routify.App.Services;

public class JwtAuthHandler : DelegatingHandler
{
    private readonly TokenStore _tokenStore;

    public JwtAuthHandler(TokenStore tokenStore)
    {
        _tokenStore = tokenStore;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenStore.ObtenerAsync();
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}