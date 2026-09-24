using Microsoft.EntityFrameworkCore;
using Routify.Auth.Models;

namespace Routify.Auth.Data;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(u => u.UsuarioId);

            entity.Property(u => u.Nombre).HasMaxLength(80).IsRequired();
            entity.Property(u => u.Apellido).HasMaxLength(80).IsRequired();
            entity.Property(u => u.NombreUsuario).HasMaxLength(50).IsRequired();
            entity.Property(u => u.Email).HasMaxLength(150).IsRequired();
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Rol).HasMaxLength(30).IsRequired();

            // Ni el "Id de usuario" del login ni el email pueden repetirse
            entity.HasIndex(u => u.NombreUsuario).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
        });
    }
}
