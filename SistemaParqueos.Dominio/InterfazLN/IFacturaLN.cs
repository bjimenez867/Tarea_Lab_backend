using SistemaParqueos.Dominio.DTO;

namespace SistemaParqueos.Dominio.InterfazLN;

public interface IFacturaLN
{
    Task<IEnumerable<FacturaDTO>> ObtenerTodosAsync();
    Task<FacturaDTO?> ObtenerPorIdAsync(int id);
    Task<FacturaDTO> CrearAsync(FacturaDTO dto);
    Task ActualizarAsync(FacturaDTO dto);
    Task EliminarAsync(int id);
}