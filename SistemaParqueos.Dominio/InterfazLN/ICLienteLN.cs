using SistemaParqueos.Dominio.DTO;

namespace SistemaParqueos.Dominio.InterfazLN;

public interface IClienteLN
{
    Task<IEnumerable<ClienteDTO>> ObtenerTodosAsync();
    Task<ClienteDTO?> ObtenerPorIdAsync(int id);
    Task<ClienteDTO> CrearAsync(ClienteDTO dto);
    Task ActualizarAsync(ClienteDTO dto);
    Task EliminarAsync(int id);
}