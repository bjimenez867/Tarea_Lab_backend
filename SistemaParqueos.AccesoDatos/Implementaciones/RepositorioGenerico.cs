using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SistemaParqueos.AccesoDatos.Contexto;
using SistemaParqueos.Dominio.InterfacesAD;

namespace SistemaParqueos.AccesoDatos.Implementaciones;

public class RepositorioGenerico<T> : IRepositorioGenerico<T> where T : class
{
    private readonly ParqueosDbContext _contexto;
    private readonly DbSet<T> _dbSet;

    public RepositorioGenerico(ParqueosDbContext contexto)
    {
        _contexto = contexto;
        _dbSet = _contexto.Set<T>();
    }

    public async Task<IEnumerable<T>> ObtenerTodosAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> ObtenerPorIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> filtro)
    {
        return await _dbSet.Where(filtro).ToListAsync();
    }

    public async Task AgregarAsync(T entidad)
    {
        await _dbSet.AddAsync(entidad);
    }

    public void Actualizar(T entidad)
    {
        _dbSet.Update(entidad);
    }

    public void Eliminar(T entidad)
    {
        _dbSet.Remove(entidad);
    }
}