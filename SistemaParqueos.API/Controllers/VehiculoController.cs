using Microsoft.AspNetCore.Mvc;
using SistemaParqueos.Dominio.DTO;
using SistemaParqueos.Dominio.InterfazLN;
using SistemaParqueos.Utilidades;

namespace SistemaParqueos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiculoController : ControllerBase
{
    private readonly IVehiculoLN _vehiculoLN;

    public VehiculoController(IVehiculoLN vehiculoLN)
    {
        _vehiculoLN = vehiculoLN;
    }

    [HttpGet]
    public async Task<ActionResult<Respuesta<IEnumerable<VehiculoDTO>>>> ObtenerTodos()
    {
        var vehiculos = await _vehiculoLN.ObtenerTodosAsync();
        return Ok(Respuesta<IEnumerable<VehiculoDTO>>.Ok(vehiculos));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Respuesta<VehiculoDTO>>> ObtenerPorId(int id)
    {
        var vehiculo = await _vehiculoLN.ObtenerPorIdAsync(id);
        if (vehiculo is null)
            return NotFound(Respuesta<VehiculoDTO>.Error("Vehículo no encontrado."));

        return Ok(Respuesta<VehiculoDTO>.Ok(vehiculo));
    }

    [HttpPost]
    public async Task<ActionResult<Respuesta<VehiculoDTO>>> Crear([FromBody] VehiculoDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(Respuesta<VehiculoDTO>.Error("Datos inválidos."));

        var creado = await _vehiculoLN.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.VehiculoId },
            Respuesta<VehiculoDTO>.Ok(creado, "Vehículo creado correctamente."));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Respuesta<object>>> Actualizar(int id, [FromBody] VehiculoDTO dto)
    {
        if (id != dto.VehiculoId)
            return BadRequest(Respuesta<object>.Error("El ID de la ruta no coincide con el del cuerpo."));

        if (!ModelState.IsValid)
            return BadRequest(Respuesta<object>.Error("Datos inválidos."));

        await _vehiculoLN.ActualizarAsync(dto);
        return Ok(Respuesta<object>.Ok(null!, "Vehículo actualizado correctamente."));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Respuesta<object>>> Eliminar(int id)
    {
        await _vehiculoLN.EliminarAsync(id);
        return Ok(Respuesta<object>.Ok(null!, "Vehículo eliminado correctamente."));
    }
}