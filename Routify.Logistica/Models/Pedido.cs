namespace Routify.Logistica.Models;

public enum EstadoPedido { Pendiente, Confirmado, Cancelado }

public class Pedido
{
    public int PedidoId { get; set; }
    public int ClienteId { get; set; }
    public DateTime FechaPedido { get; set; } = DateTime.UtcNow;
    public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;
    public string? Observaciones { get; set; }

    public Cliente Cliente { get; set; } = null!;
    public ICollection<Paquete> Paquetes { get; set; } = new List<Paquete>();

    // 1 a 0..1: el Envío no se crea con el pedido, se genera recién al confirmarlo
    // (por eso en la pantalla de Envíos no hay botón "Nuevo envío")
    public Envio? Envio { get; set; }

    public Pago? Pago { get; set; }
}
