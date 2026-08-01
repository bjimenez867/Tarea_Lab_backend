using SistemaParqueos.Dominio.DTO;
using SistemaParqueos.Dominio.Entidades;
using SistemaParqueos.Dominio.InterfacesAD;
using SistemaParqueos.Dominio.InterfazLN;
using SistemaParqueos.Utilitarios;

namespace SistemaParqueos.LogicaNegocio.Implementaciones;

public class VehiculoLN : IVehiculoLN
{
    private readonly IUnitOfWork _unitOfWork;

    public VehiculoLN(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<VehiculoDTO>> ObtenerTodosAsync()
    {
        var vehiculos = await _unitOfWork.Vehiculos.ObtenerTodosAsync();
        return vehiculos.Select(MapearADto);
    }

    public async Task<VehiculoDTO?> ObtenerPorIdAsync(int id)
    {
        var vehiculo = await _unitOfWork.Vehiculos.ObtenerPorIdAsync(id);
        return vehiculo is null ? null : MapearADto(vehiculo);
    }

    public async Task<VehiculoDTO> CrearAsync(VehiculoDTO dto)
    {
        var cliente = await _unitOfWork.Clientes.ObtenerPorIdAsync(dto.ClienteId)
            ?? throw new ValidacionNegocioException("El cliente indicado no existe.");

        var tipo = await _unitOfWork.TiposVehiculo.ObtenerPorIdAsync(dto.TipoVehiculoId)
            ?? throw new ValidacionNegocioException("El tipo de vehículo indicado no existe.");

        var existente = await _unitOfWork.Vehiculos.BuscarAsync(v => v.Placa == dto.Placa);
        if (existente.Any())
            throw new ValidacionNegocioException("Ya existe un vehículo registrado con esa placa.");

        var vehiculo = new Vehiculo
        {
            ClienteId = dto.ClienteId,
            TipoVehiculoId = dto.TipoVehiculoId,
            Placa = dto.Placa,
            Marca = dto.Marca,
            Modelo = dto.Modelo,
            Color = dto.Color,
            Activo = dto.Activo,
            CreadoEn = DateTime.UtcNow
        };

        await _unitOfWork.Vehiculos.AgregarAsync(vehiculo);
        await _unitOfWork.GuardarCambiosAsync();

        dto.VehiculoId = vehiculo.VehiculoId;
        return dto;
    }

    public async Task ActualizarAsync(VehiculoDTO dto)
    {
        var vehiculo = await _unitOfWork.Vehiculos.ObtenerPorIdAsync(dto.VehiculoId)
            ?? throw new ValidacionNegocioException("El vehículo no existe.");

        var duplicado = await _unitOfWork.Vehiculos.BuscarAsync(
            v => v.Placa == dto.Placa && v.VehiculoId != dto.VehiculoId);
        if (duplicado.Any())
            throw new ValidacionNegocioException("Ya existe otro vehículo registrado con esa placa.");

        vehiculo.ClienteId = dto.ClienteId;
        vehiculo.TipoVehiculoId = dto.TipoVehiculoId;
        vehiculo.Placa = dto.Placa;
        vehiculo.Marca = dto.Marca;
        vehiculo.Modelo = dto.Modelo;
        vehiculo.Color = dto.Color;
        vehiculo.Activo = dto.Activo;
        vehiculo.ActualizadoEn = DateTime.UtcNow;

        _unitOfWork.Vehiculos.Actualizar(vehiculo);
        await _unitOfWork.GuardarCambiosAsync();
    }

    public async Task EliminarAsync(int id)
    {
        var vehiculo = await _unitOfWork.Vehiculos.ObtenerPorIdAsync(id)
            ?? throw new ValidacionNegocioException("El vehículo no existe.");

        _unitOfWork.Vehiculos.Eliminar(vehiculo);
        await _unitOfWork.GuardarCambiosAsync();
    }

    private static VehiculoDTO MapearADto(Vehiculo v) => new()
    {
        VehiculoId = v.VehiculoId,
        ClienteId = v.ClienteId,
        TipoVehiculoId = v.TipoVehiculoId,
        Placa = v.Placa,
        Marca = v.Marca,
        Modelo = v.Modelo,
        Color = v.Color,
        Activo = v.Activo
    };
}