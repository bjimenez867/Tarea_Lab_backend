using SistemaParqueos.Dominio.DTO;
using SistemaParqueos.Dominio.Entidades;
using SistemaParqueos.Dominio.InterfacesAD;
using SistemaParqueos.Dominio.InterfazLN;
using SistemaParqueos.Utilidades;

namespace SistemaParqueos.LogicaNegocio.Implementaciones;

public class EspacioParqueoLN : IEspacioParqueoLN
{
    private readonly IUnitOfWork _unitOfWork;

    public EspacioParqueoLN(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<EspacioParqueoDTO>> ObtenerTodosAsync()
    {
        var espacios = await _unitOfWork.EspaciosParqueo.ObtenerTodosAsync();
        return espacios.Select(MapearADto);
    }

    public async Task<EspacioParqueoDTO?> ObtenerPorIdAsync(int id)
    {
        var espacio = await _unitOfWork.EspaciosParqueo.ObtenerPorIdAsync(id);
        return espacio is null ? null : MapearADto(espacio);
    }

    public async Task<EspacioParqueoDTO> CrearAsync(EspacioParqueoDTO dto)
    {
        var parqueo = await _unitOfWork.Parqueos.ObtenerPorIdAsync(dto.ParqueoId)
            ?? throw new ValidacionNegocioException("El parqueo indicado no existe.");

        var existente = await _unitOfWork.EspaciosParqueo.BuscarAsync(
            e => e.ParqueoId == dto.ParqueoId && e.NumeroEspacio == dto.NumeroEspacio);
        if (existente.Any())
            throw new ValidacionNegocioException("Ya existe un espacio con ese número en este parqueo.");

        var espacio = new EspacioParqueo
        {
            ParqueoId = dto.ParqueoId,
            NumeroEspacio = dto.NumeroEspacio,
            Disponible = dto.Disponible,
            Activo = dto.Activo,
            CreadoEn = DateTime.UtcNow
        };

        await _unitOfWork.EspaciosParqueo.AgregarAsync(espacio);
        await _unitOfWork.GuardarCambiosAsync();

        dto.EspacioId = espacio.EspacioId;
        return dto;
    }

    public async Task ActualizarAsync(EspacioParqueoDTO dto)
    {
        var espacio = await _unitOfWork.EspaciosParqueo.ObtenerPorIdAsync(dto.EspacioId)
            ?? throw new ValidacionNegocioException("El espacio no existe.");

        var duplicado = await _unitOfWork.EspaciosParqueo.BuscarAsync(
            e => e.ParqueoId == dto.ParqueoId && e.NumeroEspacio == dto.NumeroEspacio && e.EspacioId != dto.EspacioId);
        if (duplicado.Any())
            throw new ValidacionNegocioException("Ya existe otro espacio con ese número en este parqueo.");

        espacio.ParqueoId = dto.ParqueoId;
        espacio.NumeroEspacio = dto.NumeroEspacio;
        espacio.Disponible = dto.Disponible;
        espacio.Activo = dto.Activo;
        espacio.ActualizadoEn = DateTime.UtcNow;

        _unitOfWork.EspaciosParqueo.Actualizar(espacio);
        await _unitOfWork.GuardarCambiosAsync();
    }

    public async Task EliminarAsync(int id)
    {
        var espacio = await _unitOfWork.EspaciosParqueo.ObtenerPorIdAsync(id)
            ?? throw new ValidacionNegocioException("El espacio no existe.");

        _unitOfWork.EspaciosParqueo.Eliminar(espacio);
        await _unitOfWork.GuardarCambiosAsync();
    }

    private static EspacioParqueoDTO MapearADto(EspacioParqueo e) => new()
    {
        EspacioId = e.EspacioId,
        ParqueoId = e.ParqueoId,
        NumeroEspacio = e.NumeroEspacio,
        Disponible = e.Disponible,
        Activo = e.Activo
    };
}