using Routify.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routify.Logistica.Data;
using Routify.Logistica.Models;
using Routify.Shared.Dtos;

namespace Routify.Logistica.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly LogisticaDbContext _context;

    public ClientesController(LogisticaDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> GetAll([FromQuery] string? buscar)
    {
        var query = _context.Clientes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            query = query.Where(c =>
                c.Nombre.Contains(buscar) ||
                c.Apellido.Contains(buscar) ||
                c.Email.Contains(buscar));
        }

        var clientes = await query
            .Select(c => new ClienteDto
            {
                ClienteId = c.ClienteId,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Email = c.Email,
                Telefono = c.Telefono,
                FechaRegistro = c.FechaRegistro,
                CantidadPedidos = c.Pedidos.Count
            })
            .ToListAsync();

        return Ok(clientes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClienteDto>> GetById(int id)
    {
        var cliente = await _context.Clientes
            .Where(c => c.ClienteId == id)
            .Select(c => new ClienteDto
            {
                ClienteId = c.ClienteId,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Email = c.Email,
                Telefono = c.Telefono,
                FechaRegistro = c.FechaRegistro,
                CantidadPedidos = c.Pedidos.Count
            })
            .FirstOrDefaultAsync();

        if (cliente is null) return NotFound();
        return Ok(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<ClienteDto>> Create(ClienteRequestDto request)
    {
        var existe = await _context.Clientes.AnyAsync(c => c.Email == request.Email);
        if (existe) return Conflict("Ya existe un cliente con ese email.");

        var cliente = new Cliente
        {
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            Email = request.Email,
            Telefono = request.Telefono
        };

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = cliente.ClienteId }, cliente);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ClienteRequestDto request)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente is null) return NotFound();

        cliente.Nombre = request.Nombre;
        cliente.Apellido = request.Apellido;
        cliente.Email = request.Email;
        cliente.Telefono = request.Telefono;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente is null) return NotFound();

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
