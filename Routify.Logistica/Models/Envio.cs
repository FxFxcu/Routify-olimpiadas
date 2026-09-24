namespace Routify.Logistica.Models;

public enum EstadoEnvio { EnPreparacion, EnTransito, Entregado, Fallido }

public class Envio
{
    public int EnvioId { get; set; }
    public int PedidoId { get; set; }

    public int DireccionOrigenId { get; set; }
    public int DireccionDestinoId { get; set; }

    // Nullable hasta que alguien lo asigna desde la pantalla de Envíos
    public int? VehiculoId { get; set; }

    // Referencia "blanda" al Usuario del servicio Auth — no es una FK real
    // de base de datos porque Auth vive en otra base
    public Guid? RepartidorUsuarioId { get; set; }

    public DateTime FechaEnvio { get; set; } = DateTime.UtcNow;
    public DateTime? FechaEntregaEstimada { get; set; }
    public decimal? CostoEnvio { get; set; }
    public EstadoEnvio EstadoActual { get; set; } = EstadoEnvio.EnPreparacion;

    public Pedido Pedido { get; set; } = null!;
    public Direccion DireccionOrigen { get; set; } = null!;
    public Direccion DireccionDestino { get; set; } = null!;
    public Vehiculo? Vehiculo { get; set; }

    public ICollection<HistorialEstadoEnvio> Historial { get; set; } = new List<HistorialEstadoEnvio>();
}
