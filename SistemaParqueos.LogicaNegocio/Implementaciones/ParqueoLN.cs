using SistemaParqueos.Dominio.DTO;
using SistemaParqueos.Dominio.Entidades;
using SistemaParqueos.Dominio.InterfacesAD;
using SistemaParqueos.Dominio.InterfazLN;
using SistemaParqueos.Utilitarios;

namespace SistemaParqueos.LogicaNegocio.Implementaciones;

public class ParqueoLN : IParqueoLN
{
    private readonly IUnitOfWork _unitOfWork;

    public ParqueoLN(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ParqueoDTO>> ObtenerTodosAsync()
    {
        var parqueos = await _unitOfWork.Parqueos.ObtenerTodosAsync();
        return parqueos.Select(MapearADto);
    }

    public async Task<ParqueoDTO?> ObtenerPorIdAsync(int id)
    {
        var parqueo = await _unitOfWork.Parqueos.ObtenerPorIdAsync(id);
        return parqueo is null ? null : MapearADto(parqueo);
    }

    public async Task<ParqueoDTO> CrearAsync(ParqueoDTO dto)
    {
        if (dto.CapacidadTotal <= 0)
            throw new ValidacionNegocioException("La capacidad total debe ser mayor a cero.");

        var parqueo = new Parqueo
        {
            NombreParqueo = dto.NombreParqueo,
            Direccion = dto.Direccion,
            Telefono = dto.Telefono,
            CapacidadTotal = dto.CapacidadTotal,
            Activo = dto.Activo,
            CreadoEn = DateTime.UtcNow
        };

        await _unitOfWork.Parqueos.AgregarAsync(parqueo);
        await _unitOfWork.GuardarCambiosAsync();

        dto.ParqueoId = parqueo.ParqueoId;
        return dto;
    }

    public async Task ActualizarAsync(ParqueoDTO dto)
    {
        if (dto.CapacidadTotal <= 0)
            throw new ValidacionNegocioException("La capacidad total debe ser mayor a cero.");

        var parqueo = await _unitOfWork.Parqueos.ObtenerPorIdAsync(dto.ParqueoId)
            ?? throw new ValidacionNegocioException("El parqueo no existe.");

        parqueo.NombreParqueo = dto.NombreParqueo;
        parqueo.Direccion = dto.Direccion;
        parqueo.Telefono = dto.Telefono;
        parqueo.CapacidadTotal = dto.CapacidadTotal;
        parqueo.Activo = dto.Activo;
        parqueo.ActualizadoEn = DateTime.UtcNow;

        _unitOfWork.Parqueos.Actualizar(parqueo);
        await _unitOfWork.GuardarCambiosAsync();
    }

    public async Task EliminarAsync(int id)
    {
        var parqueo = await _unitOfWork.Parqueos.ObtenerPorIdAsync(id)
            ?? throw new ValidacionNegocioException("El parqueo no existe.");

        _unitOfWork.Parqueos.Eliminar(parqueo);
        await _unitOfWork.GuardarCambiosAsync();
    }

    private static ParqueoDTO MapearADto(Parqueo p) => new()
    {
        ParqueoId = p.ParqueoId,
        NombreParqueo = p.NombreParqueo,
        Direccion = p.Direccion,
        Telefono = p.Telefono,
        CapacidadTotal = p.CapacidadTotal,
        Activo = p.Activo
    };
}