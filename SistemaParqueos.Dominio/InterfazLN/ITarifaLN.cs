using SistemaParqueos.Dominio.DTO;

namespace SistemaParqueos.Dominio.InterfazLN;

public interface ITarifaLN
{
    Task<IEnumerable<TarifaDTO>> ObtenerTodosAsync();
    Task<TarifaDTO?> ObtenerPorIdAsync(int id);
    Task<TarifaDTO> CrearAsync(TarifaDTO dto);
    Task ActualizarAsync(TarifaDTO dto);
    Task EliminarAsync(int id);
}