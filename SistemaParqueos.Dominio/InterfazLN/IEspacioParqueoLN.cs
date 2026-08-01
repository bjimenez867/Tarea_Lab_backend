using SistemaParqueos.Dominio.DTO;

namespace SistemaParqueos.Dominio.InterfazLN;

public interface IEspacioParqueoLN
{
    Task<IEnumerable<EspacioParqueoDTO>> ObtenerTodosAsync();
    Task<EspacioParqueoDTO?> ObtenerPorIdAsync(int id);
    Task<EspacioParqueoDTO> CrearAsync(EspacioParqueoDTO dto);
    Task ActualizarAsync(EspacioParqueoDTO dto);
    Task EliminarAsync(int id);
}