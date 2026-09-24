namespace Routify.Logistica.Models;

public enum EstadoVehiculo { Disponible, EnRuta, Mantenimiento }

public class Vehiculo
{
    public int VehiculoId { get; set; }
    public string Patente { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty; // Moto, Furgón, Camión
    public decimal CapacidadKg { get; set; }
    public EstadoVehiculo Estado { get; set; } = EstadoVehiculo.Disponible;

    public ICollection<Envio> Envios { get; set; } = new List<Envio>();
}
