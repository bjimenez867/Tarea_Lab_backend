using Microsoft.AspNetCore.Mvc;
using SistemaParqueos.Dominio.InterfazLN;
using SistemaParqueos.Utilidades;

namespace SistemaParqueos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardLN _dashboardLN;

    public DashboardController(IDashboardLN dashboardLN)
    {
        _dashboardLN = dashboardLN;
    }

    [HttpGet]
    public async Task<ActionResult<Respuesta<object>>> ObtenerIndicadores()
    {
        var indicadores = await _dashboardLN.ObtenerIndicadoresAsync();
        return Ok(Respuesta<object>.Ok(indicadores));
    }
}