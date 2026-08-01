using System.Linq.Expressions;

namespace SistemaParqueos.Dominio.InterfacesAD;

public interface IRepositorioGenerico<T> where T : class
{
    Task<IEnumerable<T>> ObtenerTodosAsync();
    Task<T?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> filtro);
    Task AgregarAsync(T entidad);
    void Actualizar(T entidad);
    void Eliminar(T entidad);
}