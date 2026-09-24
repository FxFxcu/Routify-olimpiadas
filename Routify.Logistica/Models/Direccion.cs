namespace Routify.Logistica.Models;

public class Direccion
{
    public int DireccionId { get; set; }

    // Nullable: puede ser la dirección de un cliente, o una dirección propia
    // (ej. el depósito) que no pertenece a ningún cliente
    public int? ClienteId { get; set; }

    public string Calle { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public string Provincia { get; set; } = string.Empty;
    public string CodigoPostal { get; set; } = string.Empty;

    // Para la integración con el servicio de mapas/rutas
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }

    public Cliente? Cliente { get; set; }
}
