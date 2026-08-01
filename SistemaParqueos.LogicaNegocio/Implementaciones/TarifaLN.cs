using SistemaParqueos.Dominio.DTO;
using SistemaParqueos.Dominio.Entidades;
using SistemaParqueos.Dominio.InterfacesAD;
using SistemaParqueos.Dominio.InterfazLN;
using SistemaParqueos.Utilidades;

namespace SistemaParqueos.LogicaNegocio.Implementaciones;

public class TarifaLN : ITarifaLN
{
    private readonly IUnitOfWork _unitOfWork;

    public TarifaLN(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TarifaDTO>> ObtenerTodosAsync()
    {
        var tarifas = await _unitOfWork.Tarifas.ObtenerTodosAsync();
        return tarifas.Select(MapearADto);
    }

    public async Task<TarifaDTO?> ObtenerPorIdAsync(int id)
    {
        var tarifa = await _unitOfWork.Tarifas.ObtenerPorIdAsync(id);
        return tarifa is null ? null : MapearADto(tarifa);
    }

    public async Task<TarifaDTO> CrearAsync(TarifaDTO dto)
    {
        if (dto.MontoHora <= 0)
            throw new ValidacionNegocioException("El monto por hora debe ser mayor a cero.");

        var tipo = await _unitOfWork.TiposVehiculo.ObtenerPorIdAsync(dto.TipoVehiculoId)
            ?? throw new ValidacionNegocioException("El tipo de vehículo indicado no existe.");

        var tarifa = new Tarifa
        {
            TipoVehiculoId = dto.TipoVehiculoId,
            Descripcion = dto.Descripcion,
            MontoHora = dto.MontoHora,
            Activo = dto.Activo,
            CreadoEn = DateTime.UtcNow
        };

        await _unitOfWork.Tarifas.AgregarAsync(tarifa);
        await _unitOfWork.GuardarCambiosAsync();

        dto.TarifaId = tarifa.TarifaId;
        return dto;
    }

    public async Task ActualizarAsync(TarifaDTO dto)
    {
        if (dto.MontoHora <= 0)
            throw new ValidacionNegocioException("El monto por hora debe ser mayor a cero.");

        var tarifa = await _unitOfWork.Tarifas.ObtenerPorIdAsync(dto.TarifaId)
            ?? throw new ValidacionNegocioException("La tarifa no existe.");

        tarifa.TipoVehiculoId = dto.TipoVehiculoId;
        tarifa.Descripcion = dto.Descripcion;
        tarifa.MontoHora = dto.MontoHora;
        tarifa.Activo = dto.Activo;
        tarifa.ActualizadoEn = DateTime.UtcNow;

        _unitOfWork.Tarifas.Actualizar(tarifa);
        await _unitOfWork.GuardarCambiosAsync();
    }

    public async Task EliminarAsync(int id)
    {
        var tarifa = await _unitOfWork.Tarifas.ObtenerPorIdAsync(id)
            ?? throw new ValidacionNegocioException("La tarifa no existe.");

        _unitOfWork.Tarifas.Eliminar(tarifa);
        await _unitOfWork.GuardarCambiosAsync();
    }

    private static TarifaDTO MapearADto(Tarifa t) => new()
    {
        TarifaId = t.TarifaId,
        TipoVehiculoId = t.TipoVehiculoId,
        Descripcion = t.Descripcion,
        MontoHora = t.MontoHora,
        Activo = t.Activo
    };
}