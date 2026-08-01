using System.ComponentModel.DataAnnotations;

namespace SistemaParqueos.Dominio.DTO;

public class ParqueoDTO
{
    public int ParqueoId { get; set; }

    [Required(ErrorMessage = "El nombre del parqueo es obligatorio")]
    [MaxLength(150)]
    public string NombreParqueo { get; set; } = null!;

    [Required(ErrorMessage = "La dirección es obligatoria")]
    [MaxLength(250)]
    public string Direccion { get; set; } = null!;

    [MaxLength(25)]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "La capacidad total es obligatoria")]
    [Range(1, int.MaxValue, ErrorMessage = "La capacidad debe ser mayor a cero")]
    public int CapacidadTotal { get; set; }

    public bool Activo { get; set; } = true;
}