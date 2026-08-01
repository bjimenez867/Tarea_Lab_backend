using SistemaParqueos.Dominio.DTO;
using SistemaParqueos.Dominio.Entidades;
using SistemaParqueos.Dominio.InterfacesAD;
using SistemaParqueos.Dominio.InterfazLN;
using SistemaParqueos.Utilidades;

namespace SistemaParqueos.LogicaNegocio.Implementaciones;

public class FacturaLN : IFacturaLN
{
    private readonly IUnitOfWork _unitOfWork;

    public FacturaLN(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<FacturaDTO>> ObtenerTodosAsync()
    {
        var facturas = await _unitOfWork.Facturas.ObtenerTodosAsync();
        return facturas.Select(MapearADto);
    }

    public async Task<FacturaDTO?> ObtenerPorIdAsync(int id)
    {
        var factura = await _unitOfWork.Facturas.ObtenerPorIdAsync(id);
        return factura is null ? null : MapearADto(factura);
    }

    public async Task<FacturaDTO> CrearAsync(FacturaDTO dto)
    {
        if (dto.MontoTotal < 0)
            throw new ValidacionNegocioException("El monto total no puede ser negativo.");

        var ingreso = await _unitOfWork.IngresosVehiculo.ObtenerPorIdAsync(dto.IngresoId)
            ?? throw new ValidacionNegocioException("El ingreso indicado no existe.");

        var factura = new Factura
        {
            IngresoId = dto.IngresoId,
            FechaFactura = DateTime.UtcNow,
            HorasCobradas = dto.HorasCobradas,
            MontoTotal = dto.MontoTotal,
            CreadoEn = DateTime.UtcNow
        };

        await _unitOfWork.Facturas.AgregarAsync(factura);
        await _unitOfWork.GuardarCambiosAsync();

        dto.FacturaId = factura.FacturaId;
        dto.FechaFactura = factura.FechaFactura;
        return dto;
    }

    public async Task ActualizarAsync(FacturaDTO dto)
    {
        if (dto.MontoTotal < 0)
            throw new ValidacionNegocioException("El monto total no puede ser negativo.");

        var factura = await _unitOfWork.Facturas.ObtenerPorIdAsync(dto.FacturaId)
            ?? throw new ValidacionNegocioException("La factura no existe.");

        factura.HorasCobradas = dto.HorasCobradas;
        factura.MontoTotal = dto.MontoTotal;
        factura.ActualizadoEn = DateTime.UtcNow;

        _unitOfWork.Facturas.Actualizar(factura);
        await _unitOfWork.GuardarCambiosAsync();
    }

    public async Task EliminarAsync(int id)
    {
        var factura = await _unitOfWork.Facturas.ObtenerPorIdAsync(id)
            ?? throw new ValidacionNegocioException("La factura no existe.");

        _unitOfWork.Facturas.Eliminar(factura);
        await _unitOfWork.GuardarCambiosAsync();
    }

    private static FacturaDTO MapearADto(Factura f) => new()
    {
        FacturaId = f.FacturaId,
        IngresoId = f.IngresoId,
        FechaFactura = f.FechaFactura,
        HorasCobradas = f.HorasCobradas,
        MontoTotal = f.MontoTotal
    };
}