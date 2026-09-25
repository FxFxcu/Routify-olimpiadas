using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routify.Auth.Data;
using Routify.Auth.Services;
using Routify.Shared.Dtos;

namespace Routify.Auth.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(AuthDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto request)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.NombreUsuario == request.NombreUsuario);

        if (usuario is null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
        {
            return Unauthorized("Usuario o contraseña incorrectos.");
        }

        var (token, expiraEn) = _tokenService.GenerarToken(usuario);

        return Ok(new LoginResponseDto
        {
            Token = token,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Rol = usuario.Rol,
            ExpiraEn = expiraEn
        });
    }
}