using SistemaParqueos.AccesoDatos.Contexto;
using SistemaParqueos.Dominio.Entidades;
using SistemaParqueos.Dominio.InterfacesAD;

namespace SistemaParqueos.AccesoDatos.Implementaciones;

public class UnitOfWork : IUnitOfWork
{
    private readonly ParqueosDbContext _contexto;

    private IRepositorioGenerico<Cliente>? _clientes;
    private IRepositorioGenerico<Vehiculo>? _vehiculos;
    private IRepositorioGenerico<TipoVehiculo>? _tiposVehiculo;
    private IRepositorioGenerico<Parqueo>? _parqueos;
    private IRepositorioGenerico<EspacioParqueo>? _espaciosParqueo;
    private IRepositorioGenerico<Tarifa>? _tarifas;
    private IRepositorioGenerico<IngresoVehiculo>? _ingresosVehiculo;
    private IRepositorioGenerico<Factura>? _facturas;

    public UnitOfWork(ParqueosDbContext contexto)
    {
        _contexto = contexto;
    }

    public IRepositorioGenerico<Cliente> Clientes =>
        _clientes ??= new RepositorioGenerico<Cliente>(_contexto);

    public IRepositorioGenerico<Vehiculo> Vehiculos =>
        _vehiculos ??= new RepositorioGenerico<Vehiculo>(_contexto);

    public IRepositorioGenerico<TipoVehiculo> TiposVehiculo =>
        _tiposVehiculo ??= new RepositorioGenerico<TipoVehiculo>(_contexto);

    public IRepositorioGenerico<Parqueo> Parqueos =>
        _parqueos ??= new RepositorioGenerico<Parqueo>(_contexto);

    public IRepositorioGenerico<EspacioParqueo> EspaciosParqueo =>
        _espaciosParqueo ??= new RepositorioGenerico<EspacioParqueo>(_contexto);

    public IRepositorioGenerico<Tarifa> Tarifas =>
        _tarifas ??= new RepositorioGenerico<Tarifa>(_contexto);

    public IRepositorioGenerico<IngresoVehiculo> IngresosVehiculo =>
        _ingresosVehiculo ??= new RepositorioGenerico<IngresoVehiculo>(_contexto);

    public IRepositorioGenerico<Factura> Facturas =>
        _facturas ??= new RepositorioGenerico<Factura>(_contexto);

    public async Task<int> GuardarCambiosAsync()
    {
        return await _contexto.SaveChangesAsync();
    }

    public void Dispose()
    {
        _contexto.Dispose();
        GC.SuppressFinalize(this);
    }
}