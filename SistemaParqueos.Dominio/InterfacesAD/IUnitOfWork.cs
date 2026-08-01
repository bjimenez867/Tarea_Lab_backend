using SistemaParqueos.Dominio.Entidades;

namespace SistemaParqueos.Dominio.InterfacesAD;

public interface IUnitOfWork : IDisposable
{
    IRepositorioGenerico<Cliente> Clientes { get; }
    IRepositorioGenerico<Vehiculo> Vehiculos { get; }
    IRepositorioGenerico<TipoVehiculo> TiposVehiculo { get; }
    IRepositorioGenerico<Parqueo> Parqueos { get; }
    IRepositorioGenerico<EspacioParqueo> EspaciosParqueo { get; }
    IRepositorioGenerico<Tarifa> Tarifas { get; }
    IRepositorioGenerico<IngresoVehiculo> IngresosVehiculo { get; }
    IRepositorioGenerico<Factura> Facturas { get; }

    Task<int> GuardarCambiosAsync();
}