namespace Routify.Logistica.Models;

public class HistorialEstadoEnvio
{
    public int HistorialId { get; set; }
    public int EnvioId { get; set; }

    public EstadoEnvio Estado { get; set; }
    public DateTime FechaHora { get; set; } = DateTime.UtcNow;
    public string? Observaciones { get; set; }

    // Referencia blanda, mismo motivo que en Envio.RepartidorUsuarioId
    public Guid? UsuarioResponsableId { get; set; }

    public Envio Envio { get; set; } = null!;
}
