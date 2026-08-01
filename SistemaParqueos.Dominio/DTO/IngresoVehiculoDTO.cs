using System.ComponentModel.DataAnnotations;

namespace SistemaParqueos.Dominio.DTO;

public class IngresoVehiculoDTO
{
    public int IngresoId { get; set; }

    [Required(ErrorMessage = "El vehículo es obligatorio")]
    public int VehiculoId { get; set; }

    [Required(ErrorMessage = "El espacio es obligatorio")]
    public int EspacioId { get; set; }

    public DateTime FechaIngreso { get; set; }

    public DateTime? FechaSalida { get; set; }

    [MaxLength(20)]
    public string Estado { get; set; } = null!;
}