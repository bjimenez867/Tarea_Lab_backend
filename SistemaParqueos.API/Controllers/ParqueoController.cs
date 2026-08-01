using Microsoft.AspNetCore.Mvc;
using SistemaParqueos.Dominio.DTO;
using SistemaParqueos.Dominio.InterfazLN;
using SistemaParqueos.Utilitarios;

namespace SistemaParqueos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParqueoController : ControllerBase
{
    private readonly IParqueoLN _parqueoLN;

    public ParqueoController(IParqueoLN parqueoLN)
    {
        _parqueoLN = parqueoLN;
    }

    [HttpGet]
    public async Task<ActionResult<Respuesta<IEnumerable<ParqueoDTO>>>> ObtenerTodos()
    {
        var parqueos = await _parqueoLN.ObtenerTodosAsync();
        return Ok(Respuesta<IEnumerable<ParqueoDTO>>.Ok(parqueos));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Respuesta<ParqueoDTO>>> ObtenerPorId(int id)
    {
        var parqueo = await _parqueoLN.ObtenerPorIdAsync(id);
        if (parqueo is null)
            return NotFound(Respuesta<ParqueoDTO>.Error("Parqueo no encontrado."));

        return Ok(Respuesta<ParqueoDTO>.Ok(parqueo));
    }

    [HttpPost]
    public async Task<ActionResult<Respuesta<ParqueoDTO>>> Crear([FromBody] ParqueoDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(Respuesta<ParqueoDTO>.Error("Datos inválidos."));

        var creado = await _parqueoLN.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.ParqueoId },
            Respuesta<ParqueoDTO>.Ok(creado, "Parqueo creado correctamente."));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Respuesta<object>>> Actualizar(int id, [FromBody] ParqueoDTO dto)
    {
        if (id != dto.ParqueoId)
            return BadRequest(Respuesta<object>.Error("El ID de la ruta no coincide con el del cuerpo."));

        if (!ModelState.IsValid)
            return BadRequest(Respuesta<object>.Error("Datos inválidos."));

        await _parqueoLN.ActualizarAsync(dto);
        return Ok(Respuesta<object>.Ok(null!, "Parqueo actualizado correctamente."));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Respuesta<object>>> Eliminar(int id)
    {
        await _parqueoLN.EliminarAsync(id);
        return Ok(Respuesta<object>.Ok(null!, "Parqueo eliminado correctamente."));
    }
}