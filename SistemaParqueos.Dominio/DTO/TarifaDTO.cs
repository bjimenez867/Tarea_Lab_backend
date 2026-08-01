using System.ComponentModel.DataAnnotations;

namespace SistemaParqueos.Dominio.DTO;

public class TarifaDTO
{
    public int TarifaId { get; set; }

    [Required(ErrorMessage = "El tipo de vehículo es obligatorio")]
    public int TipoVehiculoId { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria")]
    [MaxLength(100)]
    public string Descripcion { get; set; } = null!;

    [Required(ErrorMessage = "El monto por hora es obligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto por hora debe ser mayor a cero")]
    public decimal MontoHora { get; set; }

    public bool Activo { get; set; } = true;
}