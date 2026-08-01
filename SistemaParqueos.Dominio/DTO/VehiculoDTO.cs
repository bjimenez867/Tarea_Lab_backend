using System.ComponentModel.DataAnnotations;

namespace SistemaParqueos.Dominio.DTO;

public class VehiculoDTO
{
    public int VehiculoId { get; set; }

    [Required(ErrorMessage = "El cliente es obligatorio")]
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "El tipo de vehículo es obligatorio")]
    public int TipoVehiculoId { get; set; }

    [Required(ErrorMessage = "La placa es obligatoria")]
    [MaxLength(20)]
    public string Placa { get; set; } = null!;

    [Required(ErrorMessage = "La marca es obligatoria")]
    [MaxLength(100)]
    public string Marca { get; set; } = null!;

    [MaxLength(100)]
    public string? Modelo { get; set; }

    [MaxLength(50)]
    public string? Color { get; set; }

    public bool Activo { get; set; } = true;
}