using Microsoft.AspNetCore.Mvc;
using SistemaParqueos.Dominio.DTO;
using SistemaParqueos.Dominio.InterfazLN;
using SistemaParqueos.Utilidades;

namespace SistemaParqueos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EspacioParqueoController : ControllerBase
{
    private readonly IEspacioParqueoLN _espacioParqueoLN;

    public EspacioParqueoController(IEspacioParqueoLN espacioParqueoLN)
    {
        _espacioParqueoLN = espacioParqueoLN;
    }

    [HttpGet]
    public async Task<ActionResult<Respuesta<IEnumerable<EspacioParqueoDTO>>>> ObtenerTodos()
    {
        var espacios = await _espacioParqueoLN.ObtenerTodosAsync();
        return Ok(Respuesta<IEnumerable<EspacioParqueoDTO>>.Ok(espacios));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Respuesta<EspacioParqueoDTO>>> ObtenerPorId(int id)
    {
        var espacio = await _espacioParqueoLN.ObtenerPorIdAsync(id);
        if (espacio is null)
            return NotFound(Respuesta<EspacioParqueoDTO>.Error("Espacio no encontrado."));

        return Ok(Respuesta<EspacioParqueoDTO>.Ok(espacio));
    }

    [HttpPost]
    public async Task<ActionResult<Respuesta<EspacioParqueoDTO>>> Crear([FromBody] EspacioParqueoDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(Respuesta<EspacioParqueoDTO>.Error("Datos inválidos."));

        var creado = await _espacioParqueoLN.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.EspacioId },
            Respuesta<EspacioParqueoDTO>.Ok(creado, "Espacio creado correctamente."));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Respuesta<object>>> Actualizar(int id, [FromBody] EspacioParqueoDTO dto)
    {
        if (id != dto.EspacioId)
            return BadRequest(Respuesta<object>.Error("El ID de la ruta no coincide con el del cuerpo."));

        if (!ModelState.IsValid)
            return BadRequest(Respuesta<object>.Error("Datos inválidos."));

        await _espacioParqueoLN.ActualizarAsync(dto);
        return Ok(Respuesta<object>.Ok(null!, "Espacio actualizado correctamente."));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Respuesta<object>>> Eliminar(int id)
    {
        await _espacioParqueoLN.EliminarAsync(id);
        return Ok(Respuesta<object>.Ok(null!, "Espacio eliminado correctamente."));
    }
}