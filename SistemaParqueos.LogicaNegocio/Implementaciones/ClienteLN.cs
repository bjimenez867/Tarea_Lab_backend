using SistemaParqueos.Dominio.DTO;
using SistemaParqueos.Dominio.Entidades;
using SistemaParqueos.Dominio.InterfacesAD;
using SistemaParqueos.Dominio.InterfazLN;
using SistemaParqueos.Utilitarios;

namespace SistemaParqueos.LogicaNegocio.Implementaciones;

public class ClienteLN : IClienteLN
{
    private readonly IUnitOfWork _unitOfWork;

    public ClienteLN(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ClienteDTO>> ObtenerTodosAsync()
    {
        var clientes = await _unitOfWork.Clientes.ObtenerTodosAsync();
        return clientes.Select(MapearADto);
    }

    public async Task<ClienteDTO?> ObtenerPorIdAsync(int id)
    {
        var cliente = await _unitOfWork.Clientes.ObtenerPorIdAsync(id);
        return cliente is null ? null : MapearADto(cliente);
    }

    public async Task<ClienteDTO> CrearAsync(ClienteDTO dto)
    {
        var existente = await _unitOfWork.Clientes.BuscarAsync(c => c.Cedula == dto.Cedula);
        if (existente.Any())
            throw new ValidacionNegocioException("Ya existe un cliente registrado con esa cédula.");

        var cliente = new Cliente
        {
            Nombre = dto.Nombre,
            Apellidos = dto.Apellidos,
            Cedula = dto.Cedula,
            Telefono = dto.Telefono,
            Correo = dto.Correo,
            Activo = dto.Activo,
            CreadoEn = DateTime.UtcNow
        };

        await _unitOfWork.Clientes.AgregarAsync(cliente);
        await _unitOfWork.GuardarCambiosAsync();

        dto.ClienteId = cliente.ClienteId;
        return dto;
    }

    public async Task ActualizarAsync(ClienteDTO dto)
    {
        var cliente = await _unitOfWork.Clientes.ObtenerPorIdAsync(dto.ClienteId)
            ?? throw new ValidacionNegocioException("El cliente no existe.");

        var duplicado = await _unitOfWork.Clientes.BuscarAsync(
            c => c.Cedula == dto.Cedula && c.ClienteId != dto.ClienteId);
        if (duplicado.Any())
            throw new ValidacionNegocioException("Ya existe otro cliente registrado con esa cédula.");

        cliente.Nombre = dto.Nombre;
        cliente.Apellidos = dto.Apellidos;
        cliente.Cedula = dto.Cedula;
        cliente.Telefono = dto.Telefono;
        cliente.Correo = dto.Correo;
        cliente.Activo = dto.Activo;
        cliente.ActualizadoEn = DateTime.UtcNow;

        _unitOfWork.Clientes.Actualizar(cliente);
        await _unitOfWork.GuardarCambiosAsync();
    }

    public async Task EliminarAsync(int id)
    {
        var cliente = await _unitOfWork.Clientes.ObtenerPorIdAsync(id)
            ?? throw new ValidacionNegocioException("El cliente no existe.");

        _unitOfWork.Clientes.Eliminar(cliente);
        await _unitOfWork.GuardarCambiosAsync();
    }

    private static ClienteDTO MapearADto(Cliente c) => new()
    {
        ClienteId = c.ClienteId,
        Nombre = c.Nombre,
        Apellidos = c.Apellidos,
        Cedula = c.Cedula,
        Telefono = c.Telefono,
        Correo = c.Correo,
        Activo = c.Activo
    };
}