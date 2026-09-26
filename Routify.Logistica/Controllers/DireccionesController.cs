using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routify.Logistica.Data;
using Routify.Logistica.Models;
using Routify.Shared.Dtos;

namespace Routify.Logistica.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DireccionesController : ControllerBase
{
    private readonly LogisticaDbContext _context;

    public DireccionesController(LogisticaDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DireccionDto>>> GetAll([FromQuery] int? clienteId)
    {
        var query = _context.Direcciones.Include(d => d.Cliente).AsQueryable();

        if (clienteId.HasValue)
        {
            query = query.Where(d => d.ClienteId == clienteId.Value);
        }

        var direcciones = await query
            .Select(d => new DireccionDto
            {
                DireccionId = d.DireccionId,
                ClienteId = d.ClienteId,
                ClienteNombreCompleto = d.Cliente != null ? d.Cliente.Nombre + " " + d.Cliente.Apellido : null,
                Calle = d.Calle,
                Numero = d.Numero,
                Ciudad = d.Ciudad,
                Provincia = d.Provincia,
                CodigoPostal = d.CodigoPostal,
                Latitud = d.Latitud,
                Longitud = d.Longitud
            })
            .ToListAsync();

        return Ok(direcciones);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DireccionDto>> GetById(int id)
    {
        var direccion = await _context.Direcciones
            .Include(d => d.Cliente)
            .Where(d => d.DireccionId == id)
            .Select(d => new DireccionDto
            {
                DireccionId = d.DireccionId,
                ClienteId = d.ClienteId,
                ClienteNombreCompleto = d.Cliente != null ? d.Cliente.Nombre + " " + d.Cliente.Apellido : null,
                Calle = d.Calle,
                Numero = d.Numero,
                Ciudad = d.Ciudad,
                Provincia = d.Provincia,
                CodigoPostal = d.CodigoPostal,
                Latitud = d.Latitud,
                Longitud = d.Longitud
            })
            .FirstOrDefaultAsync();

        if (direccion is null) return NotFound();
        return Ok(direccion);
    }

    [HttpPost]
    public async Task<ActionResult<DireccionDto>> Create(DireccionRequestDto request)
    {
        if (request.ClienteId.HasValue)
        {
            var existeCliente = await _context.Clientes.AnyAsync(c => c.ClienteId == request.ClienteId.Value);
            if (!existeCliente) return BadRequest("El cliente indicado no existe.");
        }

        var direccion = new Direccion
        {
            ClienteId = request.ClienteId,
            Calle = request.Calle,
            Numero = request.Numero,
            Ciudad = request.Ciudad,
            Provincia = request.Provincia,
            CodigoPostal = request.CodigoPostal,
            Latitud = request.Latitud,
            Longitud = request.Longitud
        };

        _context.Direcciones.Add(direccion);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = direccion.DireccionId }, direccion);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, DireccionRequestDto request)
    {
        var direccion = await _context.Direcciones.FindAsync(id);
        if (direccion is null) return NotFound();

        if (request.ClienteId.HasValue)
        {
            var existeCliente = await _context.Clientes.AnyAsync(c => c.ClienteId == request.ClienteId.Value);
            if (!existeCliente) return BadRequest("El cliente indicado no existe.");
        }

        direccion.ClienteId = request.ClienteId;
        direccion.Calle = request.Calle;
        direccion.Numero = request.Numero;
        direccion.Ciudad = request.Ciudad;
        direccion.Provincia = request.Provincia;
        direccion.CodigoPostal = request.CodigoPostal;
        direccion.Latitud = request.Latitud;
        direccion.Longitud = request.Longitud;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var direccion = await _context.Direcciones.FindAsync(id);
        if (direccion is null) return NotFound();

        _context.Direcciones.Remove(direccion);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}