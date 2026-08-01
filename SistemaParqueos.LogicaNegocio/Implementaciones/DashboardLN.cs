using SistemaParqueos.Dominio.DTO;
using SistemaParqueos.Dominio.InterfacesAD;
using SistemaParqueos.Dominio.InterfazLN;

namespace SistemaParqueos.LogicaNegocio.Implementaciones;

public class DashboardLN : IDashboardLN
{
    private readonly IUnitOfWork _unitOfWork;

    public DashboardLN(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DashboardDTO> ObtenerIndicadoresAsync()
    {
        var hoy = DateTime.UtcNow.Date;
        var inicioMes = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

        var ingresos = await _unitOfWork.IngresosVehiculo.ObtenerTodosAsync();
        var espacios = await _unitOfWork.EspaciosParqueo.ObtenerTodosAsync();
        var facturas = await _unitOfWork.Facturas.ObtenerTodosAsync();

        var vehiculosHoy = ingresos.Count(i => i.FechaIngreso.Date == hoy);
        var espaciosDisponibles = espacios.Count(e => e.Disponible && e.Activo);
        var facturacionDiaria = facturas
            .Where(f => f.FechaFactura.Date == hoy)
            .Sum(f => f.MontoTotal);
        var facturacionMensual = facturas
            .Where(f => f.FechaFactura.Date >= inicioMes)
            .Sum(f => f.MontoTotal);

        return new DashboardDTO
        {
            VehiculosIngresadosHoy = vehiculosHoy,
            EspaciosDisponibles = espaciosDisponibles,
            FacturacionDiaria = facturacionDiaria,
            FacturacionMensual = facturacionMensual
        };
    }
}