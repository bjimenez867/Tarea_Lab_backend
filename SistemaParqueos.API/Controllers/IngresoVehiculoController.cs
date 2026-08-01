using Microsoft.AspNetCore.Mvc;
using SistemaParqueos.Dominio.DTO;
using SistemaParqueos.Dominio.InterfazLN;
using SistemaParqueos.Utilidades;

namespace SistemaParqueos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngresoVehiculoController : ControllerBase
{
    private readonly IIngresoVehiculoLN _ingresoLN;

    public IngresoVehiculoController(IIngresoVehiculoLN ingresoLN)
    {
        _ingresoLN = ingresoLN;
    }

    [HttpGet]
    public async Task<ActionResult<Respuesta<IEnumerable<IngresoVehiculoDTO>>>> ObtenerTodos()
    {
        var ingresos = await _ingresoLN.ObtenerTodosAsync();
        return Ok(Respuesta<IEnumerable<IngresoVehiculoDTO>>.Ok(ingresos));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Respuesta<IngresoVehiculoDTO>>> ObtenerPorId(int id)
    {
        var ingreso = await _ingresoLN.ObtenerPorIdAsync(id);
        if (ingreso is null)
            return NotFound(Respuesta<IngresoVehiculoDTO>.Error("Ingreso no encontrado."));

        return Ok(Respuesta<IngresoVehiculoDTO>.Ok(ingreso));
    }

    [HttpPost]
    public async Task<ActionResult<Respuesta<IngresoVehiculoDTO>>> Crear([FromBody] IngresoVehiculoDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(Respuesta<IngresoVehiculoDTO>.Error("Datos inválidos."));

        var creado = await _ingresoLN.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.IngresoId },
            Respuesta<IngresoVehiculoDTO>.Ok(creado, "Ingreso registrado correctamente."));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Respuesta<object>>> Actualizar(int id, [FromBody] IngresoVehiculoDTO dto)
    {
        if (id != dto.IngresoId)
            return BadRequest(Respuesta<object>.Error("El ID de la ruta no coincide con el del cuerpo."));

        if (!ModelState.IsValid)
            return BadRequest(Respuesta<object>.Error("Datos inválidos."));

        await _ingresoLN.ActualizarAsync(dto);
        return Ok(Respuesta<object>.Ok(null!, "Ingreso actualizado correctamente."));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Respuesta<object>>> Eliminar(int id)
    {
        await _ingresoLN.EliminarAsync(id);
        return Ok(Respuesta<object>.Ok(null!, "Ingreso eliminado correctamente."));
    }

    [HttpPut("{id}/salida")]
    public async Task<ActionResult<Respuesta<object>>> RegistrarSalida(int id)
    {
        await _ingresoLN.RegistrarSalidaAsync(id);
        return Ok(Respuesta<object>.Ok(null!, "Salida registrada correctamente. Espacio liberado."));
    }
}