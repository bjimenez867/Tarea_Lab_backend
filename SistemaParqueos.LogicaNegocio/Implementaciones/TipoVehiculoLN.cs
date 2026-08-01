using SistemaParqueos.Dominio.DTO;
using SistemaParqueos.Dominio.Entidades;
using SistemaParqueos.Dominio.InterfacesAD;
using SistemaParqueos.Dominio.InterfazLN;
using SistemaParqueos.Utilidades;

namespace SistemaParqueos.LogicaNegocio.Implementaciones;

public class TipoVehiculoLN : ITipoVehiculoLN
{
    private readonly IUnitOfWork _unitOfWork;

    public TipoVehiculoLN(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TipoVehiculoDTO>> ObtenerTodosAsync()
    {
        var tipos = await _unitOfWork.TiposVehiculo.ObtenerTodosAsync();
        return tipos.Select(MapearADto);
    }

    public async Task<TipoVehiculoDTO?> ObtenerPorIdAsync(int id)
    {
        var tipo = await _unitOfWork.TiposVehiculo.ObtenerPorIdAsync(id);
        return tipo is null ? null : MapearADto(tipo);
    }

    public async Task<TipoVehiculoDTO> CrearAsync(TipoVehiculoDTO dto)
    {
        var existente = await _unitOfWork.TiposVehiculo.BuscarAsync(t => t.Descripcion == dto.Descripcion);
        if (existente.Any())
            throw new ValidacionNegocioException("Ya existe un tipo de vehículo con esa descripción.");

        var tipo = new TipoVehiculo
        {
            Descripcion = dto.Descripcion,
            Activo = dto.Activo,
            CreadoEn = DateTime.UtcNow
        };

        await _unitOfWork.TiposVehiculo.AgregarAsync(tipo);
        await _unitOfWork.GuardarCambiosAsync();

        dto.TipoVehiculoId = tipo.TipoVehiculoId;
        return dto;
    }

    public async Task ActualizarAsync(TipoVehiculoDTO dto)
    {
        var tipo = await _unitOfWork.TiposVehiculo.ObtenerPorIdAsync(dto.TipoVehiculoId)
            ?? throw new ValidacionNegocioException("El tipo de vehículo no existe.");

        var duplicado = await _unitOfWork.TiposVehiculo.BuscarAsync(
            t => t.Descripcion == dto.Descripcion && t.TipoVehiculoId != dto.TipoVehiculoId);
        if (duplicado.Any())
            throw new ValidacionNegocioException("Ya existe otro tipo de vehículo con esa descripción.");

        tipo.Descripcion = dto.Descripcion;
        tipo.Activo = dto.Activo;
        tipo.ActualizadoEn = DateTime.UtcNow;

        _unitOfWork.TiposVehiculo.Actualizar(tipo);
        await _unitOfWork.GuardarCambiosAsync();
    }

    public async Task EliminarAsync(int id)
    {
        var tipo = await _unitOfWork.TiposVehiculo.ObtenerPorIdAsync(id)
            ?? throw new ValidacionNegocioException("El tipo de vehículo no existe.");

        _unitOfWork.TiposVehiculo.Eliminar(tipo);
        await _unitOfWork.GuardarCambiosAsync();
    }

    private static TipoVehiculoDTO MapearADto(TipoVehiculo t) => new()
    {
        TipoVehiculoId = t.TipoVehiculoId,
        Descripcion = t.Descripcion,
        Activo = t.Activo
    };
}