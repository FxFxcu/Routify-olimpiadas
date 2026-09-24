namespace Routify.Logistica.Models;

public class Paquete
{
    public int PaqueteId { get; set; }
    public int PedidoId { get; set; }

    public string Descripcion { get; set; } = string.Empty;
    public decimal PesoKg { get; set; }
    public decimal Alto { get; set; }
    public decimal Ancho { get; set; }
    public decimal Largo { get; set; }
    public decimal ValorDeclarado { get; set; }

    public Pedido Pedido { get; set; } = null!;
}
