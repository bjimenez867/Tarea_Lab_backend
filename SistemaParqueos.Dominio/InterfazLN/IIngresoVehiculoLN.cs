using SistemaParqueos.Dominio.DTO;

namespace SistemaParqueos.Dominio.InterfazLN;

public interface IIngresoVehiculoLN
{
    Task<IEnumerable<IngresoVehiculoDTO>> ObtenerTodosAsync();
    Task<IngresoVehiculoDTO?> ObtenerPorIdAsync(int id);
    Task<IngresoVehiculoDTO> CrearAsync(IngresoVehiculoDTO dto);
    Task ActualizarAsync(IngresoVehiculoDTO dto);
    Task EliminarAsync(int id);

    // Método específico de negocio: registrar la salida de un vehículo
    Task RegistrarSalidaAsync(int ingresoId);
}