using SistemaParqueos.Dominio.DTO;

namespace SistemaParqueos.Dominio.InterfazLN;

public interface IVehiculoLN
{
    Task<IEnumerable<VehiculoDTO>> ObtenerTodosAsync();
    Task<VehiculoDTO?> ObtenerPorIdAsync(int id);
    Task<VehiculoDTO> CrearAsync(VehiculoDTO dto);
    Task ActualizarAsync(VehiculoDTO dto);
    Task EliminarAsync(int id);
}