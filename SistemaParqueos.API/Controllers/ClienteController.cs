using Microsoft.AspNetCore.Mvc;
using SistemaParqueos.Dominio.DTO;
using SistemaParqueos.Dominio.InterfazLN;
using SistemaParqueos.Utilidades;

namespace SistemaParqueos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly IClienteLN _clienteLN;

    public ClienteController(IClienteLN clienteLN)
    {
        _clienteLN = clienteLN;
    }

    [HttpGet]
    public async Task<ActionResult<Respuesta<IEnumerable<ClienteDTO>>>> ObtenerTodos()
    {
        var clientes = await _clienteLN.ObtenerTodosAsync();
        return Ok(Respuesta<IEnumerable<ClienteDTO>>.Ok(clientes));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Respuesta<ClienteDTO>>> ObtenerPorId(int id)
    {
        var cliente = await _clienteLN.ObtenerPorIdAsync(id);
        if (cliente is null)
            return NotFound(Respuesta<ClienteDTO>.Error("Cliente no encontrado."));

        return Ok(Respuesta<ClienteDTO>.Ok(cliente));
    }

    [HttpPost]
    public async Task<ActionResult<Respuesta<ClienteDTO>>> Crear([FromBody] ClienteDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(Respuesta<ClienteDTO>.Error("Datos inválidos."));

        var creado = await _clienteLN.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.ClienteId },
            Respuesta<ClienteDTO>.Ok(creado, "Cliente creado correctamente."));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Respuesta<object>>> Actualizar(int id, [FromBody] ClienteDTO dto)
    {
        if (id != dto.ClienteId)
            return BadRequest(Respuesta<object>.Error("El ID de la ruta no coincide con el del cuerpo."));

        if (!ModelState.IsValid)
            return BadRequest(Respuesta<object>.Error("Datos inválidos."));

        await _clienteLN.ActualizarAsync(dto);
        return Ok(Respuesta<object>.Ok(null!, "Cliente actualizado correctamente."));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Respuesta<object>>> Eliminar(int id)
    {
        await _clienteLN.EliminarAsync(id);
        return Ok(Respuesta<object>.Ok(null!, "Cliente eliminado correctamente."));
    }
}