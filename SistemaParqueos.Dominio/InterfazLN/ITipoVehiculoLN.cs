using SistemaParqueos.Dominio.DTO;

namespace SistemaParqueos.Dominio.InterfazLN;

public interface ITipoVehiculoLN
{
    Task<IEnumerable<TipoVehiculoDTO>> ObtenerTodosAsync();
    Task<TipoVehiculoDTO?> ObtenerPorIdAsync(int id);
    Task<TipoVehiculoDTO> CrearAsync(TipoVehiculoDTO dto);
    Task ActualizarAsync(TipoVehiculoDTO dto);
    Task EliminarAsync(int id);
}