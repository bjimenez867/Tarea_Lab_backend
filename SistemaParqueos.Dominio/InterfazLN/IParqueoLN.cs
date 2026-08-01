using SistemaParqueos.Dominio.DTO;

namespace SistemaParqueos.Dominio.InterfazLN;

public interface IParqueoLN
{
    Task<IEnumerable<ParqueoDTO>> ObtenerTodosAsync();
    Task<ParqueoDTO?> ObtenerPorIdAsync(int id);
    Task<ParqueoDTO> CrearAsync(ParqueoDTO dto);
    Task ActualizarAsync(ParqueoDTO dto);
    Task EliminarAsync(int id);
}