using Microsoft.AspNetCore.Mvc;
using SistemaParqueos.Dominio.DTO;
using SistemaParqueos.Dominio.InterfazLN;
using SistemaParqueos.Utilitarios;

namespace SistemaParqueos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TarifaController : ControllerBase
{
    private readonly ITarifaLN _tarifaLN;

    public TarifaController(ITarifaLN tarifaLN)
    {
        _tarifaLN = tarifaLN;
    }

    [HttpGet]
    public async Task<ActionResult<Respuesta<IEnumerable<TarifaDTO>>>> ObtenerTodos()
    {
        var tarifas = await _tarifaLN.ObtenerTodosAsync();
        return Ok(Respuesta<IEnumerable<TarifaDTO>>.Ok(tarifas));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Respuesta<TarifaDTO>>> ObtenerPorId(int id)
    {
        var tarifa = await _tarifaLN.ObtenerPorIdAsync(id);
        if (tarifa is null)
            return NotFound(Respuesta<TarifaDTO>.Error("Tarifa no encontrada."));

        return Ok(Respuesta<TarifaDTO>.Ok(tarifa));
    }

    [HttpPost]
    public async Task<ActionResult<Respuesta<TarifaDTO>>> Crear([FromBody] TarifaDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(Respuesta<TarifaDTO>.Error("Datos inválidos."));

        var creada = await _tarifaLN.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.TarifaId },
            Respuesta<TarifaDTO>.Ok(creada, "Tarifa creada correctamente."));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Respuesta<object>>> Actualizar(int id, [FromBody] TarifaDTO dto)
    {
        if (id != dto.TarifaId)
            return BadRequest(Respuesta<object>.Error("El ID de la ruta no coincide con el del cuerpo."));

        if (!ModelState.IsValid)
            return BadRequest(Respuesta<object>.Error("Datos inválidos."));

        await _tarifaLN.ActualizarAsync(dto);
        return Ok(Respuesta<object>.Ok(null!, "Tarifa actualizada correctamente."));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Respuesta<object>>> Eliminar(int id)
    {
        await _tarifaLN.EliminarAsync(id);
        return Ok(Respuesta<object>.Ok(null!, "Tarifa eliminada correctamente."));
    }
}