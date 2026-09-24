namespace Routify.Auth.Models;

public class Usuario
{
    public Guid UsuarioId { get; set; } = Guid.NewGuid();

    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;

    // Es el "Id de usuario" que se tipea en el login — distinto del email
    public string NombreUsuario { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    // Nunca se guarda la contraseña en texto plano, solo su hash
    public string PasswordHash { get; set; } = string.Empty;

    public string Rol { get; set; } = "Operador"; // Administrador, Operador, Repartidor...

    public DateTime FechaAlta { get; set; } = DateTime.UtcNow;
}
