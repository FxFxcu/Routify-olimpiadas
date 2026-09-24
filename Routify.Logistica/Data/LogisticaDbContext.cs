using Microsoft.EntityFrameworkCore;
using Routify.Logistica.Models;

namespace Routify.Logistica.Data;

public class LogisticaDbContext : DbContext
{
    public LogisticaDbContext(DbContextOptions<LogisticaDbContext> options) : base(options) { }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Direccion> Direcciones => Set<Direccion>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<Paquete> Paquetes => Set<Paquete>();
    public DbSet<Envio> Envios => Set<Envio>();
    public DbSet<Vehiculo> Vehiculos => Set<Vehiculo>();
    public DbSet<HistorialEstadoEnvio> HistorialEstadosEnvio => Set<HistorialEstadoEnvio>();
    public DbSet<Pago> Pagos => Set<Pago>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ---------- Cliente ----------
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.Property(c => c.Nombre).HasMaxLength(80).IsRequired();
            entity.Property(c => c.Apellido).HasMaxLength(80).IsRequired();
            entity.Property(c => c.Email).HasMaxLength(150).IsRequired();
            entity.HasIndex(c => c.Email).IsUnique();

            entity.HasMany(c => c.Direcciones)
                  .WithOne(d => d.Cliente)
                  .HasForeignKey(d => d.ClienteId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Restrict: no se puede borrar un cliente que ya tiene pedidos
            // (evita perder el historial por accidente)
            entity.HasMany(c => c.Pedidos)
                  .WithOne(p => p.Cliente)
                  .HasForeignKey(p => p.ClienteId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ---------- Direccion ----------
        modelBuilder.Entity<Direccion>(entity =>
        {
            entity.Property(d => d.Calle).HasMaxLength(120).IsRequired();
            entity.Property(d => d.Ciudad).HasMaxLength(80).IsRequired();
            entity.Property(d => d.Provincia).HasMaxLength(80).IsRequired();
            entity.Property(d => d.CodigoPostal).HasMaxLength(15);
        });

        // ---------- Pedido ----------
        modelBuilder.Entity<Pedido>(entity =>
        {
            // Se guarda como texto ("Pendiente", "Confirmado"...) en vez de un número,
            // así se puede leer directo en DBeaver sin tener que recordar qué es cada valor
            entity.Property(p => p.Estado).HasConversion<string>().HasMaxLength(20);

            entity.HasMany(p => p.Paquetes)
                  .WithOne(pa => pa.Pedido)
                  .HasForeignKey(pa => pa.PedidoId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.Envio)
                  .WithOne(e => e.Pedido)
                  .HasForeignKey<Envio>(e => e.PedidoId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Pago)
                  .WithOne(pa => pa.Pedido)
                  .HasForeignKey<Pago>(pa => pa.PedidoId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ---------- Vehiculo ----------
        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.Property(v => v.Patente).HasMaxLength(10).IsRequired();
            entity.HasIndex(v => v.Patente).IsUnique();
            entity.Property(v => v.Estado).HasConversion<string>().HasMaxLength(20);
        });

        // ---------- Envio ----------
        modelBuilder.Entity<Envio>(entity =>
        {
            entity.Property(e => e.EstadoActual).HasConversion<string>().HasMaxLength(20);

            // Envio tiene DOS foreign keys que apuntan a Direccion (origen y destino).
            // EF Core no puede adivinar solo cuál navegación va con cuál FK,
            // así que hay que decírselo a mano con HasForeignKey — si no, tira
            // una excepción al arrancar la app.
            entity.HasOne(e => e.DireccionOrigen)
                  .WithMany()
                  .HasForeignKey(e => e.DireccionOrigenId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.DireccionDestino)
                  .WithMany()
                  .HasForeignKey(e => e.DireccionDestinoId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Restrict acá es el respaldo a nivel base de datos de la regla que
            // ya habíamos hablado: no se puede borrar un vehículo con envíos.
            // Igual conviene chequearlo antes en el servicio, para devolver un
            // error claro en vez de que explote la excepción de Postgres.
            entity.HasOne(e => e.Vehiculo)
                  .WithMany(v => v.Envios)
                  .HasForeignKey(e => e.VehiculoId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Historial)
                  .WithOne(h => h.Envio)
                  .HasForeignKey(h => h.EnvioId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ---------- HistorialEstadoEnvio ----------
        modelBuilder.Entity<HistorialEstadoEnvio>(entity =>
        {
            entity.HasKey(h => h.HistorialId);
            entity.Property(h => h.Estado).HasConversion<string>().HasMaxLength(20);
        });

        // ---------- Pago ----------
        modelBuilder.Entity<Pago>(entity =>
        {
            entity.Property(p => p.MedioPago).HasMaxLength(40);
            entity.Property(p => p.Estado).HasConversion<string>().HasMaxLength(20);
        });
    }
}
