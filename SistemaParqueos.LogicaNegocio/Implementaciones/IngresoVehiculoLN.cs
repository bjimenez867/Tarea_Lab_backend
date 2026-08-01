using SistemaParqueos.Dominio.DTO;
using SistemaParqueos.Dominio.Entidades;
using SistemaParqueos.Dominio.InterfacesAD;
using SistemaParqueos.Dominio.InterfazLN;
using SistemaParqueos.Utilidades;

namespace SistemaParqueos.LogicaNegocio.Implementaciones;

public class IngresoVehiculoLN : IIngresoVehiculoLN
{
    private readonly IUnitOfWork _unitOfWork;

    public IngresoVehiculoLN(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<IngresoVehiculoDTO>> ObtenerTodosAsync()
    {
        var ingresos = await _unitOfWork.IngresosVehiculo.ObtenerTodosAsync();
        return ingresos.Select(MapearADto);
    }

    public async Task<IngresoVehiculoDTO?> ObtenerPorIdAsync(int id)
    {
        var ingreso = await _unitOfWork.IngresosVehiculo.ObtenerPorIdAsync(id);
        return ingreso is null ? null : MapearADto(ingreso);
    }

    public async Task<IngresoVehiculoDTO> CrearAsync(IngresoVehiculoDTO dto)
    {
        var vehiculo = await _unitOfWork.Vehiculos.ObtenerPorIdAsync(dto.VehiculoId)
            ?? throw new ValidacionNegocioException("El vehículo indicado no existe.");

        var espacio = await _unitOfWork.EspaciosParqueo.ObtenerPorIdAsync(dto.EspacioId)
            ?? throw new ValidacionNegocioException("El espacio indicado no existe.");

        if (!espacio.Disponible)
            throw new ValidacionNegocioException("No hay espacios disponibles: el espacio seleccionado ya está ocupado.");

        var ingreso = new IngresoVehiculo
        {
            VehiculoId = dto.VehiculoId,
            EspacioId = dto.EspacioId,
            FechaIngreso = DateTime.UtcNow,
            Estado = "Activo",
            CreadoEn = DateTime.UtcNow
        };

        await _unitOfWork.IngresosVehiculo.AgregarAsync(ingreso);

        espacio.Disponible = false;
        espacio.ActualizadoEn = DateTime.UtcNow;
        _unitOfWork.EspaciosParqueo.Actualizar(espacio);

        await _unitOfWork.GuardarCambiosAsync();

        dto.IngresoId = ingreso.IngresoId;
        dto.FechaIngreso = ingreso.FechaIngreso;
        dto.Estado = ingreso.Estado;
        return dto;
    }

    public async Task ActualizarAsync(IngresoVehiculoDTO dto)
    {
        var ingreso = await _unitOfWork.IngresosVehiculo.ObtenerPorIdAsync(dto.IngresoId)
            ?? throw new ValidacionNegocioException("El ingreso no existe.");

        ingreso.Estado = dto.Estado ?? ingreso.Estado;
        ingreso.FechaSalida = dto.FechaSalida;
        ingreso.ActualizadoEn = DateTime.UtcNow;

        _unitOfWork.IngresosVehiculo.Actualizar(ingreso);
        await _unitOfWork.GuardarCambiosAsync();
    }

    public async Task EliminarAsync(int id)
    {
        var ingreso = await _unitOfWork.IngresosVehiculo.ObtenerPorIdAsync(id)
            ?? throw new ValidacionNegocioException("El ingreso no existe.");

        _unitOfWork.IngresosVehiculo.Eliminar(ingreso);
        await _unitOfWork.GuardarCambiosAsync();
    }

    public async Task RegistrarSalidaAsync(int ingresoId)
    {
        var ingreso = await _unitOfWork.IngresosVehiculo.ObtenerPorIdAsync(ingresoId)
            ?? throw new ValidacionNegocioException("El ingreso no existe.");

        if (ingreso.Estado == "Finalizado")
            throw new ValidacionNegocioException("Este ingreso ya tiene registrada su salida.");

        var espacio = await _unitOfWork.EspaciosParqueo.ObtenerPorIdAsync(ingreso.EspacioId)
            ?? throw new ValidacionNegocioException("El espacio asociado no existe.");

        ingreso.FechaSalida = DateTime.UtcNow;
        ingreso.Estado = "Finalizado";
        ingreso.ActualizadoEn = DateTime.UtcNow;
        _unitOfWork.IngresosVehiculo.Actualizar(ingreso);

        espacio.Disponible = true;
        espacio.ActualizadoEn = DateTime.UtcNow;
        _unitOfWork.EspaciosParqueo.Actualizar(espacio);

        await _unitOfWork.GuardarCambiosAsync();
    }

    private static IngresoVehiculoDTO MapearADto(IngresoVehiculo i) => new()
    {
        IngresoId = i.IngresoId,
        VehiculoId = i.VehiculoId,
        EspacioId = i.EspacioId,
        FechaIngreso = i.FechaIngreso,
        FechaSalida = i.FechaSalida,
        Estado = i.Estado
    };
}