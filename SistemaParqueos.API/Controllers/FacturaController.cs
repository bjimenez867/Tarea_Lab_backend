using Microsoft.AspNetCore.Mvc;
using SistemaParqueos.Dominio.DTO;
using SistemaParqueos.Dominio.InterfazLN;
using SistemaParqueos.Utilidades;

namespace SistemaParqueos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FacturaController : ControllerBase
{
    private readonly IFacturaLN _facturaLN;

    public FacturaController(IFacturaLN facturaLN)
    {
        _facturaLN = facturaLN;
    }

    [HttpGet]
    public async Task<ActionResult<Respuesta<IEnumerable<FacturaDTO>>>> ObtenerTodos()
    {
        var facturas = await _facturaLN.ObtenerTodosAsync();
        return Ok(Respuesta<IEnumerable<FacturaDTO>>.Ok(facturas));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Respuesta<FacturaDTO>>> ObtenerPorId(int id)
    {
        var factura = await _facturaLN.ObtenerPorIdAsync(id);
        if (factura is null)
            return NotFound(Respuesta<FacturaDTO>.Error("Factura no encontrada."));

        return Ok(Respuesta<FacturaDTO>.Ok(factura));
    }

    [HttpPost]
    public async Task<ActionResult<Respuesta<FacturaDTO>>> Crear([FromBody] FacturaDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(Respuesta<FacturaDTO>.Error("Datos inválidos."));

        var creada = await _facturaLN.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.FacturaId },
            Respuesta<FacturaDTO>.Ok(creada, "Factura creada correctamente."));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Respuesta<object>>> Actualizar(int id, [FromBody] FacturaDTO dto)
    {
        if (id != dto.FacturaId)
            return BadRequest(Respuesta<object>.Error("El ID de la ruta no coincide con el del cuerpo."));

        if (!ModelState.IsValid)
            return BadRequest(Respuesta<object>.Error("Datos inválidos."));

        await _facturaLN.ActualizarAsync(dto);
        return Ok(Respuesta<object>.Ok(null!, "Factura actualizada correctamente."));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Respuesta<object>>> Eliminar(int id)
    {
        await _facturaLN.EliminarAsync(id);
        return Ok(Respuesta<object>.Ok(null!, "Factura eliminada correctamente."));
    }
}