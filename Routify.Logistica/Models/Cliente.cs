namespace Routify.Logistica.Models;

public class Cliente
{
    public int ClienteId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    public ICollection<Direccion> Direcciones { get; set; } = new List<Direccion>();
    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
