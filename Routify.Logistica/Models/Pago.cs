namespace Routify.Logistica.Models;

public enum EstadoPago { Pendiente, Aprobado, Rechazado }

public class Pago
{
    public int PagoId { get; set; }
    public int PedidoId { get; set; }

    public decimal MontoPagado { get; set; }
    public string MedioPago { get; set; } = "MercadoPago";
    public string? IdTransaccionExterna { get; set; }
    public EstadoPago Estado { get; set; } = EstadoPago.Pendiente;
    public DateTime? FechaPago { get; set; }

    public Pedido Pedido { get; set; } = null!;
}
