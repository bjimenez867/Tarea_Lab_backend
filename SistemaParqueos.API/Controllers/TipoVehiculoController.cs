using Microsoft.AspNetCore.Mvc;
using SistemaParqueos.Dominio.DTO;
using SistemaParqueos.Dominio.InterfazLN;
using SistemaParqueos.Utilitarios;

namespace SistemaParqueos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TipoVehiculoController : ControllerBase
{
    private readonly ITipoVehiculoLN _tipoVehiculoLN;

    public TipoVehiculoController(ITipoVehiculoLN tipoVehiculoLN)
    {
        _tipoVehiculoLN = tipoVehiculoLN;
    }

    [HttpGet]
    public async Task<ActionResult<Respuesta<IEnumerable<TipoVehiculoDTO>>>> ObtenerTodos()
    {
        var tipos = await _tipoVehiculoLN.ObtenerTodosAsync();
        return Ok(Respuesta<IEnumerable<TipoVehiculoDTO>>.Ok(tipos));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Respuesta<TipoVehiculoDTO>>> ObtenerPorId(int id)
    {
        var tipo = await _tipoVehiculoLN.ObtenerPorIdAsync(id);
        if (tipo is null)
            return NotFound(Respuesta<TipoVehiculoDTO>.Error("Tipo de vehículo no encontrado."));

        return Ok(Respuesta<TipoVehiculoDTO>.Ok(tipo));
    }

    [HttpPost]
    public async Task<ActionResult<Respuesta<TipoVehiculoDTO>>> Crear([FromBody] TipoVehiculoDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(Respuesta<TipoVehiculoDTO>.Error("Datos inválidos."));

        var creado = await _tipoVehiculoLN.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.TipoVehiculoId },
            Respuesta<TipoVehiculoDTO>.Ok(creado, "Tipo de vehículo creado correctamente."));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Respuesta<object>>> Actualizar(int id, [FromBody] TipoVehiculoDTO dto)
    {
        if (id != dto.TipoVehiculoId)
            return BadRequest(Respuesta<object>.Error("El ID de la ruta no coincide con el del cuerpo."));

        if (!ModelState.IsValid)
            return BadRequest(Respuesta<object>.Error("Datos inválidos."));

        await _tipoVehiculoLN.ActualizarAsync(dto);
        return Ok(Respuesta<object>.Ok(null!, "Tipo de vehículo actualizado correctamente."));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Respuesta<object>>> Eliminar(int id)
    {
        await _tipoVehiculoLN.EliminarAsync(id);
        return Ok(Respuesta<object>.Ok(null!, "Tipo de vehículo eliminado correctamente."));
    }
}